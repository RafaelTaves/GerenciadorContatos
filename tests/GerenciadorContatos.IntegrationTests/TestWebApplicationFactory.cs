using CommonTestUtilities.Repositories;
using GerenciadorContatos.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GerenciadorContatos.IntegrationTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    public InMemoryContactRepository Repository { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] =
                    "Server=(localdb)\\MSSQLLocalDB;Database=GerenciadorContatosIntegrationTestsDb;Trusted_Connection=True;TrustServerCertificate=True"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IContactRepository>();
            services.AddSingleton<IContactRepository>(Repository);
        });
    }
}
