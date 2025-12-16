using InTend_ProductAndShoppingCart.Business.Api;
using InTend_ProductAndShoppingCart.Business.Repository;
using InTend_ProductAndShoppingCart.Data.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<ProductApi>();
builder.Services.AddScoped<ShoppingCartApi>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



