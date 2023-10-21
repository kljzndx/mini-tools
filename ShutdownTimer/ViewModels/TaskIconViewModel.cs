using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using HappyStudio.Mvvm.Input.Wpf;
using ShutdownTimer.Services;
using ShutdownTimer.Views;

namespace ShutdownTimer.ViewModels
{
    public class TaskIconViewModel : ObservableObject
    {
        private readonly ShutdownTimerService _service;
        private string _toolTip;


        public TaskIconViewModel(ShutdownTimerService service)
        {
            _service = service;
            _toolTip = SwitchToolTip();
            _service.PropertyChanged += Service_OnPropertyChanged;

            StopTimerCommand = new RelayCommand(() => _service.Stop(), () => _service.IsEnable);
            ShowMainWindowCommand = new RelayCommand(() =>
            {
                Application.Current.MainWindow = new MainWindow();
                Application.Current.MainWindow.Show();
            }, () => MainWindow.Current == null);
            ExitCommand = new RelayCommand(Application.Current.Shutdown);
        }

        public string ToolTip
        {
            get => _toolTip;
            set => SetProperty(ref _toolTip, value);
        }

        public RelayCommand StopTimerCommand { get; }
        public RelayCommand ShowMainWindowCommand { get; }
        public RelayCommand ExitCommand { get; }

        public string SwitchToolTip()
        {
            if (_service.IsEnable)
                return $"系统将在 {_service.ShutdownTime} 关机";
            else
                return "定时器未开启";
        }

        private void Service_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_service.IsEnable))
                ToolTip = SwitchToolTip();
        }
    }
}
