﻿using System.ComponentModel;

namespace Stellar.Maui.Views;

public abstract class GridBase<TViewModel> : ReactiveGrid<TViewModel>, IStellarView<TViewModel>
    where TViewModel : class
{
    private bool _isDisposed;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ViewManager ViewManager { get; } = new MauiViewManager();

    public IObservable<Unit> Activated => ViewManager.Activated;

    public IObservable<Unit> Deactivated => ViewManager.Deactivated;

    public IObservable<Unit> IsAppearing => ViewManager.IsAppearing;

    public IObservable<Unit> IsDisappearing => ViewManager.IsDisappearing;

    public IObservable<LifecycleEvent> Lifecycle => ViewManager.Lifecycle;

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

    protected override void OnHandlerChanging(HandlerChangingEventArgs args)
    {
        ((MauiViewManager)ViewManager).HandlerChanging(this, args);

        base.OnHandlerChanging(args);
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        ViewManager.PropertyChanged<GridBase<TViewModel>, TViewModel>(this, propertyName);

        base.OnPropertyChanged(propertyName);
    }

    protected virtual void Dispose(bool disposing) =>
        this.ManageDispose(disposing, ref _isDisposed);

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

public abstract class GridBase<TViewModel, TDataModel> : ReactiveGrid<TViewModel>, IStellarView<TViewModel>
    where TViewModel : class
{
    private bool _isDisposed;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ViewManager ViewManager { get; } = new MauiViewManager();

    public IObservable<Unit> Activated => ViewManager.Activated;

    public IObservable<Unit> Deactivated => ViewManager.Deactivated;

    public IObservable<Unit> IsAppearing => ViewManager.IsAppearing;

    public IObservable<Unit> IsDisappearing => ViewManager.IsDisappearing;

    public IObservable<LifecycleEvent> Lifecycle => ViewManager.Lifecycle;

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

    protected abstract void MapDataModelToViewModel(TViewModel viewModel, TDataModel dataModel);

    protected override void OnHandlerChanging(HandlerChangingEventArgs args)
    {
        ((MauiViewManager)ViewManager).HandlerChanging(this, args);

        base.OnHandlerChanging(args);
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        ViewManager.PropertyChanged<GridBase<TViewModel, TDataModel>, TViewModel>(this, propertyName);
        base.OnPropertyChanged(propertyName);
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (ViewModel is not null && BindingContext is TDataModel dataModel)
        {
            MapDataModelToViewModel(ViewModel, dataModel);
        }
    }

    protected virtual void Dispose(bool disposing) =>
        this.ManageDispose(disposing, ref _isDisposed);

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}