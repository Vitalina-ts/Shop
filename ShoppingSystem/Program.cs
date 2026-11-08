using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(options => options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute()));
builder.Services.AddDbContext<ShoppingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }
// Єдиний формат десяткового вводу в HTML number та серверному model binding.
app.UseRequestLocalization(new RequestLocalizationOptions { DefaultRequestCulture = new RequestCulture("en-US"), SupportedCultures = new[] { new CultureInfo("en-US") }, SupportedUICultures = new[] { new CultureInfo("uk-UA") } });
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
if (!args.Contains("--skip-db-init"))
{
    await using var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetRequiredService<ShoppingContext>();
    await db.Database.MigrateAsync();
    await SampleData.InitializeAsync(db);
}
await app.RunAsync();