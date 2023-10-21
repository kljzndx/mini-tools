using CommunityToolkit.Mvvm.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShutdownTimer.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel Main => Ioc.Default.GetRequiredService<MainViewModel>();
        public TaskIconViewModel TaskIcon => Ioc.Default.GetRequiredService<TaskIconViewModel>();
    }
}
