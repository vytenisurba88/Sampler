using Sampler.Models;

namespace Sampler.DataProviders
{
    public interface IDataProvider
    {
        IEnumerable<Measurement> GetData();
    }
}