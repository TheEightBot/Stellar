﻿using Stellar.ViewModel;

namespace Stellar.UnitTests;

public class ViewModelBaseTests
{
    [Fact]
    public void SetupViewModel_InitializesOnce()
    {
        // Arrange
        var viewModel = new TestViewModel();

        // Act
        viewModel.SetupViewModel();

        // Assert
        Assert.True(viewModel.Initialized);
    }

    [Fact]
    public void RegisterBindings_RegistersOnce()
    {
        // Arrange
        var viewModel = new TestViewModel();

        // Act
        viewModel.RegisterBindings();

        // Assert
        Assert.True(viewModel.BindingsRegistered);
    }

    [Fact]
    public void UnregisterBindings_ClearsBindings()
    {
        // Arrange
        var viewModel = new TestViewModel();
        viewModel.RegisterBindings();

        // Act
        viewModel.UnregisterBindings();

        // Assert
        Assert.False(viewModel.BindingsRegistered);
    }

    [Fact]
    public void Dispose_DisposesBindings()
    {
        // Arrange
        var viewModel = new TestViewModel();
        viewModel.RegisterBindings();

        // Act
        viewModel.Dispose();

        // Assert
        Assert.True(viewModel.IsDisposed);
    }

    private class TestViewModel : ViewModelBase
    {
        public bool Initialized { get; private set; }

        protected override void Initialize()
        {
            Initialized = true;
        }

        protected override void RegisterObservables()
        {
            // Do nothing
        }
    }
}