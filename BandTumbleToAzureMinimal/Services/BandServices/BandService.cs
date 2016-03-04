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

        private String FirmwareVersion;
        private String HardwareVersion;

        public static BandService Instance { get; }
        static BandService()
        {
            // implement singleton pattern
            Instance = Instance ?? new BandService();
        }

        public async Task InitBand()
        {
            PairedBands = await BandClientManager.Instance.GetBandsAsync();
            try
            {
                using ( BandClient = await BandClientManager.Instance.ConnectAsync(PairedBands[0]))
                {
                    // set up stuff after successful connection
                    FirmwareVersion = await BandClient.GetFirmwareVersionAsync();
                    HardwareVersion = await BandClient.GetHardwareVersionAsync();
                }
            }
            catch (BandException ex)
            {
                // handle Band exceptions
            }
        }



    }
}
