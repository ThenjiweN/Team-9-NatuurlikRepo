using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using NatuurlikBase.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using NatuurlikBase.Services;
using NatuurlikBase.Repository.IRepository;
using NatuurlikBase.Repository;
using NatuurlikBase.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddDefaultTokenProviders().AddDefaultUI()
    .AddEntityFrameworkStores<DatabaseContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });
builder.Services.AddTransient<IProductInventoryRepository, ProductInventoryRepository>();
builder.Services.AddTransient<IInventoryItemRepository, InventoryItemRepository>();
builder.Services.AddTransient<IProductTransactionRepository, ProductTransactionRepository>();
builder.Services.AddTransient<IInventoryItemTransactionRepository, InventoryItemTransactionRepository>();
builder.Services.AddTransient<IViewProductsByName, ViewProductsByName>();
builder.Services.AddTransient<IViewInventoriesByName, ViewInventoriesByName>();
builder.Services.AddTransient<IViewInventoryById, ViewInventoryById>();
builder.Services.AddTransient<IViewProductById, ViewProductById>();
builder.Services.AddTransient<IEditProduct, EditProduct>();
builder.Services.AddTransient<IProduceFinishedProduct, ProduceFinishedProduct>();
builder.Services.AddTransient<IValidateEnoughInventories, ValidateEnoughInventories>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();
builder.Services.AddRazorPages();

//Configure Application Cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    options.LogoutPath = $"/Identity/Account/Logout";
}
);


// Add services to the container.

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

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
app.UseAuthentication();;

app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    app.MapFallbackToPage("/_Host");
});



app.Run();

