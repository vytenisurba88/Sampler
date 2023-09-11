using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sampler.DataProviders;
using Sampler.Models;

namespace Sampler
{
    internal partial class Program
    {
        public static void Main(string[] args)
        {
            var startDate = DateTime.Parse("2017-01-03T10:00:00");

            var serviceProvider = BuildServiceProvider();
            var sampleService = serviceProvider.GetRequiredService<ISampleService>();
            var samples = sampleService.SampleData(startDate);

            PrettyPrint(samples);
            Console.ReadKey();
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddTransient<ISampleProvider, SampleProvider>();
            services.AddTransient<IIntervalProvider, IntervalProvider>();
            services.AddTransient<IDataProvider, DummyDataProvider>();
            services.AddSingleton(new SamplerConfiguration { SampleInterval = TimeSpan.FromMinutes(5) });
            services.AddTransient<ISampleService, SampleService>();

            return services.BuildServiceProvider();
        }

        public static void PrettyPrint(IEnumerable<Measurement> measurements)
        {
            foreach (var sample in measurements)
            {
                Console.WriteLine($"{sample.Time} {sample.Type} {sample.MeasurementValue}");
            }
        }
    }
}