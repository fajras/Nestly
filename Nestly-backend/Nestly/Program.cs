using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;
using Nestly.Services.Repository;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddDbContext<NestlyDbContext>(options =>
    options.UseSqlServer(
        config.GetConnectionString("db1"),
        b => b.MigrationsAssembly("Nestly.Services")
    ));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(
        config.GetConnectionString("db1"),
        b => b.MigrationsAssembly("Nestly.Services")
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
builder.Services.AddScoped<IMilestoneService, MilestoneService>();
builder.Services.AddScoped<IPregnancyService, PregnancyService>();
builder.Services.AddScoped<ISleepLogService, SleepLogService>();
builder.Services.AddScoped<IWeeklyAdviceService, WeeklyAdviceService>();
builder.Services.AddScoped<IQaQuestionService, QaQuestionService>();
builder.Services.AddScoped<IQaAnswerService, QaAnswerService>();

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Nestly")
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        AuthenticationType = "Jwt",
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();


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

