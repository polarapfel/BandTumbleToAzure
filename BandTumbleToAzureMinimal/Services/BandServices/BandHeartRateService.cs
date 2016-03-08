using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BandTumbleToAzureMinimal.Services.BandServices
{
    class BandHeartRateService
    {

        private BandService _bandService = BandService.Instance;
        private IBandClient _bandClient;
        private IBandInfo _pairedBand;
        private int samplesReceived = 0; // the number of Accelerometer samples received
        private Boolean isRunning = false;

        private ConcurrentQueue<BandSensorReadingEventArgs<IBandHeartRateReading>> _readingsQueue = new ConcurrentQueue<BandSensorReadingEventArgs<IBandHeartRateReading>>();

        public static BandHeartRateService Instance { get; }

        public ConcurrentQueue<BandSensorReadingEventArgs<IBandHeartRateReading>> ReadingsQueue
        {
            get
            {
                return _readingsQueue;
            }
        }

        static BandHeartRateService()
        {
            // implement singleton pattern
            Instance = Instance ?? new BandHeartRateService();
        }

        public async Task Start(Boolean reset)
        {
            _bandClient = _bandService.BandClient;
            _pairedBand = _bandService.PairedBand;

            if (_bandClient.SensorManager.HeartRate.GetCurrentUserConsent() != UserConsent.Granted)
            {
                await _bandClient.SensorManager.HeartRate.RequestUserConsentAsync();
            }

            using (_bandClient = await BandClientManager.Instance.ConnectAsync(_pairedBand))
            {
                // Subscribe to Heart Rate data.
                _bandClient.SensorManager.HeartRate.ReadingChanged += (s, args) =>
                {
                    _readingsQueue.Enqueue(args);
                    samplesReceived++;
                };
                await _bandClient.SensorManager.HeartRate.StartReadingsAsync();
            }
        }

        public async Task Stop(Boolean force)
        {
            _bandClient = _bandService.BandClient;
            _pairedBand = _bandService.PairedBand;
            await _bandClient.SensorManager.HeartRate.StopReadingsAsync();
        }
    }
}
