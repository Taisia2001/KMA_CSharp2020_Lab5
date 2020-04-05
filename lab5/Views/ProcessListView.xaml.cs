using KMA.ProgrammingInCSharp2020.Lab5.Tools.Managers;
using KMA.ProgrammingInCSharp2020.Lab5.ViewModels;
using System.Windows.Controls;

namespace KMA.ProgrammingInCSharp2020.Lab5.Views
{
    /// <summary>
    /// Логика взаимодействия для ProcessListView.xaml
    /// </summary>
    public partial class ProcessListView : UserControl
    {
        public ProcessListView()
        {
            InitializeComponent();
            StationManager.Initialize();
            DataContext = new ProcessListViewModel();
        }
    }
}
