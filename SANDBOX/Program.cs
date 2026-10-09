using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SANDBOX.AppSettings;
using SANDBOX.Data;
using SANDBOX.Handlers;
using SANDBOX.Mappings;
using SANDBOX.Services.AuthService;
using SANDBOX.Services.GroupsService;
using SANDBOX.Services.ProblemsService;
using SANDBOX.Services.TagsService;
using SANDBOX.Services.UsersService;
using Scalar.AspNetCore;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings")
);
var appSettings = builder.Configuration
    .GetSection("AppSettings")
    .Get<AppSettings>()
    ?? throw new InvalidOperationException("AppSettings missing");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = appSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = appSettings.Audience,

            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(appSettings.Token!)),
            ValidateIssuerSigningKey = true
        };
    });


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLEXPRESS03")));

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IProblemsService, ProblemsService>();
builder.Services.AddScoped<ITagsService, TagsService>();
builder.Services.AddScoped<IGroupsService, GroupsService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAutoMapper(
    cfg => { },
    typeof(MappingProfile)
);

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
