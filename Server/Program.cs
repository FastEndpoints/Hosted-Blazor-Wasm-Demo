global using FastEndpoints;
using FastEndpoints.ClientGen;
using FastEndpoints.Swagger;
using NJsonSchema.CodeGeneration.CSharp;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerDoc(
        s => s.DocumentName = "MyApi",
        shortSchemaNames: true,
        removeEmptySchemas: true);
}

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseOpenApi();
    app.UseSwaggerUi3(s => s.ConfigureDefaults());
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
app.UseRouting();
app.UseAuthorization();
app.UseFastEndpoints(c =>
{
    c.Endpoints.ShortNames = true;
    c.Serializer.Options.PropertyNamingPolicy = null;
});

//NOTE: just run `dotnet run --generateclients true` anytime you wanna update the ApiClient in the MyProj.Client project

await app.GenerateClientsAndExitAsync(
    documentName: "MyApi",
    destinationPath: "../Client/HttpClient",
    csSettings: c =>
    {
        c.ClassName = "ApiClient";
        c.CSharpGeneratorSettings.Namespace = "MyProj.Client";
        c.CSharpGeneratorSettings.JsonLibrary = CSharpJsonLibrary.SystemTextJson;
    },
    tsSettings: null);

app.Run();
