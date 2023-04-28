﻿using System.Runtime.CompilerServices;
using DynamicData.Diagnostics;
using Stellar.Maui.Exceptions;
using Stellar.ViewModel;

namespace Stellar.Maui;

public static class VisualElementExtensions
{
    public static TPage GetPage<TPage>(this Element element)
        where TPage : Page
    {
        return element.FindMauiContext().Services.GetService<TPage>().ThrowIfNull();
    }

    public static TViewModel GetViewModel<TViewModel>(this Element element)
        where TViewModel : ViewModelBase
    {
        return element.FindMauiContext().Services.GetService<TViewModel>().ThrowIfNull();
    }

    public static TService GetService<TService>(this Element element)
    {
        return element.FindMauiContext().Services.GetService<TService>().ThrowIfNull();
    }

    private static T ThrowIfNull<T>(this T obj)
    {
        if (obj is null)
        {
            throw new RegisteredServiceNotFoundException(
                $@"""{typeof(T).Name} must be registered as a service in order to be resolved.
                    Please use the ServiceRegistration attribute or register the component with the MauiAppBuilder.");
        }

        return obj;
    }

    internal static IEnumerable<Element> GetParentsPath(this Element self)
    {
        Element current = self;

        while (!current.RealParent.IsApplicationOrNull())
        {
            current = current.RealParent;
            yield return current;
        }
    }

    internal static bool IsApplicationOrNull(this Element element)
        => element is null || element is IApplication;

    internal static bool IsApplicationOrWindowOrNull(this Element element)
        => element is null || element is IApplication || element is IWindow;

    private static IMauiContext FindMauiContext(this Element element)
    {
        if (element is IElement fe && fe.Handler?.MauiContext is not null)
        {
            return fe.Handler.MauiContext;
        }

        foreach (var parent in element.GetParentsPath())
        {
            if (parent is IElement parentView && parentView.Handler?.MauiContext is not null)
            {
                return parentView.Handler.MauiContext;
            }
        }

        return Application.Current?.FindMauiContext();
    }
}