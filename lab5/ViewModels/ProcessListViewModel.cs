using KMA.ProgrammingInCSharp2020.Lab5.Models;
using KMA.ProgrammingInCSharp2020.Lab5.Tools;
using KMA.ProgrammingInCSharp2020.Lab5.Tools.Managers;
using KMA.ProgrammingInCSharp2020.Lab5.Tools.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KMA.ProgrammingInCSharp2020.Lab5.ViewModels
{
	internal class ProcessListViewModel : BaseViewModel
	{
		#region Fields
		private ProcessModel _chosenProcess;
		private ObservableCollection<ProcessModel> _processes;
		private ProcessModuleCollection _modules;
		private ProcessThreadCollection _threads;
		#endregion
		#region Commands
		private RelayCommand<object> _openCommand;
		private RelayCommand<object> _stopCommand;
		private RelayCommand<object> _sortById;
		private RelayCommand<object> _sortByName;
		private RelayCommand<object> _sortByCpu;
		private RelayCommand<object> _sortByRam;
		private RelayCommand<object> _sortByRamPercent;
		private RelayCommand<object> _sortByThreads;
		private RelayCommand<object> _sortByPath;
		private RelayCommand<object> _sortByUser;
		private RelayCommand<object> _sortByStartTime;
		private RelayCommand<object> _sortByIsActive;
		#endregion

		#region Properties
		public ObservableCollection<ProcessModel> Processes
		{
			get
			{
				return _processes;
			}
			set
			{
				_processes = value;
				OnPropertyChanged();
			}
		}
		public ProcessModel ChosenProcess
		{
			get
			{
				return _chosenProcess;
			}
			set
			{
				_chosenProcess = value;
				try
				{
					Modules = ChosenProcess.CurrentProcess.Modules;
				}
				catch
				{
					Modules = null;
				}
				try
				{
					Threads = ChosenProcess.CurrentProcess.Threads;

				}
				catch
				{
					Threads = null;
				}
				OnPropertyChanged();
			}
		}

		public ProcessThreadCollection Threads
		{
			get
			{
				return _threads;
			}
			private set
			{
				_threads = value;
				OnPropertyChanged();
			}
		}
		public ProcessModuleCollection Modules
		{
			get
			{
				return _modules;
			}
			private set
			{
				_modules = value;
				OnPropertyChanged();
			}
		}

		public RelayCommand<object> OpenCommand
		{
			get
			{
				return _openCommand ?? (_openCommand = new RelayCommand<object>(
						   OpenCommandImplementation, o => CanExecuteCommand()));
			}
		}

		private void OpenCommandImplementation(object obj)
		{
			LoaderManager.Instance.ShowLoader();
			try
			{
				Process.Start("explorer.exe",
					_chosenProcess.Path.Substring(0,
						_chosenProcess.Path.LastIndexOf("\\", StringComparison.Ordinal)));
			}
			catch
			{
				MessageBox.Show("Path is unavailable ", "Opening failed");
			}
			LoaderManager.Instance.HideLoader();
		}

		public RelayCommand<object> StopCommand
		{
			get
			{
				return _stopCommand ?? (_stopCommand = new RelayCommand<object>(
						  StopCommandImplementation, o => CanExecuteCommand()));
			}
		}

		private async void StopCommandImplementation(object obj)
		{
			LoaderManager.Instance.ShowLoader();
			await Task.Run(() =>
			{
				try
				{
					if (MessageBox.Show("Are you sure you want to stop " + ChosenProcess.CurrentProcess.ProcessName, "Stop?",
							MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
					{
						
						Processes.Remove(ChosenProcess);
						ChosenProcess.CurrentProcess.Kill();
						ChosenProcess = null;

					}
				}
				catch (Exception e)
				{
					MessageBox.Show(e.ToString(), "Fail");
					//MessageBox.Show("Could not close this process", "Fail");
				}

			});
			LoaderManager.Instance.HideLoader();
		}

		public RelayCommand<object> SortById
		{
			get
			{
				return _sortById ?? (_sortById = new RelayCommand<object>(o =>
							   SortImplementation(o, 0)));
			}
		}
		public RelayCommand<object> SortByName
		{
			get
			{
				return _sortByName ?? (_sortByName = new RelayCommand<object>(o =>
						   SortImplementation(o, 1)));
			}
		}
		
		public RelayCommand<object> SortByCpu
		{
			get
			{
				return _sortByCpu ?? (_sortByCpu= new RelayCommand<object>(o =>
						   SortImplementation(o, 2)));
			}
		}
		public RelayCommand<object> SortByRamPercent
		{
			get
			{
				return _sortByRamPercent ?? (_sortByRamPercent = new RelayCommand<object>(o =>
						   SortImplementation(o, 3)));
			}
		}
		public RelayCommand<object> SortByRam
		{
			get
			{
				return _sortByRam ?? (_sortByRam = new RelayCommand<object>(o =>
						   SortImplementation(o, 4)));
			}
		}
		public RelayCommand<object> SortByThreads
		{
			get
			{
				return _sortByThreads ?? (_sortByThreads = new RelayCommand<object>(o =>
						   SortImplementation(o, 5)));
			}
		}
		public RelayCommand<object> SortByPath
		{
			get
			{
				return _sortByPath ?? (_sortByPath = new RelayCommand<object>(o =>
						   SortImplementation(o, 6)));
			}
		}
		public RelayCommand<object> SortByUser
		{
			get
			{
				return _sortByUser ?? (_sortByUser = new RelayCommand<object>(o =>
						   SortImplementation(o, 7)));
			}
		}
		
		public RelayCommand<object> SortByStartTime
		{
			get
			{
				return _sortByStartTime ?? (_sortByStartTime = new RelayCommand<object>(o =>
						   SortImplementation(o, 8)));
			}
		}
		public RelayCommand<object> SortByIsActive
		{
			get
			{
				return _sortByIsActive ?? (_sortByIsActive = new RelayCommand<object>(o =>
						   SortImplementation(o, 9)));
			}
		}

		private async void SortImplementation(object o, int i)
		{
			LoaderManager.Instance.ShowLoader();
			await Task.Run(() =>
			{
				IOrderedEnumerable<ProcessModel> sortedProsesses;
				switch (i)
				{
					case 0:
						sortedProsesses = from p in _processes
										  orderby p.Id
										select p;
						break;
					case 1:
						sortedProsesses = from p in _processes
										orderby p.Name.ToLower()
										select p;
						break;
					case 2:
						sortedProsesses = from p in _processes
										orderby p.Cpu
										select p;
						break;
					case 3:
						sortedProsesses = from p in _processes
										  orderby p.RamPercent
										select p;
						break;
					case 4:
						sortedProsesses = from p in _processes
										  orderby p.Ram
										select p;
						break;
					case 5:
						sortedProsesses = from p in _processes
										  orderby p.Threads
										select p;
						break;
					case 6:
						sortedProsesses = from p in _processes
										  orderby p.Path.ToLower()
										select p;
						break;
					case 7:
						sortedProsesses = from p in _processes
										  orderby p.User.ToLower()
										  select p;
						break;
					case 8:
						sortedProsesses = from p in _processes
										  orderby p.StartTime
										  select p;
						break;
					default:
						sortedProsesses = from p in _processes
										orderby p.IsActive
										select p;
						break;
				}
				Processes = new ObservableCollection<ProcessModel>(sortedProsesses);

			});
			LoaderManager.Instance.HideLoader();
		}

		#endregion

		public ProcessListViewModel()
		{
			Processes = new ObservableCollection<ProcessModel>();
			Process[] processes = System.Diagnostics.Process.GetProcesses();
			foreach (Process p in processes)
			{
				Processes.Add(new ProcessModel(p));
			}
			//new Thread(RefreshProcesses).Start();
			new Thread(RefreshProcessMeta).Start();
			try
			{
				Modules = ChosenProcess.CurrentProcess.Modules;
			}
			catch
			{
				Modules = null;
			}
			try
			{
				Threads = ChosenProcess.CurrentProcess.Threads;

			}
			catch
			{
				Threads = null;
			}
		}

		private void RefreshProcessMeta(object obj)
		{
			while (true)
			{
				foreach (ProcessModel process in Processes)
					process.Refresh();
				Processes = new ObservableCollection<ProcessModel>(Processes);
				Thread.Sleep(2000);
			}
		}

		private void RefreshProcesses(object obj)
		{
			while (true)
			{
				ObservableCollection<ProcessModel> temp = new ObservableCollection<ProcessModel>(Processes);
				foreach (Process p in Process.GetProcesses())
				{
					ProcessModel tempProcess = new ProcessModel(p);
					temp.Add(tempProcess);
					//if (ChosenProcess != null && tempProcess.Id == ChosenProcess.Id) ;
						//a = temp.IndexOf(tempProcess);
					
				}
				ProcessModel t = ChosenProcess;
				Processes = temp;
				//ChosenProcess = temp.;
				Thread.Sleep(2000);
			}
		}


		private bool CanExecuteCommand()
		{
			return ChosenProcess != null;
		}
	}
}

