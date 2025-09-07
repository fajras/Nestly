//using Microsoft.EntityFrameworkCore;
//using Nestly.Services.Data;
//using Nestly.Services.Interfaces;
//using Nestly.Services.Repository;

//var builder = WebApplication.CreateBuilder(args);

//var config = new ConfigurationBuilder()
//    .AddJsonFile("appsettings.json", false)
//    .Build();

//builder.Services.AddDbContext<NestlyDbContext>(options =>
//    options.UseSqlServer(config.GetConnectionString("db1")));


//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<IAppUserService, AppUserService>();
//builder.Services.AddScoped<IBabyGrowthService, BabyGrowthService>();
//builder.Services.AddScoped<IBabyProfileService, BabyProfileService>();
//builder.Services.AddScoped<IBlogPostService, BlogPostService>();
//builder.Services.AddScoped<IBabyProfileService, BabyProfileService>();
//builder.Services.AddScoped<ICalendarEventService, CalendarEventService>();
//builder.Services.AddScoped<IDiaperLogService, DiaperLogService>();
//builder.Services.AddScoped<IFeedingLogService, FeedingLogService>();
//builder.Services.AddScoped<IFetalDevelopmentWeekService, FetalDevelopmentWeekService>();
//builder.Services.AddScoped<IHealthEntryService, HealthEntryService>();
//builder.Services.AddScoped<IMealPlanService, MealPlanService>();
//builder.Services.AddScoped<IMedicationPlanService, MedicationPlanService>();
//builder.Services.AddScoped<IMedicationScheduleTimeService, MedicationScheduleTimeService>();
//builder.Services.AddScoped<IMilestoneService, MilestoneService>();
//builder.Services.AddScoped<IPregnancyService, PregnancyService>();
//builder.Services.AddScoped<ISleepLogService, SleepLogService>();
//builder.Services.AddScoped<IWeeklyAdviceService, WeeklyAdviceService>();
//builder.Services.AddScoped<IQaQuestionService, QaQuestionService>();
//builder.Services.AddScoped<IQaAnswerService, QaAnswerService>();


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowLocalhost4200",
//        builder => builder
//            .WithOrigins("https://localhost:4200")
//            .AllowAnyMethod()
//            .AllowAnyHeader());
//});
//app.Use(async (context, next) =>
//{
//    context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");
//    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
//    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
//    await next();
//});

//app.UseSwagger();
//app.UseSwaggerUI();
//app.UseCors("AllowLocalhost4200");

//app.UseCors(
//    options => options
//        .AllowAnyMethod()
//        .AllowAnyHeader()
//        .AllowCredentials()
//); //This needs to set everything allowed

//app.UseAuthentication();

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using Microsoft.EntityFrameworkCore;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;
using Nestly.Services.Repository;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddDbContext<NestlyDbContext>(options =>
    options.UseSqlServer(
        config.GetConnectionString("db1"),
        b => b.MigrationsAssembly("Nestly WebAPI")
    ));


builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IBabyGrowthService, BabyGrowthService>();
builder.Services.AddScoped<IBabyProfileService, BabyProfileService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();
builder.Services.AddScoped<ICalendarEventService, CalendarEventService>();
builder.Services.AddScoped<IDiaperLogService, DiaperLogService>();
builder.Services.AddScoped<IFeedingLogService, FeedingLogService>();
builder.Services.AddScoped<IFetalDevelopmentWeekService, FetalDevelopmentWeekService>();
builder.Services.AddScoped<IHealthEntryService, HealthEntryService>();
builder.Services.AddScoped<IMealPlanService, MealPlanService>();
builder.Services.AddScoped<IMedicationPlanService, MedicationPlanService>();
builder.Services.AddScoped<IMedicationScheduleTimeService, MedicationScheduleTimeService>();
builder.Services.AddScoped<IMilestoneService, MilestoneService>();
builder.Services.AddScoped<IPregnancyService, PregnancyService>();
builder.Services.AddScoped<ISleepLogService, SleepLogService>();
builder.Services.AddScoped<IWeeklyAdviceService, WeeklyAdviceService>();
builder.Services.AddScoped<IQaQuestionService, QaQuestionService>();
builder.Services.AddScoped<IQaAnswerService, QaAnswerService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200", p => p
        .WithOrigins("http://localhost:4200") // ili dodaj i "https://localhost:4200"
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowLocalhost4200");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

