using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using KMA.ProgrammingInCSharp2020.Lab5.Models;

namespace KMA.ProgrammingInCSharp2020.Lab5.Tools.Managers
{
    internal static class StationManager
    {
        private static List<ProcessModel> _processes;


        internal static List<ProcessModel> Processes
        {
            get { return _processes; }
        }

        internal static int SortingType { get; set; }
        internal static void Initialize()
        {
            SortingType = -1;
            _processes = new List<ProcessModel>();
            RefreshProcesses();
        }

       internal static void RefreshProcesses()
        {
            var currentIds = Processes.Select(p => p.Id).ToList();

            foreach (var p in Process.GetProcesses())
            {
                if (!currentIds.Remove(p.Id))
                {
                    Processes.Add(new ProcessModel(p));
                }
            }

            foreach (var id in currentIds)
            {
                var process = Processes.First(p => p.Id == id);
                Processes.Remove(process);
            }
            SortProcesses();
        }
        internal static void DeleteProcess(ref ProcessModel p)
        {
            _processes.Remove(p);
        }

		internal static void SortProcesses()
		{
				OrderedParallelQuery<ProcessModel> sortedProsesses;
				switch (SortingType)
				{
					case 0:
						sortedProsesses = from p in _processes.AsParallel()
										  orderby p.Id
										  select p;
						break;
					case 1:
						sortedProsesses = from p in _processes.AsParallel()
										  orderby p.Name.ToLower()
										  select p;
						break;
					case 2:
						sortedProsesses = from p in _processes.AsParallel()
										  orderby p.Cpu
										  select p;
						break;
					case 3:
						sortedProsesses = from p in _processes.AsParallel()
										  orderby p.RamPercent
										  select p;
						break;
					case 4:
						sortedProsesses = from p in _processes.AsParallel()
										  orderby p.Ram
										  select p;
						break;
					case 5:
						sortedProsesses = from p in _processes.AsParallel()
										  orderby p.Threads
										  select p;
						break;
					case 6:
						sortedProsesses = from p in _processes.AsParallel()
										  orderby p.Path.ToLower()
										  select p;
						break;
					case 7:
						sortedProsesses = from p in _processes.AsParallel()
										  orderby p.User.ToLower()
										  select p;
						break;
					case 8:
						sortedProsesses = from p in _processes.AsParallel()
										  orderby p.StartTime
										  select p;
						break;
					case 9:
						sortedProsesses = from p in _processes.AsParallel()
										  orderby p.IsActive
										  select p;
						break;
					default:
						return;

				}
				_processes = sortedProsesses.ToList();
		}



	}
}