using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using Sampler;
using Sampler.DataProviders;
using Sampler.Models;
using UnitTests.Sampler.AutoMoq;

namespace UnitTests.Sampler
{
    public class SampleServiceTests
    {
        [Test]
        [AutoMoqData()]
        public void SampleServiceSampleData_SamplesProvided_MeasurementReturnedWithIntervalTimes(
            [Frozen] Mock<IIntervalProvider> intervalProvider,
            [Frozen] Mock<ISampleProvider> sampleProvider,
            [Frozen] Mock<IDataProvider> dataProvider,
            SampleService sampleService)
        {
            // Arrange          
            sampleProvider
                .Setup(x => x.Sample(It.IsAny<IEnumerable<Measurement>>(), It.IsAny<IEnumerable<Interval>>())).Returns(Samples);

            // Act
            var sampledMeasurements = sampleService.SampleData(StartDate, EndDate).ToList();


            // Assert
            dataProvider.Verify(x => x.GetData(), Times.Once);
            intervalProvider.Verify(x => x.SampleIntervals(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<TimeSpan>()), Times.Once);
            sampleProvider.Verify(x => x.Sample(It.IsAny<IEnumerable<Measurement>>(), It.IsAny<IEnumerable<Interval>>()), Times.Once);

            sampledMeasurements.Should().HaveCount(4);
            sampledMeasurements[0].Time.Should().Be(FirstIntervalEndDate);
            sampledMeasurements[1].Time.Should().Be(FirstIntervalEndDate);
            sampledMeasurements[2].Time.Should().Be(EndDate);
            sampledMeasurements[3].Time.Should().Be(EndDate);
        }

        private static readonly DateTime StartDate = DateTime.Parse("2001.01.01 10:00:00");
        private static readonly DateTime FirstIntervalEndDate = DateTime.Parse("2001.01.01 10:05:00");
        private static readonly DateTime EndDate = DateTime.Parse("2001.01.01 10:10:00");

        private static List<Sample> Samples =>
            new List<Sample> {
                new Sample {
                    Interval = new Interval { Start = StartDate, End = FirstIntervalEndDate },
                    Measurements = new List<Measurement>
                    {
                        new Measurement { Time = DateTime.Parse("2001.01.01 10:00:05"), MeasurementValue = 36.6M, Type = MeasurementType.Temp  },
                        new Measurement {  Time =DateTime.Parse("2001.01.01 10:04:05"), MeasurementValue = 99M, Type = MeasurementType.SpO2 }
                    }
                },
                new Sample
                {
                    Interval = new Interval { Start = FirstIntervalEndDate, End = EndDate },
                    Measurements = new List<Measurement>
                    {
                        new Measurement { Time = DateTime.Parse("2001.01.01 10:07:05"), MeasurementValue = 37.6M, Type = MeasurementType.Temp  },
                        new Measurement {  Time =DateTime.Parse("2001.01.01 10:09:05"), MeasurementValue = 10M, Type = MeasurementType.SpO2 }
                    }
                }
            };

    }
}
