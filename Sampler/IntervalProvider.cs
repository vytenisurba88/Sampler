using Sampler.Models;

namespace Sampler
{
    public interface IIntervalProvider
    {
        List<Interval> SampleIntervals(DateTime start, DateTime end, TimeSpan intervalsSize);
    }

    public class IntervalProvider : IIntervalProvider
    {

        public List<Interval> SampleIntervals(DateTime start, DateTime end, TimeSpan intervalsSize)
        {
            var currentDate = start;
            var intervals = new List<Interval>();

            while (currentDate < end)
            {
                intervals.Add(new Interval
                {
                    Start = currentDate,
                    End = currentDate.Add(intervalsSize)
                });
                currentDate = currentDate.Add(intervalsSize);
            }

            return intervals;
        }
    }
}