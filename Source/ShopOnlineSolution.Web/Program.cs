using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShopOnlineSolution.Web;
using ShopOnlineSolution.Web.Services;
using ShopOnlineSolution.Web.Services.Contracts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7122/") });
builder.Services.AddScoped<IProductService, ProductService>();

await builder.Build().RunAsync();
