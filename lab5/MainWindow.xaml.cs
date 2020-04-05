using KMA.ProgrammingInCSharp2020.Lab5.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
        public static event Action StopThreads;
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            StopThreads?.Invoke();
            Environment.Exit(0);
        }
    }
}
