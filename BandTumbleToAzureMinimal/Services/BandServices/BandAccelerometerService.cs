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
    class BandAccelerometerService
    {

        private BandService _bandService = BandService.Instance;
        private IBandClient _bandClient;
        private IBandInfo _pairedBand;
        private int samplesReceived = 0; // the number of Accelerometer samples received
        private Boolean isRunning = false;

        private ConcurrentQueue<BandSensorReadingEventArgs<IBandAccelerometerReading>> _readingsQueue = new ConcurrentQueue<BandSensorReadingEventArgs<IBandAccelerometerReading>>();

        public static BandAccelerometerService Instance { get; }

        public ConcurrentQueue<BandSensorReadingEventArgs<IBandAccelerometerReading>> ReadingsQueue
        {
            get
            {
                return _readingsQueue;
            }
        }

        static BandAccelerometerService()
        {
            // implement singleton pattern
            Instance = Instance ?? new BandAccelerometerService();
        }

        public async Task Start(Boolean reset)
        {
            _bandClient = _bandService.BandClient;
            _pairedBand = _bandService.PairedBand;
            using (_bandClient = await BandClientManager.Instance.ConnectAsync(_pairedBand))
            {
                // Subscribe to Accelerometer data.
                _bandClient.SensorManager.Accelerometer.ReadingChanged += (s, args) =>
                {
                    _readingsQueue.Enqueue(args);
                    samplesReceived++;
                };
                await _bandClient.SensorManager.Accelerometer.StartReadingsAsync();
            }
        }

        public async Task Stop(Boolean force)
        {
            _bandClient = _bandService.BandClient;
            _pairedBand = _bandService.PairedBand;
            await _bandClient.SensorManager.Accelerometer.StopReadingsAsync();
        }
    }
}
