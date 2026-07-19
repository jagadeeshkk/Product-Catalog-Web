using ProductCatalog.Web.Business.IService;
using ProductCatalog.Data.Service;
using ProductCatalog.Web.Components;
using ProductCatalog.Web.Business.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddJsonConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
});
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IProductApiService, ProductApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7112/");
});
builder.Services.AddSingleton<IProductFilterService, ProductFilterService>();
builder.Services.AddSingleton<IShareMessageBuilder, ShareMessageBuilder>();
builder.Services.AddScoped<IAddedItemsService, AddedItemsService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
