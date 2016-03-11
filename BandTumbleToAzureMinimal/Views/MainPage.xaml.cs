using System;
using BandTumbleToAzureMinimal.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Microsoft.Band;
using System.Threading.Tasks;
using BandTumbleToAzureMinimal.Services.BandServices;
using Microsoft.Band.Sensors;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace BandTumbleToAzureMinimal.Views
{
    public sealed partial class MainPage : Page
    {

        #region instance stuff
        private ConcurrentQueue<BandSensorReadingEventArgs<IBandHeartRateReading>> HeartRateReadingsQueue = new ConcurrentQueue<BandSensorReadingEventArgs<IBandHeartRateReading>>();
        private IBandClient bandClient = null;

        #endregion

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private async void toggleHeartRate_Click(object sender, RoutedEventArgs e)
        {
            IBandInfo[] pairedBands;
            bool state = this.toggleHeartRate.IsOn;

            #region toggledOn
            if (this.toggleHeartRate.IsOn)
            {
                #region InitBandClient
                if (bandClient == null)
                {
                    // Get the list of Microsoft Bands paired to the phone.
                    pairedBands = await BandClientManager.Instance.GetBandsAsync();

                    if (pairedBands.Length < 1)
                    {
                        this.toggleHeartRateFeedback.Text = "No Band paired.";
                        this.toggleHeartRate.IsOn = false;
                        return; // nothing to do
                    }
                    if (pairedBands.Length > 1)
                    {
                        this.toggleHeartRateFeedback.Text = "Trying to connect.";
                        try
                        {
                            foreach (var band in pairedBands)
                            {
                                if (bandClient != null) break;
                                try
                                {
                                    bandClient = await BandClientManager.Instance.ConnectAsync(band);
                                }
                                catch (Exception)
                                {
                                    this.toggleHeartRateFeedback.Text = "Connection to " + band.Name + " failed. Trying next.";
                                }
                            }
                        }
                        catch (Exception)
                        {
                            this.toggleHeartRateFeedback.Text = "Unexpected exception occured.";
                        }
                        if (bandClient != null) this.toggleHeartRateFeedback.Text = "Connected.";
                    }
                }
                #endregion

                #region StartHeartRateSensor
                if (bandClient != null)
                {

                    bool heartRateConsentGranted;

                    // Check whether the user has granted access to the HeartRate sensor.
                    if (bandClient.SensorManager.HeartRate.GetCurrentUserConsent() == UserConsent.Granted)
                    {
                        heartRateConsentGranted = true;
                    }
                    else
                    {
                        heartRateConsentGranted = await bandClient.SensorManager.HeartRate.RequestUserConsentAsync();
                    }

                    if (!heartRateConsentGranted)
                    {
                        this.toggleHeartRateFeedback.Text = "Heart rate sensor access denied.";
                    }
                    else
                    {
                        try
                        {
                            // Connect to Microsoft Band.

                            //int samplesReceived = 0; // the number of Accelerometer samples received
                            // Subscribe to Accelerometer data.
                            bandClient.SensorManager.HeartRate.ReadingChanged += (s, args) =>
                            {
                                this.HeartRateReadingsQueue.Enqueue(args);
                            };

                            try
                            {
                                bool heartRateTask = await bandClient.SensorManager.HeartRate.StartReadingsAsync();
                                this.toggleHeartRateFeedback.Text = "Collecting. Sensor active.";
                            }
                            catch (Exception ex)
                            {
                                this.toggleHeartRateFeedback.Text = "Exception: " + ex.Message;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.toggleHeartRateFeedback.Text = "Exception: " + ex.Message;
                        }
                    }
                }
                #endregion
            }
            #endregion
            #region toggledOff
            else
            {
                if (bandClient != null)
                {
                    try
                    {
                        using (bandClient)
                        {
                            // Receive Accelerometer data for a while, then stop the subscription.
                            await bandClient.SensorManager.HeartRate.StopReadingsAsync();
                            this.toggleHeartRateFeedback.Text = "Stopped. Read " + this.HeartRateReadingsQueue.Count + " so far.";
                            bandClient = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.toggleHeartRateFeedback.Text = "Exception: " + ex.Message;
                    }
                }
                else
                {
                    //this should not happen. Why does the class instance of bandClient die?!
                }
            }
            #endregion
        }

        // fires when second layer command bar options are opened from header
        private void pageHeader_Opened(object sender, object e)
        {

        }
    }
}
