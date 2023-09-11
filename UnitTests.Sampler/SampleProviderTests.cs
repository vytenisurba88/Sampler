using FluentAssertions;
using Sampler;
using Sampler.Models;
using UnitTests.Sampler.AutoMoq;

namespace UnitTests.Sampler
{
    public class SampleProviderTests
    {
        [Test]
        [AutoMoqData]
        public void SampleProviderSample_OneMeasurementProvided_ShouldReturnOneMeasurement(SampleProvider sampleProvider)
        {
            // Arrange
            var measurements = SingleMeasurement;

            // Act
            var samples = sampleProvider.Sample(measurements, Intervals).ToList();

            // Assert
            samples.Should().HaveCount(2);

            var sample = samples.First();
            sample.Measurements.Should().HaveCount(1);
            sample.Measurements.First().Should().Be(measurements.First());

            samples[1].Measurements.Should().HaveCount(0);
        }

        [Test]
        [AutoMoqData]
        public void SampleProviderSample_OneMeasurementProvidedAtTheEndOfInterval_ShouldReturnOneMeasurement(SampleProvider sampleProvider)
        {
            // Arrange
            var measurements = SingleMeasurementAtTheEndOfInterval;

            // Act
            var samples = sampleProvider.Sample(measurements, Intervals).ToList();

            // Assert
            samples.Should().HaveCount(2);

            var sample = samples.First();
            sample.Measurements.Should().HaveCount(1);
            sample.Measurements.First().Should().Be(measurements.First());

            samples[1].Measurements.Should().HaveCount(0);
        }

        [Test]
        [AutoMoqData]
        public void SampleProviderSample_TwoDifferentTypeMeasurementsProvided_ReturnsBothMeasurements(SampleProvider sampleProvider)
        {
            // Arrange
            var measurements = TwoDifferentTypeMeasurementsInSingleInterval;

            // Act
            var samples = sampleProvider.Sample(measurements, Intervals).ToList();

            // Assert
            samples.Should().HaveCount(2);

            var sample = samples.First();
            sample.Measurements.Should().HaveCount(2);
            sample.Measurements.Should().Contain(measurements.ToList()[0]);
            sample.Measurements.Should().Contain(measurements.ToList()[1]);

            samples[1].Measurements.Should().HaveCount(0);
        }

        [Test]
        [AutoMoqData]
        public void SampleProviderSample_MultipleSameTypeMeasurementsProvidedInOneInterval_ReturnsOldestOneForInterval(SampleProvider sampleProvider)
        {
            // Arrange
            var measurements = MultipleSameTypeMeasurementsInSingleInterval;
            var latestMeasurement = measurements.MaxBy(x => x.Time);

            // Act
            var samples = sampleProvider.Sample(measurements, Intervals).ToList();

            // Assert
            samples.Should().HaveCount(2);

            var sample = samples.First();
            sample.Measurements.Should().HaveCount(1);
            sample.Measurements.Should().Contain(latestMeasurement);

            samples[1].Measurements.Should().HaveCount(0);
        }

        [Test]
        [AutoMoqData]
        public void SampleProviderSample_MultipleSameTypeMeasurementsProvidedInMultipleIntervals_ReturnsOldestMeasurementForEachInterval(SampleProvider sampleProvider)
        {
            // Arrange
            var measurements = MultipleSameTypeMeasurementsInMultipleInterval;
            var firstIntervalMax = measurements.Where(x => x.Time < SecondIntervalStart).MaxBy(x => x.Time);
            var secondIntervalMax = measurements.Where(x => x.Time < ThirdIntervalStart).MaxBy(x => x.Time);

            // Act
            var samples = sampleProvider.Sample(measurements, Intervals).ToList();

            // Assert
            samples.Should().HaveCount(2);

            var sample = samples.First();
            sample.Measurements.Should().HaveCount(1);
            sample.Measurements.Should().Contain(firstIntervalMax);

            samples[1].Measurements.Should().HaveCount(1);
            samples[1].Measurements.Should().Contain(secondIntervalMax);
        }

        [Test]
        [AutoMoqData]
        public void SampleProviderSample_MultipleDifferentTypeMeasurementsProvidedInSingleIntervals_ReturnsOldestMeasurementForInterval(SampleProvider sampleProvider)
        {
            // Arrange
            var measurements = MultipleDifferentTypeMeasurementsInSingleInterval;
            var tempMeasurement = measurements.Where(x => x.Type == MeasurementType.Temp).MaxBy(x => x.Time);
            var spo2Measurement = measurements.Where(x => x.Type == MeasurementType.SpO2).MaxBy(x => x.Time);

            // Act
            var samples = sampleProvider.Sample(measurements, Intervals).ToList();

            // Assert
            samples.Should().HaveCount(2);

            var sample = samples.First();
            sample.Measurements.Should().HaveCount(2);
            sample.Measurements.Should().Contain(spo2Measurement);
            sample.Measurements.Should().Contain(tempMeasurement);
        }


        // Ideally this should probably go into AutoMoqDataAttributes
        private readonly int IntervalSpan = 5;
        private DateTime FirstIntervalStart => DateTime.Parse("2020.01.01 10:00:00");
        private DateTime SecondIntervalStart => FirstIntervalStart.AddMinutes(IntervalSpan);
        private DateTime ThirdIntervalStart => SecondIntervalStart.AddMinutes(IntervalSpan);

        private IEnumerable<Measurement> SingleMeasurement =>
            new List<Measurement> {
                new Measurement { Time = FirstIntervalStart.AddMinutes(1), MeasurementValue = 37.1M, Type = MeasurementType.Temp }
            };

        private IEnumerable<Measurement> SingleMeasurementAtTheEndOfInterval =>
          new List<Measurement> {
                new Measurement { Time = SecondIntervalStart, MeasurementValue = 37.1M, Type = MeasurementType.Temp }
          };

        private IEnumerable<Measurement> TwoDifferentTypeMeasurementsInSingleInterval =>
            new List<Measurement> {
                new Measurement { Time = FirstIntervalStart.AddMinutes(1), MeasurementValue = 37.1M, Type = MeasurementType.Temp },
                new Measurement { Time = FirstIntervalStart.AddMinutes(2), MeasurementValue = 42, Type = MeasurementType.SpO2 }
            };

        private IEnumerable<Measurement> MultipleSameTypeMeasurementsInSingleInterval =>
            new List<Measurement> {
                   new Measurement { Time = FirstIntervalStart.AddMinutes(3), MeasurementValue = 43, Type = MeasurementType.Temp },
                   new Measurement { Time = FirstIntervalStart.AddMinutes(4), MeasurementValue = 36.1M, Type = MeasurementType.Temp },
                   new Measurement { Time = FirstIntervalStart.AddMinutes(1), MeasurementValue = 37.1M, Type = MeasurementType.Temp },
                   new Measurement { Time = FirstIntervalStart.AddMinutes(2), MeasurementValue = 42, Type = MeasurementType.Temp }
            };

        private IEnumerable<Measurement> MultipleSameTypeMeasurementsInMultipleInterval =>
           new List<Measurement> {
                   new Measurement { Time = FirstIntervalStart.AddMinutes(3), MeasurementValue = 43, Type = MeasurementType.Temp },
                   new Measurement { Time = FirstIntervalStart.AddMinutes(4), MeasurementValue = 36.1M, Type = MeasurementType.Temp },
                   new Measurement { Time = FirstIntervalStart.AddMinutes(6), MeasurementValue = 37.1M, Type = MeasurementType.Temp },
                   new Measurement { Time = FirstIntervalStart.AddMinutes(2), MeasurementValue = 42, Type = MeasurementType.Temp }
           };

        private IEnumerable<Measurement> MultipleDifferentTypeMeasurementsInSingleInterval =>
           new List<Measurement> {
                   new Measurement { Time = FirstIntervalStart.AddMinutes(3), MeasurementValue = 43, Type = MeasurementType.Temp },
                   new Measurement { Time = FirstIntervalStart.AddMinutes(4), MeasurementValue = 36.1M, Type = MeasurementType.SpO2 },
                   new Measurement { Time = FirstIntervalStart.AddMinutes(1), MeasurementValue = 37.1M, Type = MeasurementType.Temp },
                   new Measurement { Time = FirstIntervalStart.AddMinutes(2), MeasurementValue = 42, Type = MeasurementType.SpO2 }
           };

        private IEnumerable<Interval> Intervals =>
            new List<Interval> {
                new Interval { Start = FirstIntervalStart, End = SecondIntervalStart },
                new Interval { Start = SecondIntervalStart, End = ThirdIntervalStart }
            };
    }
}
