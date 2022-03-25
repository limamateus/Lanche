using Lanche.Areas.Admin.Services;
using Lanche.Context;
using Lanche.Models;
using Lanche.Repositories;
using Lanche.Repositories.Interfaces;
using Lanche.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Lanche;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<IdentityUser, IdentityRole>() // Serviço que permite gerenciar os perfils
            .AddEntityFrameworkStores<AppDbContext>()// recuperar os dados do usuario
            .AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(options =>
        {

            options.Password.RequireDigit = false; // Requisito de um digito
            options.Password.RequireLowercase = false; // Requisito de um caracter minusculo
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false; // uma letra maiuscula
            options.Password.RequiredLength = 3; // tamanho minimo da senha
            options.Password.RequiredUniqueChars = 1; // repetição de caracter

        });


        services.AddTransient<ICategoriaRepository, CategoriaRepository>();
        services.AddTransient<IProdutoRepository, ProdutoRepository>();
        services.AddTransient<IPedidoRepository, PedidoRepository>();

        services.AddScoped<RelatorioVendasServices>();
        services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

        services.Configure<ConfigurationImagens>(Configuration.GetSection("ConnectionPastaImagens"));

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", politica =>
            {
                politica.RequireRole("Admin");
            });
        });
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));
        services.AddControllersWithViews();

        services.AddPaging(options =>
        {
            options.ViewName = "Bootstrap4";
            options.PageParameterName = "pageindex";

        });


        services.AddMemoryCache();
        services.AddSession();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedUserRoleInitial seedUserRoleInitial)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseRouting();
        seedUserRoleInitial.SeedRole();
        seedUserRoleInitial.SeedUsers();
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
                 );
            endpoints.MapControllerRoute(
                name: "categoriaFiltro",
                pattern: "Produto/{action}/{categoria}",
                defaults: new { controller = "Produto", action = "List" }

                );
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


        });
    }
}