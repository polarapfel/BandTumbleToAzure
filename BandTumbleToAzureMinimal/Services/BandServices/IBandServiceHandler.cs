using Microsoft.Band.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BandTumbleToAzureMinimal.Services.BandServices
{
    interface IBandServiceHandler
    {
        IBandServiceHandler Next();

        Task Handle(BandSensorReadingEventArgs<IBandHeartRateReading> reading);
    }
}
