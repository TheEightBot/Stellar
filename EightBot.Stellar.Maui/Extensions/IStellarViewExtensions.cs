﻿using System;
using EightBot.Stellar.ViewModel;

namespace EightBot.Stellar.Maui;

public static class IStellarViewExtensions
{
    public static void InitializeComponent<TViewModel>(this IStellarView<TViewModel> stellarView, bool delayBindingRegistrationUntilAttached = false)
        where TViewModel : class
    {
        stellarView.SetupViewModel();

        if (Attribute.GetCustomAttribute(stellarView.GetType(), typeof(ServiceRegistrationAttribute)) is ServiceRegistrationAttribute sra)
        {
            switch (sra.ServiceRegistrationType)
            {
                case Lifetime.Scoped:
                case Lifetime.Singleton:
                    stellarView.MaintainBindings = true;
                    break;
            }
        }

        stellarView.Initialize();

        stellarView.SetupUserInterface();

        if (!delayBindingRegistrationUntilAttached)
        {
            stellarView.ViewManager.RegisterBindings(stellarView);
        }
    }

    public static void ManageDispose<TViewModel>(this IStellarView<TViewModel> stellarView, bool disposing, ref bool isDisposed)
        where TViewModel : class
    {
        if (!isDisposed)
        {
            isDisposed = true;

            if (disposing)
            {
                stellarView.ViewManager?.Dispose();
                stellarView.ViewModel = null;

                stellarView.DisposeViewModel();
            }
        }
    }
}