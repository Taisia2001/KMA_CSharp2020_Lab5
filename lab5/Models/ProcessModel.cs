using System;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace KMA.ProgrammingInCSharp2020.Lab5.Models
{
    class ProcessModel
    {
        #region Fields
        private PerformanceCounter _ramCounter;
        private PerformanceCounter _cpuCounter;
        private static double _deviceRam = new ManagementObjectSearcher("select * from Win32_OperatingSystem").Get().Cast<ManagementObject>().Select(mo => new
        {
            TotalVisibleMemorySize = Double.Parse(mo["TotalVisibleMemorySize"].ToString())
        }).FirstOrDefault().TotalVisibleMemorySize;
        private Process _currentProcess;

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
                return _currentProcess.ProcessName;
            }
            
        }
        public int Id
        {
            get
            {
                return _currentProcess.Id;
            }

        }
        public bool IsActive
        {
            get
            {
               return  _currentProcess.Responding;
            }
        }
        public string StartTime
        {
            get
            {
                try
                {
                   return _currentProcess.StartTime.ToString();
                }
                catch
                {
                    return "N/A";
                }
            }
        }
        public int Threads
        {
            get
            {
                return _currentProcess.Threads.Count;
            }

        }
        public string Path
        {
            get
            {
                try
                {
                    return _currentProcess.MainModule.FileName;
                }
                catch
                {
                   return "N/A";
                }
            }
        }
        public string User
        {
            get
            {
                return Environment.UserName;
            }
        }
        public double Ram
        {
            get
            {
                try
                {
                    return Math.Round(((double)(_ramCounter.RawValue) / 1024 / 1024), 1);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public double Cpu
        {
            get
            {
                try
                {
                    return Math.Round(_cpuCounter.NextValue() / Environment.ProcessorCount, 1);
                }
                catch { return 0; }
            }
        }
        public double RamPercent
        {
            get
            {
                return Math.Round(Ram*100 / _deviceRam * 100,1);
            }
        }


        #endregion


        internal ProcessModel(Process process)
        {
            CurrentProcess = process;
            try { 
            _ramCounter = new PerformanceCounter("Process", "Working Set - Private", Name);
            }
            catch { }
            try
            {
                _cpuCounter = new PerformanceCounter("Process", "% Processor Time", Name, true);
                _cpuCounter.NextValue();
            }
            catch { }

        }
        

    }
}
