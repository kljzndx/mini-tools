using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using ShutdownTimer.Services;
using ShutdownTimer.ViewModels;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ShutdownTimer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var sc = new ServiceCollection();
            sc.AddSingleton<ShutdownTimerService>();
            sc.AddSingleton<MainViewModel>();
            sc.AddSingleton<TaskIconViewModel>();
            Ioc.Default.ConfigureServices(sc.BuildServiceProvider());

            FindResource("TaskIcon");
        }
    }
}
