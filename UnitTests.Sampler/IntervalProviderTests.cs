using FluentAssertions;
using Sampler;
using Sampler.Models;
using UnitTests.Sampler.AutoMoq;

namespace UnitTests.Sampler
{
    public class IntervalProviderTests
    {
        [Test]
        [InlineAutoMoqData("2020.01.01 13:00:00", "2020.01.01 14:00:00", 5, 12)]
        [InlineAutoMoqData("2020.01.01 13:00:00", "2020.01.01 13:01:00", 5, 1)]
        [InlineAutoMoqData("2020.01.01 13:00:00", "2020.01.01 13:06:00", 5, 2)]
        public void IntervalProvider_StartEndDatesAndSpanProvided_ReturnsExpectedAmountOfIntervals(
            string startDateString, 
            string endDateString, 
            int spanSizeInMinutes, 
            int expectedIntervalCount, 
            IntervalProvider intervalProvider)
        {
            // Arrange
            var startDate = DateTime.Parse(startDateString);
            var endDate = DateTime.Parse(endDateString);
            var span = TimeSpan.FromMinutes(spanSizeInMinutes);

            // Act
            var intervals = intervalProvider.SampleIntervals(startDate, endDate, span);

            // Assert
            intervals.Should().HaveCount(expectedIntervalCount);
            intervals.SingleOrDefault(x => x.Start == startDate).Should().NotBeNull();
            intervals.SingleOrDefault(x => x.End > endDate.Add(span)).Should().BeNull();

            AssertMatchingStartAndEnd(intervals, startDate, endDate);
        }

        private void AssertMatchingStartAndEnd(List<Interval> intervals, DateTime start, DateTime end)
        {
            //TODO: implement
        }
    }
}

