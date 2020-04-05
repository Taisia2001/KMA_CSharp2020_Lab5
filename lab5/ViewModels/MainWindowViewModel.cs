using KMA.ProgrammingInCSharp2020.Lab5.Tools;
using KMA.ProgrammingInCSharp2020.Lab5.Tools.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace KMA.ProgrammingInCSharp2020.Lab5.ViewModels
{
    class MainWindowViewModel : BaseViewModel, ILoaderOwner
    {
        #region Fields
        private Visibility _loaderVisibility = Visibility.Hidden;
        private bool _isControlEnabled = true;
        #endregion

        #region Properties
        public Visibility LoaderVisibility
        {
            get { return _loaderVisibility; }
            set
            {
                _loaderVisibility = value;
                OnPropertyChanged();
            }
        }
        public bool IsControlEnabled
        {
            get { return _isControlEnabled; }
            set
            {
                _isControlEnabled = value;
                OnPropertyChanged();
            }
        }
        #endregion

        internal MainWindowViewModel()
        {
            LoaderManager.Instance.Initialize(this);
        }
    }
}
