using DatabaseLab.API.Middlewares;
using DatabaseLab.DAL.Interfaces;
using DatabaseLab.DAL.Repositories.Default;
using DatabaseLab.DAL.Repositories.Logging;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Options;
using DatabaseLab.Services.Implementations;
using DatabaseLab.Services.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .Configure<DbOptions>(builder.Configuration
    .GetSection(nameof(DbOptions)));

builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.Decorate<IActorRepository, LoggingActorRepository>();

builder.Services.AddScoped<ISpectacleRepository, SpectacleRepository>();
builder.Services.Decorate<ISpectacleRepository, LoggingSpectacleRepository>();

builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.Decorate<IContractRepository, LoggingContractRepository>();

builder.Services.AddScoped<IActorDetailRepository, ActorDetailsRepository>();
builder.Services.Decorate<IActorDetailRepository, LoggingActorDetailsRepository>();

builder.Services.AddScoped<IAgencyRepository, AgencyRepository>();
builder.Services.Decorate<IAgencyRepository, LoggingAgencyRepository>();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IReportService, ReportService>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<IPdfGenerator, PdfGenerator>();
builder.Services.AddSingleton<IConverter, SynchronizedConverter>(sp =>
{
    return new SynchronizedConverter(new PdfTools());
});

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
