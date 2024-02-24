using CMWeather.Models;
using CMWeather.Helpers;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMWeather.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        #region Properties        
        public event PropertyChangedEventHandler PropertyChanged;

        private WeatherResponse _weatherResponse;
        public WeatherResponse WeatherResponse
        {
            get { return _weatherResponse; }
            set
            {
                _weatherResponse = value;
                OnPropertyChanged(nameof(WeatherResponse));
            }
        }
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
            }
        }
        #endregion

        #region Commands
        public IAsyncCommand UpdateCommand { get; private set; }
        #endregion
        public MainViewModel()
        {
            WeatherResponse = new WeatherResponse();
            UpdateCommand = new AsyncCommand(ExecuteUpdateAsync, CanExecuteUpdate);
            MainWindow_Loaded();
            SetNotifyIcon();
        }
        #region VM Methods
        void MainWindow_Loaded()
        {
            RegeditManager();
        }

        void SetNotifyIcon()
        {
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Information;
            notifyIcon.Visible = true;
            notifyIcon.Click += Icon_Click;
        }

        private void RegeditManager()
        {
            string rutaEnsamblado = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // SET Clave del registro | Run
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rk.GetValue("CWeather") == null)
                rk.SetValue("CWeather", rutaEnsamblado);
        }
        #endregion
        #region Commands events
        private async Task ExecuteUpdateAsync()
        {
            try
            {
                IsBusy = true;
                APIHelper apiResponse = new APIHelper();
                WeatherResponse = await apiResponse.Call_API();
            }
            finally
            {
                IsBusy = false;
            }
        }
        private bool CanExecuteUpdate()
        {
            return !IsBusy;
        }
        private async void Icon_Click(object sender, EventArgs e)
        {
            await ExecuteUpdateAsync();
        }
        #endregion
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

