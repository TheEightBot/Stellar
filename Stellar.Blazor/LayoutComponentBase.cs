﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI.Blazor;

namespace Stellar.Blazor;

public abstract class LayoutComponentBase<TViewModel> : ReactiveLayoutComponentBase<TViewModel>, IStellarView<TViewModel>
    where TViewModel : class, INotifyPropertyChanged
{
    private bool _isDisposed;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ViewManager ViewManager { get; } = new BlazorViewManager();

    public CompositeDisposable ControlBindings => ViewManager.ControlBindings;

    public bool Maintain
    {
        get => ViewManager.Maintain;
        set => ViewManager.Maintain = value;
    }

    public virtual void Initialize()
    {
    }

    public abstract void SetupUserInterface();

    public abstract void BindControls();

    protected override void OnInitialized()
    {
        ViewManager.HandleActivated(this);

        base.OnInitialized();
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        ViewManager.PropertyChanged<LayoutComponentBase<TViewModel>, TViewModel>(this, propertyName);

        base.OnPropertyChanged(propertyName);
    }

    protected override void Dispose(bool disposing)
    {
        this.ManageDispose(disposing, ref _isDisposed);

        base.Dispose(disposing);
    }
}