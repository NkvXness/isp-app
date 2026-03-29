using IspApp.Extensions;
using IspApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IspContext>(options =>
    options.UseSqlServer(connectionString));

// регистрируем сервис серверного кэширования ответов
builder.Services.AddResponseCaching();

// 2 * 11 + 240 = 262 секунды
builder.Services.AddControllersWithViews(options =>
{
    options.CacheProfiles.Add("Default", new CacheProfile
    {
        Duration = 262
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseResponseCaching();

app.UseAuthorization();

// инициализируем БД тестовыми данными при первом запуске
app.UseDbInitializer();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();