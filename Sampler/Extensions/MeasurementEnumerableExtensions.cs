using Sampler.Models;

namespace Sampler.Extensions
{
    public static class MeasurementEnumerableExtensions
    {
        public static DateTime StartDate(this IEnumerable<Measurement> measurements) => measurements.Min(x => x.Time);
        public static DateTime EndDate(this IEnumerable<Measurement> measurements) => measurements.Max(x => x.Time);      
    }
}