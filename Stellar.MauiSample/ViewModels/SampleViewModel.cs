﻿using System.ComponentModel;
using Stellar.MauiSample.Services;

namespace Stellar.MauiSample.ViewModels;

[ServiceRegistration]
public class SampleViewModel : ViewModelBase
{
    private readonly TestService _testService;

    [Reactive]
    public ReactiveCommand<Unit, Unit> GoPopup { get; private set; }

    [Reactive]
    public ReactiveCommand<Unit, Unit> GoModal { get; private set; }

    [Reactive]
    public ReactiveCommand<Unit, Unit> GoNext { get; private set; }

    [Reactive]
    public byte[] ColorArray { get; private set; }

    [Reactive]
    public IEnumerable<TestItem> TestItems { get; private set; }

    [Reactive]
    public TestItem SelectedTestItem { get; set; }

    public SampleViewModel(TestService testService)
    {
        _testService = testService;
    }

    protected override void Initialize()
    {
        var rng = new Random(Guid.NewGuid().GetHashCode());

        var colors = new byte[4];
        rng.NextBytes(colors);
        ColorArray = colors;

        var items = new List<TestItem>();

        for (int i = 0; i < rng.Next(10, 100); i++)
        {
            items.Add(
                new TestItem
                {
                    Value1 = $"Value {i}",
                    Value2 = i,
                });
        }

        SelectedTestItem = items.ElementAt(rng.Next(0, items.Count - 1));

        TestItems = items;
    }

    protected override void RegisterObservables()
    {
        GoPopup =
            ReactiveCommand
                .Create(DefaultAction)
                .DisposeWith(ViewModelBindings);

        GoModal =
            ReactiveCommand
                .Create(DefaultAction)
                .DisposeWith(ViewModelBindings);

        GoNext =
            ReactiveCommand
                .Create(DefaultAction)
                .DisposeWith(ViewModelBindings);
    }
}

public class TestItem : INotifyPropertyChanged
{
#pragma warning disable CS0067
    public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

    public string Value1 { get; set; }

    public int Value2 { get; set; }
}