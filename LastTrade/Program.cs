using LastTrade.Application.RepoContract;
using LastTrade.Application.Services;
using LastTrade.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITradeRepo, TradeRepo>();
builder.Services.AddScoped<ITradeService, TradeService>();
builder.Host.UseDefaultServiceProvider(options =>
            options.ValidateScopes = false);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/LastTrade", async (DateTime? date) =>
 {

     var _TradeService = (ITradeService)app.Services.GetRequiredService(typeof(ITradeService));
    

     var res = await _TradeService.GetLastTradeAsync(date);
 
     await _TradeService.SaveLastTrades(res);
     
     return await Task.Run(() => res);
 });


app.Run();
