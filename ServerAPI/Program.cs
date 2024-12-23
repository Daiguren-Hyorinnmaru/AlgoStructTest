using Microsoft.EntityFrameworkCore;
using ServerAPI.DataBase;
using DataBaseModels.Models;
using ServerAPI.DataBase.Repository;
using ServerAPI.DataBase.UnitOfWork;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options => options.UseSqlite("Data Source=TestingSystem.db"));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//to delete
//MyStartupAction();

app.Run();

//to delete
async void MyStartupAction()
{
    Console.WriteLine("Start MyStartupAction");

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        

    }
    Console.WriteLine("End MyStartupAction");
}