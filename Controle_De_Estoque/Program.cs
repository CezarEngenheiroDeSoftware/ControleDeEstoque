using Controle_De_Estoque.Repository;
using Controle_De_Estoque.Iservice;
using Microsoft.EntityFrameworkCore;
using Controle_De_Estoque.Data;
using Controle_De_Estoque.Service;
using Controle_De_Estoque.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddScoped<IMercadoLivreService, MercadoLivreRepository>();
builder.Services.AddScoped<IWooCommerceService, WooCommerceRepository>();
builder.Services.AddScoped<ILogin, LoginRepository>();
builder.Services.AddScoped<ISession, SessionRepository>();
builder.Services.AddScoped<MemoryService>();
builder.Services.AddScoped<IServiceAPIMercadoLivre, ServiceAPIMercadoLivre>();
builder.Services.AddScoped<IWooCommerceService, WooCommerceRepository>();
builder.Services.AddScoped<WooCommerce>();
builder.Services.AddScoped<ServiceConfigurations>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.Configure<MeliAcess>(builder.Configuration.GetSection("MeliAcess"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSession();
app.UseAntiforgery();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
     pattern: "{controller=Login}/{action=LoginExecute}/{id?}");

app.Run();
