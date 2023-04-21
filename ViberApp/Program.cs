using Viber.Bot.NetCore.Middleware;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using ViberApp.Models;
using ViberApp.Services.ViberConrolServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ViberDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddViberBotApi(opt =>
{
    opt.Token = "50e3edf02727e768-f7acbe1bb3344977-76094c86e1902170";
    opt.Webhook = "https://0e09-194-44-96-160.ngrok-free.app/Viber";
});
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped<IViberControlService, ViberControlService>();

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
