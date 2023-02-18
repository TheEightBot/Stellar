﻿using Stellar.ViewModel;

namespace Stellar.Maui;

public static class IViewForExtensions
{
    public static void SetupViewModel<TViewModel>(
        this IViewFor<TViewModel> view,
        TViewModel viewModel = null,
        bool resolveViewModel = true)
        where TViewModel : class
    {
        if (viewModel is not null && !viewModel.Equals(view.ViewModel))
        {
            view.ViewModel = viewModel;
        }

        if (view.ViewModel is null && resolveViewModel && view is Element element)
        {
            view.ViewModel = element.GetService<TViewModel>();
        }

        if (view.ViewModel is not null && view.ViewModel is ViewModelBase vmb)
        {
            if (Attribute.GetCustomAttribute(typeof(TViewModel), typeof(ServiceRegistrationAttribute)) is ServiceRegistrationAttribute sra)
            {
                switch (sra.ServiceRegistrationType)
                {
                    case Lifetime.Scoped:
                    case Lifetime.Singleton:
                        vmb.Maintain = true;
                        break;
                }
            }

            vmb.SetupViewModel();
        }
    }

    public static void RegisterViewModelBindings<TViewModel>(this IViewFor<TViewModel> view)
            where TViewModel : class
    {
        if (view.ViewModel is not null && view.ViewModel is ViewModelBase vmb)
        {
            vmb.RegisterBindings();
        }
    }

    public static void UnregisterViewModelBindings<TViewModel>(this IViewFor<TViewModel> view)
            where TViewModel : class
    {
        if (view.ViewModel is not null && view.ViewModel is ViewModelBase vmb)
        {
            vmb.UnregisterBindings();
        }
    }

    public static void DisposeViewModel<TViewModel>(this IViewFor<TViewModel> view)
        where TViewModel : class
    {
        if (view.ViewModel is not null && view.ViewModel is ViewModelBase vmb && !vmb.Maintain)
        {
            vmb.Dispose();
            view.ViewModel = null;
        }
    }

    public static void DisposeView(this IStellarView ve)
    {
        if (ve is not null && ve is IStellarView isv && !isv.Maintain)
        {
            isv.Dispose();
        }
    }
}