using ChannelEngine.Core.Helpers;
using ChannelEngine.Core.Services.Orders;
using ChannelEngine.Core.Services.Products;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register configuration
builder.Services.Configure<ChannelEngineSettings>(builder.Configuration.GetSection("ChannelEngine"));

// Register HttpClient and services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddHttpClient<IOrderApiClient, OrderApiClient>();


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

app.Run();
