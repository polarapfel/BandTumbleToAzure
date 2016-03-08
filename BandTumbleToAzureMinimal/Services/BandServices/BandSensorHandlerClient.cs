using Microsoft.Band.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BandTumbleToAzureMinimal.Services.BandServices
{
    class BandSensorHandlerClient
    {
        public static async Task Process()
        {
            // Connect to Band (if not done already)
            BandService bsInstance = BandService.Instance;
            await bsInstance.InitBand(false);

            // Start collecting Accelerometer readings
            BandAccelerometerService basInstance = BandAccelerometerService.Instance;
            await basInstance.Start(false);

            //// Start collecting heart rate readings
            //BandHeartRateService hrsInstance = BandHeartRateService.Instance;
            //await hrsInstance.Start(false);

            //IBandServiceHandler handler = new BandToIoTHubHandler();

            //try
            //{
            //    int failed = 0;
            //    while (true)
            //    {
            //        BandSensorReadingEventArgs<IBandHeartRateReading> value;
            //        var hasValue = hrsInstance.ReadingsQueue.TryDequeue(out value);
            //        if (!hasValue)
            //        {
            //            failed++;
            //        }
            //        else
            //        {
            //            await handler.Handle(value);
            //        }
            //        if (failed == 500) break;
            //    }
                
            //}
            //catch (Exception e)
            //{
            //    throw(e);
            //}
        }
    }
}
