using Microsoft.Band.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BandTumbleToAzureMinimal.Models.BandSensor
{
    class SensorReading
    {
        private string _uniqueId;
        private BandSensorReadingEventArgs<IBandSensorReading> _reading;

        public SensorReading(String UniqueIdentifier, BandSensorReadingEventArgs<IBandSensorReading> SensorReading)
        {
            _uniqueId = UniqueIdentifier;
            _reading = SensorReading;
        }

        public string UniqueIdentifier
        {
            get
            {
                return _uniqueId;
            }
        }

        public string ToJson()
        {
            
            return JsonConvert.SerializeObject(this);
        }
    }
}
