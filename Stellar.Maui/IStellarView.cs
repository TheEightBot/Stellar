﻿using System.ComponentModel;

namespace Stellar.Maui;

public interface IStellarView<TViewModel> : IViewFor<TViewModel>, IStellarView
    where TViewModel : class
{
}

public interface IStellarView : IDisposable, IMaintain
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ViewManager ViewManager { get; set; }

    public void Initialize();

    public void SetupUserInterface();

    public void BindControls();
}