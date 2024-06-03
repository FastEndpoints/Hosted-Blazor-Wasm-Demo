using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyProj.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(
    new ApiClient(
        builder.HostEnvironment.BaseAddress,
        new() { BaseAddress = new(builder.HostEnvironment.BaseAddress) }));

await builder.Build().RunAsync();