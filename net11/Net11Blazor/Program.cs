using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.AI;
using Net11Blazor;
using Net11Blazor.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<CircuitAccessor>();
builder.Services.AddScoped<CircuitHandler>(services => services.GetRequiredService<CircuitAccessor>());
builder.Services.AddSingleton<IChatClient, DemoChatClient>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    // Blazor 11: configure the browser side from server C# instead of Blazor.start() in JavaScript
    .WithBrowserOptions(options =>
    {
        options.LogLevel = LogLevel.Information;
        options.InteractiveServer.ReconnectionMaxRetries = 10;
        options.InteractiveServer.ReconnectionRetryInterval = TimeSpan.FromSeconds(2);
    });

app.Run();
