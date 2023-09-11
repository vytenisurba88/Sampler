namespace Sampler.Models
{
    // TODO: This is Contract AND internal Model, ideally, this should be separate
    public class Measurement
    {
        public DateTime Time { get; set; }
        public decimal MeasurementValue { get; set; }
        public MeasurementType Type { get; set; }
    }
}