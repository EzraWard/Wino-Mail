﻿using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Wino.Core.Messages.Shell;
using Wino.Core.WinUI;
using Wino.Mail.ViewModels;
using Wino.Mail.WinUI;

namespace Wino
{
    public class BasePage : Page, IRecipient<LanguageChanged>
    {
        public UIElement ShellContent
        {
            get { return (UIElement)GetValue(ShellContentProperty); }
            set { SetValue(ShellContentProperty, value); }
        }

        public static readonly DependencyProperty ShellContentProperty = DependencyProperty.Register(nameof(ShellContent), typeof(UIElement), typeof(BasePage), new PropertyMetadata(null));

        public void Receive(LanguageChanged message)
        {
            OnLanguageChanged();
        }

        public virtual void OnLanguageChanged() { }
    }

    public abstract class BasePage<T> : BasePage where T : BaseViewModel
    {
        public T ViewModel { get; } = App.Current.Services.GetService<T>();

        protected BasePage()
        {
            // UWP and WinUI Dispatchers are different.
#if NET8_0
            ViewModel.Dispatcher = new WinAppDispatcher(DispatcherQueue);
#else
            ViewModel.Dispatcher = new UWPDispatcher(Dispatcher);
#endif
        }

        ~BasePage()
        {
            Debug.WriteLine($"Disposed {this.GetType().Name}");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var mode = GetNavigationMode(e.NavigationMode);
            var parameter = e.Parameter;

            WeakReferenceMessenger.Default.UnregisterAll(this);
            WeakReferenceMessenger.Default.RegisterAll(this);

            ViewModel.OnNavigatedTo(mode, parameter);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            var mode = GetNavigationMode(e.NavigationMode);
            var parameter = e.Parameter;

            WeakReferenceMessenger.Default.UnregisterAll(this);

            ViewModel.OnNavigatedFrom(mode, parameter);

            GC.Collect();
        }

        private Core.Domain.Models.Navigation.NavigationMode GetNavigationMode(NavigationMode mode)
        {
            return (Core.Domain.Models.Navigation.NavigationMode)mode;
        }
    }
}
