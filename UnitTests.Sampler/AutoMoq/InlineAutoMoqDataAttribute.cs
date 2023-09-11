using AutoFixture.NUnit3;

namespace UnitTests.Sampler.AutoMoq
{
    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] objects) : base(objects) { }
    }
}

