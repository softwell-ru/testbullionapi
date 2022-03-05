using Microsoft.EntityFrameworkCore;
using test_api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BullionContext>(options => options.UseInMemoryDatabase(databaseName: "Bullions"));

var app = builder.Build();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<BullionContext>();

context.AddRange(
    new Bullion
    {
        Name = "10001",
        Weight = 5
    },

new Bullion
{
    Name = "10002",
    Weight = 3
},

new Bullion
{
    Name = "10003",
    Weight = 2
},

new Bullion
{
    Name = "10004",
    Weight = 1
},

new Bullion
{
    Name = "10005",
    Weight = 1
},

new Bullion
{
    Name = "10006",
    Weight = 22
},

new Bullion
{
    Name = "10007",
    Weight = 21
}
);
context.SaveChanges();
var inBasket = context.Bullions.Where(x => x.Name == "10004" || x.Name == "10003").ToList();
context.Add(
    new Basket()
    {
        Bullions = inBasket
    }
);
context.SaveChanges();

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
