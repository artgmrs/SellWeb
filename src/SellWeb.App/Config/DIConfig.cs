using Microsoft.Extensions.DependencyInjection;
using SellWeb.Business.Interfaces;
using SellWeb.Business.Notifications;
using SellWeb.Business.Services;
using SellWeb.Data.Context;
using SellWeb.Data.Repository;

namespace SellWeb.App.Config
{
    public static class DIConfig
    {
        public static IServiceCollection ResolverDependencias(this IServiceCollection services)
        {
            services.AddScoped<SellWebDbContext>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IFornecedorService, FornecedorService>();
            services.AddScoped<IProdutoService, ProdutoService>();

            return services;
        }
    }
}
