using CommunityToolkit.Mvvm.ComponentModel;
using HappyStudio.Mvvm.Input.Wpf;

using ShutdownTimer.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShutdownTimer.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private int _minutes = 5;

        public MainViewModel(ShutdownTimerService timerService)
        {
            TimerService = timerService;
            StartTimerCommand = new (() => TimerService.Start(Minutes));
            StopTimerCommand = new (() => TimerService.Stop()); 
        }

        public int Minutes
        {
            get => _minutes;
            set => SetProperty(ref _minutes, value);
        }

        public ShutdownTimerService TimerService { get; }

        public RelayCommand StartTimerCommand { get; }
        public RelayCommand StopTimerCommand { get; }
    }
}
