using FluentValidation;
using GerenciadorContatos.Application.DTOs.Contacts;
using GerenciadorContatos.Application.Services.Contacts;
using GerenciadorContatos.Application.Validators.Contacts;
using GerenciadorContatos.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

builder.Services.AddInfrastructure(connectionString);

builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddScoped<IValidator<CreateContactRequest>, CreateContactRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateContactRequest>, UpdateContactRequestValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;
