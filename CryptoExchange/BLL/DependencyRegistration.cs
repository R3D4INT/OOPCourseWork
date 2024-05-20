using BLL.Implementations;
using BLL.Interfaces.IOperationsInterfaces;
using BLL.Interfaces.IServiceInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public class DependencyRegistration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICoinService, CoinService>();
            services.AddScoped<IMarketWalletService, WalletForMarketService>();
            services.AddScoped<ISeedPhraseService, SeedPhraseService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ISupportService, SupportService>();
            services.AddScoped<IFuethersDealService, FuethersDealService>();
            services.AddScoped<IDepositService, DepositService>();
            services.AddScoped<IAdminService, AdminService>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            DAL.DependencyRegistration.RegisterRepositories(services, configuration);
        }
    }
}
