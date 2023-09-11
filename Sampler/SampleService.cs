using Sampler.DataProviders;
using Sampler.Extensions;
using Sampler.Models;

namespace Sampler
{
    public interface ISampleService
    {
        IEnumerable<Measurement> SampleData(DateTime start, DateTime? end = null);
    }

    public class SampleService : ISampleService
    {
        private readonly IIntervalProvider _intervalProvider;
        private readonly ISampleProvider _sampleProvider;
        private readonly TimeSpan _timeSpan;
        private readonly IDataProvider _dataProvider;

        public SampleService(IIntervalProvider intervalProvider, 
            ISampleProvider sampleProvider, 
            SamplerConfiguration configuration,
            IDataProvider dataProvider)
        {
            _intervalProvider = intervalProvider;
            _sampleProvider = sampleProvider;
            _dataProvider = dataProvider;
            _timeSpan = configuration.SampleInterval;
        }

        public IEnumerable<Measurement> SampleData(DateTime start, DateTime? end = null)
        {
            var measurements = _dataProvider.GetData();

            var startDate = start;
            var endDate = end ?? measurements.EndDate();

            var intervals = _intervalProvider.SampleIntervals(startDate, endDate, _timeSpan);
            var samples = _sampleProvider.Sample(measurements, intervals);
            
            var sampledMeasurements = NormalizeMeasureTimes(samples);

            return sampledMeasurements;
        }

        private IEnumerable<Measurement> NormalizeMeasureTimes(IEnumerable<Sample> samples)
        {
            foreach (var sample in samples)
            {
                sample.Measurements.SetValue(x => x.Time = sample.Interval.End);
            }

            return samples.SelectMany(x => x.Measurements);
        }
    }
}