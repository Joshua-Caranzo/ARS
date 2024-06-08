using ARS.API.Services.Email;
using ARS.API.Services.School;
using ARS.API.Services.SchoolYear;
using ARS.API.Services.Section;
using ARS.API.Services.Student;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using MySqlConnector;
using Payroll.API.Context;
using Payroll.API.Services.AppSettings;
using Payroll.API.Services.User;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var dbConnectionString = builder.Configuration.GetConnectionString("PayrollSystem");
builder.Services.AddDbContext<ParyollContext>(options =>
    options.UseMySql(dbConnectionString, new MySqlServerVersion("8.4.0")));




builder.Services.AddScoped<IDbConnection>(_ => new MySqlConnection(dbConnectionString));


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAppSettingsService, AppSettingsService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISchoolYearService, SchoolYearService>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseStaticFiles(new StaticFileOptions
{

});

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
