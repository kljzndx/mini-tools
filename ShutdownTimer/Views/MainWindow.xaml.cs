using System;
using System.Windows;

namespace ShutdownTimer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow? Current;
        private bool needExitApp = true;
        
        public MainWindow()
        {
            InitializeComponent();
            Current = this;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Minimized)
            {
                needExitApp = false;
                Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Current = null;

            if (needExitApp)
                App.Current.Shutdown();
        }
    }
}
