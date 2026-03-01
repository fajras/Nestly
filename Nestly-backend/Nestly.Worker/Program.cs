using Microsoft.EntityFrameworkCore;
using Nestly.Services.Data;
using Nestly.Worker.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<NestlyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("db1")));

builder.Services.AddSignalRCore();

builder.Services.AddHostedService<RabbitMqConsumer>();

var host = builder.Build();
host.Run();