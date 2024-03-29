﻿using System;
using System.Runtime.CompilerServices;
using ReactiveUI.Fody.Helpers;

namespace Stellar.ViewModel;

public class SelectionViewModel<TSelectedValKey> : ViewModelBase
{
    [Reactive]
    public TSelectedValKey? Key { get; set; }

    [Reactive]
    public string? DisplayValue { get; set; }

    [Reactive]
    public bool Selected { get; set; }

    [Reactive]
    public ReactiveCommand<Unit, bool>? ToggleSelected { get; private set; }

    protected override void Bind(CompositeDisposable disposables)
    {
        ToggleSelected =
            ReactiveCommand
                .Create(() => Selected = !Selected)
                .DisposeWith(disposables);
    }
}