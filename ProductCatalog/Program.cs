using ProductCatalog.Web.Data.IService;
using ProductCatalog.Data.Service;
using ProductCatalog.Web.Components;
using ProductCatalog.Web.Data.Service;

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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
