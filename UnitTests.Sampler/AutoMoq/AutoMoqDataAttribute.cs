using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace UnitTests.Sampler.AutoMoq
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        [Obsolete]
        public AutoMoqDataAttribute()
            : base(new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }

}

