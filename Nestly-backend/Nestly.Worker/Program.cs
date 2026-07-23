using Microsoft.EntityFrameworkCore;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;
using Nestly.Services.MachineLearning;
using Nestly.Services.Messaging;
using Nestly.Services.Repository;
using Nestly.Worker.Messaging;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior =
        BackgroundServiceExceptionBehavior.Ignore;
});

builder.Services.AddDbContext<NestlyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("db1")));

builder.Services.AddSignalRCore();

builder.Services.AddScoped<IBabyHealthMonitoringService, BabyHealthMonitoringService>();
builder.Services.AddPediatricMLModels();

builder.Services.AddHostedService<RabbitMqConsumer>();
builder.Services.AddHostedService<CalendarReminderService>();
builder.Services.AddHostedService<DailyParentReminderService>();
builder.Services.AddHostedService<MedicationReminderService>();
builder.Services.AddHostedService<HealthDeviationMonitoringService>();
builder.Services.AddSingleton<RabbitMqPublisher>();

var host = builder.Build();
host.Run();