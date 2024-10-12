using DatabaseLab.API.Middlewares;
using DatabaseLab.DAL.Interfaces;
using DatabaseLab.DAL.Repositories.Default;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .Configure<DbOptions>(builder.Configuration
    .GetSection(nameof(DbOptions)));

builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<ISpectacleRepository, SpectacleRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IRepository<ActorDetail>, ActorDetailsRepository>();

var app = builder.Build();

app.UseMiddleware<ValidationLoggingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();
