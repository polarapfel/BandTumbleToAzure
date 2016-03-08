using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Band.Sensors;
using Newtonsoft.Json;
using Windows.System.Profile;
using Microsoft.Azure.Devices.Client;

namespace BandTumbleToAzureMinimal.Services.BandServices
{
    class BandToIoTHubHandler : IBandServiceHandler
    {
        private const string deviceConnectionString = "HostName=TumbleHub.azure-devices.net;DeviceId=BandDevice;SharedAccessKey=Tt8Jx/A5OrJZMDG22BJdeDjHqtRSK8439ELWimjJCs4=";
        private HardwareToken ASHWID;

        private IBandServiceHandler _next = null;

        public BandToIoTHubHandler()
        {
            ASHWID = HardwareIdentification.GetPackageSpecificToken(null); // temporary workaround
        }

        public BandToIoTHubHandler(IBandServiceHandler next)
        {
            _next = next;
            ASHWID = HardwareIdentification.GetPackageSpecificToken(null); // temporary workaround
        }

        public async Task Handle(BandSensorReadingEventArgs<IBandHeartRateReading> Reading)
        {
            var type = Reading.SensorReading.GetType();
            await SendDeviceToCloudMessageAsync(Reading);
            if (_next != null) await _next.Handle(Reading);
        }

        public IBandServiceHandler Next()
        {
            return _next;
        }

        private async Task SendDeviceToCloudMessageAsync(BandSensorReadingEventArgs<IBandHeartRateReading> reading)
        {
            var deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Http1);

            var str = JsonConvert.SerializeObject(reading);
            var message = new Message(Encoding.UTF8.GetBytes(str));

            await deviceClient.SendEventAsync(message);
        }
    }
}
