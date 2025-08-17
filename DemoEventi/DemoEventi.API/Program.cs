using DemoEventi.Application.Interfaces;
using DemoEventi.Application.Services;
using DemoEventi.Application.Validators;
using DemoEventi.Application.Common.Behaviors;
using DemoEventi.Application.Mapping;
using DemoEventi.Infrastructure.Extensions;
using DemoEventi.Infrastructure.Data;
using FluentValidation;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// TODO: Add Swagger when package is installed
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "DemoEventi API", Version = "v1" });
});

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMauiBlazor", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Infrastructure layer
builder.Services.AddInfrastructure(builder.Configuration);

// Application layer
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

// MediatR Pipeline Behaviors
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Application services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IInterestService, InterestService>();

var app = builder.Build();

// Seed the database with mock data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DataSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Commented out for mobile development - causes timeout issues
// app.UseHttpsRedirection();

app.UseCors("AllowMauiBlazor");

app.UseAuthorization();

app.MapControllers();

app.Run();
