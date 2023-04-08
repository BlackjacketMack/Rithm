using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Rithm;
using Rithm.Docs;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.Configure<RithmOptions>(builder.Configuration.GetSection(nameof(RithmOptions)));

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddRithm(options =>
{
    options.AddDocumentation();
});

await builder.Build().RunAsync();
