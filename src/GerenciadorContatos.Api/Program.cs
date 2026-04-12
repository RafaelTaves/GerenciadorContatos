using FluentValidation;
using GerenciadorContatos.Application.UseCases.Contacts.Activate;
using GerenciadorContatos.Application.UseCases.Contacts.Create;
using GerenciadorContatos.Application.UseCases.Contacts.Deactivate;
using GerenciadorContatos.Application.UseCases.Contacts.Delete;
using GerenciadorContatos.Application.UseCases.Contacts.GetAll;
using GerenciadorContatos.Application.UseCases.Contacts.GetById;
using GerenciadorContatos.Application.UseCases.Contacts.Update;
using GerenciadorContatos.Communication.Requests;
using GerenciadorContatos.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

builder.Services.AddInfrastructure(connectionString);

builder.Services.AddScoped<CreateContactUseCase>();
builder.Services.AddScoped<UpdateContactUseCase>();
builder.Services.AddScoped<GetContactByIdUseCase>();
builder.Services.AddScoped<GetAllContactsUseCase>();
builder.Services.AddScoped<ActivateContactUseCase>();
builder.Services.AddScoped<DeactivateContactUseCase>();
builder.Services.AddScoped<DeleteContactUseCase>();

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
