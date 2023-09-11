namespace Sampler.Models
{
    public class Sample
    {
        public Interval Interval { get; set; }
        public IEnumerable<Measurement> Measurements { get; set; }
    }
}