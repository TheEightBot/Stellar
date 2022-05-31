﻿using System.Reflection;
using EightBot.Stellar.ViewModel;

namespace EightBot.Stellar.Maui;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder PreCacheComponents<TStellarAssembly>(this MauiAppBuilder mauiApp)
    {
        PreCache(typeof(TStellarAssembly).GetTypeInfo().Assembly);

        return mauiApp;
    }

    public static MauiAppBuilder RegisterViewModels<TStellarAssembly>(this MauiAppBuilder mauiAppBuilder)
    {
        Register<IViewModel>(mauiAppBuilder, typeof(TStellarAssembly).GetTypeInfo().Assembly);

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterServices<TStellarAssembly>(this MauiAppBuilder mauiAppBuilder)
    {
        Register<IService>(mauiAppBuilder, typeof(TStellarAssembly).GetTypeInfo().Assembly);

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews<TStellarAssembly>(this MauiAppBuilder mauiAppBuilder)
    {
        Register<IStellarView>(mauiAppBuilder, typeof(TStellarAssembly).GetTypeInfo().Assembly);

        return mauiAppBuilder;
    }

    private static MauiAppBuilder Register<T>(this MauiAppBuilder mauiAppBuilder, Assembly assembly)
    {
#if DEBUG
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var sb = new System.Text.StringBuilder();
        var totalTime = 0L;

#endif

        var registrationType = typeof(T);

#if DEBUG
        sb
            .AppendLine()
            .AppendLine("------------------------------------------------------------------------------------------")
            .AppendLine($" Registering {registrationType} - Start")
            .AppendLine("------------------------------------------------------------------------------------------");
#endif

        var assTypes =
            assembly
                ?.ExportedTypes
                ?.Where(ti => ti.IsAssignableTo(registrationType) && !ti.IsAbstract)
                ?? Enumerable.Empty<Type>();
#if DEBUG
        sb
            .AppendLine($" Assembly Loading\t-\t{sw.ElapsedMilliseconds:N1}ms")
            .AppendLine()
            .AppendLine("-------------------------")
            .AppendLine($" Registering {registrationType}")
            .AppendLine();
#endif

        foreach (var ti in assTypes)
        {
            mauiAppBuilder.Services.AddTransient(ti);
        }

#if DEBUG
        sw.Stop();

        sb
            .AppendLine()
            .AppendLine($" {"Total Loading":-50} \t-\t{totalTime:N1}ms")
            .AppendLine("-------------------------")
            .AppendLine()
            .AppendLine("------------------------------------------------------------------------------------------")
            .AppendLine($" Registering {registrationType} - End")
            .AppendLine("------------------------------------------------------------------------------------------");

        System.Diagnostics.Debug.WriteLine(sw.ToString());
#endif

        return mauiAppBuilder;
    }

    private static Task PreCache(Assembly assembly)
    {
        return Task.Run(
            () =>
            {
#if DEBUG
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var sb = new System.Text.StringBuilder();
                var totalTime = 0L;

                sb
                    .AppendLine()
                    .AppendLine("------------------------------------------------------------------------------------------")
                    .AppendLine(" Precaching - Start")
                    .AppendLine("------------------------------------------------------------------------------------------");
#endif
                var precacheAttribute = typeof(PreCacheAttribute);

                var assTypes =
                    assembly
                        ?.ExportedTypes
                        ?.Where(
                            ti =>
                                Attribute.IsDefined(ti, precacheAttribute) &&
                                ti.IsClass && !ti.IsAbstract &&
                                ti.GetConstructor(Type.EmptyTypes) != null && !ti.ContainsGenericParameters)
                        ?? Enumerable.Empty<Type>();
#if DEBUG
                sb
                    .AppendLine($" Assembly Loading\t-\t{sw.ElapsedMilliseconds:N1}ms")
                    .AppendLine()
                    .AppendLine("-------------------------")
                    .AppendLine(" Precaching Views and View Models")
                    .AppendLine();
#endif

                foreach (var ti in assTypes)
                {
                    try
                    {
#if DEBUG
                        sw.Restart();
#endif

                        assembly.CreateInstance(ti.FullName);

#if DEBUG
                        var elapsed = sw.ElapsedMilliseconds;
                        totalTime += elapsed;
                        sb.AppendLine($" {ti.Name,-50}\t-\t{elapsed:N1}ms");
#endif
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        sb
                            .AppendLine($" {ti.Name,-50}\t-\tException: {ex}ms")
                            .AppendLine()
                            .AppendLine($"{ex}")
                            .AppendLine();
#endif
                    }
                }

#if DEBUG
                sw.Stop();

                sb
                    .AppendLine()
                    .AppendLine($" {"Total View and View Model Loading":-50} \t-\t{totalTime:N1}ms")
                    .AppendLine("-------------------------")
                    .AppendLine()
                    .AppendLine("------------------------------------------------------------------------------------------")
                    .AppendLine(" Precaching - End")
                    .AppendLine("------------------------------------------------------------------------------------------");

                System.Diagnostics.Debug.WriteLine(sw.ToString());
#endif
            })
        .ContinueWith(
            result =>
            {
#if DEBUG
                if (result.IsFaulted)
                {
                    System.Diagnostics.Debug.WriteLine(result.Exception);
                }
#endif
            });
    }
}