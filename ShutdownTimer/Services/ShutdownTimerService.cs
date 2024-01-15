using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ShutdownTimer.Services
{
    public class ShutdownTimerService : ObservableObject
    {
        private int _minutes;
        private bool _isEnable;
        private DispatcherTimer? _timer;
        private string _shutdownTime = "";

        public string ShutdownTime
        {
            get => _shutdownTime;
            private set => SetProperty(ref _shutdownTime, value);
        }

        public int Minutes
        {
            get => _minutes;
            private set => SetProperty(ref _minutes, value);
        }

        public bool IsEnable
        {
            get => _isEnable;
            private set => SetProperty(ref _isEnable, value);
        }

        public void Start(int minutes)
        {
            if (_timer != null)
                return;

            _timer = new (DispatcherPriority.Normal)
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
            
            Minutes = minutes;
            ShutdownTime = (DateTime.Now + TimeSpan.FromMinutes(Minutes)).ToLongTimeString();
            IsEnable = true;
        }

        public void Stop()
        {
            if (_timer == null)
                return;

            if (!_timer.IsEnabled)
                CancelShutdown();

            _timer.Stop();
            _timer.Tick -= Timer_Tick;
            _timer = null;

            Minutes = 0;
            ShutdownTime = "";
            IsEnable = false;
        }

        private void Shutdown()
        {
            Process process = new Process();
            process.StartInfo.FileName = "shutdown";
            process.StartInfo.Arguments = "-s -t 60";
            process.Start();
        }

        private void CancelShutdown()
        {
            Process process = new Process();
            process.StartInfo.FileName = "shutdown";
            process.StartInfo.Arguments = "-a";
            process.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (Minutes == 0)
            { 
                Shutdown();
                _timer?.Stop();
            }
            else
                --Minutes;
        }
    }
}
