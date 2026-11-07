using ClinicManagementBackend.Application.Interfaces;
using ClinicManagementBackend.Application.Services;
using ClinicManagementBackend.Infrastructure;
using ClinicManagementBackend.Infrastructure.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register Infrastructure (EF Core, SQLite, Repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// Register Application Services
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();

var app = builder.Build();

// Automatically ensure SQLite database and tables are created on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ClinicDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Interactive API UI at: /scalar/v1
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
