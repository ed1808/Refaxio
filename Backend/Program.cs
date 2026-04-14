using Backend.Interfaces;
using Backend.Models;
using Backend.Repositories;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IDocumentIdTypeRepository, DocumentIdTypeRepository>();
builder.Services.AddScoped<IPersonTypeRepository, PersonTypeRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IStorageRepository, StorageRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryMovementRepository, InventoryMovementRepository>();
builder.Services.AddScoped<IPurchaseDetailRepository, PurchaseDetailRepository>();
builder.Services.AddScoped<ISalesDetailRepository, SalesDetailRepository>();
builder.Services.AddScoped<ITransferRepository, TransferRepository>();
builder.Services.AddScoped<ITransferDetailRepository, TransferDetailRepository>();

// Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDocumentIdTypeService, DocumentIdTypeService>();
builder.Services.AddScoped<IPersonTypeService, PersonTypeService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IInventoryMovementService, InventoryMovementService>();
builder.Services.AddScoped<IPurchaseDetailService, PurchaseDetailService>();
builder.Services.AddScoped<ISalesDetailService, SalesDetailService>();
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddScoped<ITransferDetailService, TransferDetailService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
