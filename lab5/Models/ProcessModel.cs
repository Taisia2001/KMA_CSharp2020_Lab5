
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
        private double _deviceRam;
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
            Name = CurrentProcess.ProcessName;
            try { 
            _ramCounter = new PerformanceCounter("Process", "Working Set - Private", _name);
            }
            catch { }
            try
            {
                _cpuCounter = new PerformanceCounter("Process", "% Processor Time", _name, true);
                _cpuCounter.NextValue();
            }
            catch { }

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
            try
            {
                Cpu = _cpuCounter.NextValue() / Environment.ProcessorCount;
            }
            catch {}
            try
            {
                var memoryValues = new ManagementObjectSearcher("select * from Win32_OperatingSystem").Get().Cast<ManagementObject>().Select(mo => new
                {
                    TotalVisibleMemorySize = Double.Parse(mo["TotalVisibleMemorySize"].ToString())
                }).FirstOrDefault();
                Ram = (float)Math.Round(((double)(_ramCounter.RawValue) / 1024 / 1024), 1);
               
               if (memoryValues != null)
                {
                    _deviceRam = memoryValues.TotalVisibleMemorySize;
                    RamPercent = (Ram / _deviceRam) * 100;
                }
            }
            catch { }

        }
        
        internal async void Refresh()
        {
             try
             {
                Ram = (float)Math.Round(((double)(_ramCounter.RawValue) / 1024 / 1024), 1);

                if (_deviceRam != 0)
                {
                    RamPercent = (Ram / _deviceRam) * 100;
                }
            }
            catch
             {
             }

             try
             {

                Cpu = _cpuCounter.NextValue() / Environment.ProcessorCount;
            }
             catch { }
            IsActive = CurrentProcess.Responding;
            Threads = CurrentProcess.Threads.Count;

        }

    }
}
