using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Band;
using Microsoft.Band.Sensors;

namespace BandTumbleToAzureMinimal.Services.BandServices
{
    class BandService
    {

        private IBandInfo[] PairedBands;
        private IBandClient BandClient;

        private String _firmwareVersion;
        private String _hardwareVersion;

        public static BandService Instance { get; }
        static BandService()
        {
            // implement singleton pattern
            Instance = Instance ?? new BandService();
        }

        public async Task InitBand()
        {
            var _bandService = BandService.Instance;
            _bandService.PairedBands = await BandClientManager.Instance.GetBandsAsync();
            if (PairedBands.Length != 0)
            {
                try
                {
                    using (BandClient = await BandClientManager.Instance.ConnectAsync(PairedBands[0])) // init with first Band found
                    {
                        // set up stuff after successful connection
                        _firmwareVersion = await BandClient.GetFirmwareVersionAsync();
                        _hardwareVersion = await BandClient.GetHardwareVersionAsync();
                    }
                }
                catch (BandException ex)
                {
                    // handle Band exceptions
                }
            }
            else
            {
                // do something to indicate no Band was found.
            }
        }

        public String HardwareVersion
        {
            get
            {
                return _hardwareVersion;
            }
        }

        public String FirmwareVersion
        {
            get
            {
                return _firmwareVersion;
            }
        }
    }
}

//using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
//                {
//                    int samplesReceived = 0; // the number of Accelerometer samples received

//List<BandSensorReadingEventArgs<IBandAccelerometerReading>> _args = new List<BandSensorReadingEventArgs<IBandAccelerometerReading>>();

//// Subscribe to Accelerometer data.
//bandClient.SensorManager.Accelerometer.ReadingChanged += (s, args) => {
//                        //_args[samplesReceived] = args;
//                        _args.Add(args);
//                        samplesReceived++;
//                    };
//                    await bandClient.SensorManager.Accelerometer.StartReadingsAsync();

//// Receive Accelerometer data for a while, then stop the subscription.
//await Task.Delay(TimeSpan.FromSeconds(20));
//                    await bandClient.SensorManager.Accelerometer.StopReadingsAsync();

//                    this.viewModel.StatusMessage = string.Format("Done. {0} Accelerometer samples were received.", samplesReceived);
//                }
