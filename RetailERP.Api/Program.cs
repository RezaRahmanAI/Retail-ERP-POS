using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RetailERP.Application.Interfaces;
using RetailERP.Application.Mappings;
using RetailERP.Application.Services;
using RetailERP.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RetailERP API",
        Version = "v1"
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ErpDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "RetailERP API v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
