using Microsoft.Band.Sensors;
using System.Threading.Tasks;

namespace BandTumbleToAzureMinimal.Services.BandServices
{
    interface IBandAccelerometerServiceHandler
    {
        Task Handle(BandSensorReadingEventArgs<IBandAccelerometerReading> reading);
    }
}