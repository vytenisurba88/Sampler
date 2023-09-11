using Sampler.Models;

namespace Sampler.DataProviders
{
    public class DummyDataProvider : IDataProvider
    {
        public IEnumerable<Measurement> GetData() => new List<Measurement>
            {
                new Measurement { MeasurementValue = 35.79M, Time = DateTime.Parse("2017-01-03T10:04:45"), Type = MeasurementType.Temp },
                new Measurement { MeasurementValue = 98.78M, Time = DateTime.Parse("2017-01-03T10:01:18"), Type = MeasurementType.SpO2 },
                new Measurement { MeasurementValue = 35.01M, Time = DateTime.Parse("2017-01-03T10:09:07"), Type = MeasurementType.Temp },
                new Measurement { MeasurementValue = 96.49M, Time = DateTime.Parse("2017-01-03T10:03:34"), Type = MeasurementType.SpO2 },
                new Measurement { MeasurementValue = 35.82M, Time = DateTime.Parse("2017-01-03T10:02:01"), Type = MeasurementType.Temp },
                new Measurement { MeasurementValue = 97.17M, Time = DateTime.Parse("2017-01-03T10:05:00"), Type = MeasurementType.SpO2 },
                new Measurement { MeasurementValue = 95.08M, Time = DateTime.Parse("2017-01-03T10:05:01"), Type = MeasurementType.SpO2 }
            };
    }
}