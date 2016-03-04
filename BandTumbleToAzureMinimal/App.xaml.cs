using System;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using BandTumbleToAzureMinimal.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Template10.Mvvm;
using Template10.Common;
using BandTumbleToAzureMinimal.Services.BandServices;

namespace BandTumbleToAzureMinimal
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region App settings

            var _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;
            CacheMaxDuration = _settings.CacheMaxDuration;
            ShowShellBackButton = _settings.UseShellBackButton;

            #endregion

        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await Task.CompletedTask;
            // Put Cortana initialization here
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // long-running startup tasks go here

            NavigationService.Navigate(typeof(Views.MainPage));
            await Task.CompletedTask;

            #region Band stuff

            var _bandService = BandService.Instance;
            await _bandService.InitBand();

            var _hwVersion = _bandService.HardwareVersion;
            var _fwVersion = _bandService.FirmwareVersion;

            #endregion


        }
    }
}

