using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Cumilative1.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Cumilative1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Cumilative1Context") ?? throw new InvalidOperationException("Connection string 'Cumilative1Context' not found.")));


builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();    
builder.Services.AddSwaggerGen();              

builder.Services.AddSingleton<Cumilative1.Models.SchoolDbContext>(); 

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(); 


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
