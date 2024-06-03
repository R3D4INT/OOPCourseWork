using Microsoft.Extensions.DependencyInjection;

namespace UI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = DependencyRegistration.Register();
            var app = serviceProvider.GetRequiredService<App>(); 
            await app.Run();
        }
    }
}
