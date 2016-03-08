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

        private IBandInfo[] _pairedBands;
        private IBandClient _bandClient;

        private IBandInfo _pairedBand;

        private String _firmwareVersion;
        private String _hardwareVersion;

        private bool _isInit = false;
        
        public static BandService Instance { get; }
        static BandService()
        {
            // implement singleton pattern
            Instance = Instance ?? new BandService();
        }

        public async Task InitBand(Boolean force)
        {
            var _bandService = BandService.Instance;
            if (!_isInit || force)
            {
                _bandService._pairedBands = await BandClientManager.Instance.GetBandsAsync();
                if (_pairedBands.Length != 0)
                {
                    _pairedBand = _pairedBands[0];
                    try
                    {
                        using (_bandClient = await BandClientManager.Instance.ConnectAsync(_pairedBands[0])) // init with first Band found
                        {
                            // set up stuff after successful connection
                            _firmwareVersion = await _bandClient.GetFirmwareVersionAsync();
                            _hardwareVersion = await _bandClient.GetHardwareVersionAsync();
                            
                        }
                        _isInit = true;
                    }
                    catch (BandException ex)
                    {
                        // handle Band exceptions
                    }
                }
                else
                {
                    // do something to indicate no Band was found.
                    _isInit = false;
                }
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

        public IBandClient BandClient
        {
            get
            {
                if (_isInit) return _bandClient;
                else return null;
            }
        }

        public IBandInfo PairedBand
        {
            get
            {
                if (_isInit) return _pairedBand;
                else return null;
            }
        }
    }
}