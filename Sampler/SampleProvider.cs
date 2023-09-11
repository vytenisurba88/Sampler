using Sampler.Extensions;
using Sampler.Models;

namespace Sampler
{
    public interface ISampleProvider
    {
        IEnumerable<Sample> Sample(IEnumerable<Measurement> measurements, IEnumerable<Interval> sampleIntervals);
        IEnumerable<Measurement> SampleInterval(IEnumerable<Measurement> measurements, Interval interval);
    }

    public class SampleProvider : ISampleProvider
    {
        public IEnumerable<Sample> Sample(IEnumerable<Measurement> measurements, IEnumerable<Interval> sampleIntervals)
        {
            var sampledIntervals = new List<Sample>();
            foreach (var interval in sampleIntervals)
            {
                sampledIntervals.Add(new Sample
                {
                    Measurements = SampleInterval(measurements, interval),
                    Interval = interval
                });
            }

            return sampledIntervals;
        }

        // Filter out measurement for each interval
        public IEnumerable<Measurement> SampleInterval(IEnumerable<Measurement> measurements, Interval interval) =>
            measurements
                .Where(x => x.Time > interval.Start)
                .Where(x => x.Time <= interval.End)
                .OrderByDescending(x => x.Time)
                .GroupBy(x => x.Type)
                .Select(x => x.First());
    }
}