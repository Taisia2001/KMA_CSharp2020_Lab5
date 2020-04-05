

using EO.Internal;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KMA.ProgrammingInCSharp2020.Lab5.Models
{
    class ProcessModel
    {
        #region Fields
        private Process _currentProcess;
        private int _id;
        private string _name;
        private string _path;
        private string _startTime;
        private string _user;

        private bool _isActive;
        private double _cpu;
        private double _ram;
        private double _ramPercent;
        private int _threads;

        #endregion
      

        #region Properties
        public Process CurrentProcess
        {
            get
            {
                return _currentProcess;
            }
            private set
            {
                _currentProcess = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
            }
            
        }
        public int Id
        {
            get
            {
                return _id;
            }
            private set
            {
                _id = value;
            }

        }
        public bool IsActive
        {
            get
            {
               return  _isActive;
            }
            private set
            {
                _isActive = value;
            }
        }
        public string StartTime
        {
            get
            {
                return _startTime;
            }
            private set
            {
                _startTime = value;
            }
        }
        public int Threads
        {
            get
            {
                return _threads;
            }
            private set
            {
                _threads = value;
            }

        }
        public string Path
        {
            get
            {
                return _path;
            }
            private set
            {
                _path = value;
            }
        }
        public string User
        {
            get
            {
                return _user;
            }
            private set
            {
                _user = value;
            }
        }
        public double Ram
        {
            get
            {
                return _ram;
            }
            private set
            {
                _ram = value;
            }
        }
        public double Cpu
        {
            get
            {
                return _cpu;
            }
            private set
            {
                _cpu = value;
            }
        }
        public double RamPercent
        {
            get
            {
                return _ramPercent;
            }
            private set
            {
                _ramPercent = value;
            }
        }


        #endregion


        internal ProcessModel(Process process)
        {
            CurrentProcess = process;
            Name = CurrentProcess .ProcessName;
            Id = CurrentProcess .Id;
            IsActive = CurrentProcess.Responding;
            try
            {
                StartTime = CurrentProcess.StartTime.ToString();
            }
            catch
            {
                StartTime = "N/A";
            }
            Threads = CurrentProcess.Threads.Count;
            try
            {
                Path = CurrentProcess.MainModule.FileName;
            }
            catch
            {
                Path = "N/A";
            }
            User = Environment.UserName;
            Ram = CurrentProcess.WorkingSet64 / 1024f / 1024f;
            Task.Run(() => CpuUsageForProcess()).Wait();

        }
        private async void CpuUsageForProcess()
        {
            try
            {
               DateTime startTime = DateTime.UtcNow;
              TimeSpan startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
                await Task.Delay(500);

                DateTime endTime = DateTime.UtcNow;
                TimeSpan endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
                double cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                double totalMsPassed = (endTime - startTime).TotalMilliseconds;
                double cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
                Cpu = cpuUsageTotal * 100;
            }
            catch
            {
            }
        }
        internal async void Refresh()
        {
             try
             {
               //  Ram /= 1024;
             }
             catch
             {
             }

             try
             {

               CpuUsageForProcess();
             }
             catch { }
            Ram = CurrentProcess.WorkingSet64 / 1024f / 1024f;
            IsActive = CurrentProcess.Responding;
            Threads = CurrentProcess.Threads.Count;

        }

    }
}
