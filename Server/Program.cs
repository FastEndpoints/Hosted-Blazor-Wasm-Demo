using FastEndpoints.ClientGen;
using NJsonSchema.CodeGeneration.CSharp;

var bld = WebApplication.CreateBuilder(args);
bld.Services
   .AddAuthenticationJwtBearer(s => s.SigningKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")
   .AddAuthorization()
   .AddFastEndpoints();

if (bld.Environment.IsDevelopment())
{
    bld.Services.SwaggerDocument(
        o =>
        {
            o.DocumentSettings = d => d.DocumentName = "MyApi";
            o.ShortSchemaNames = true;
            o.RemoveEmptyRequestSchema = true;
        });
}

var app = bld.Build();

if (app.Environment.IsDevelopment())
    app.UseWebAssemblyDebugging();

app.UseBlazorFrameworkFiles()
   .UseStaticFiles();
app.MapFallbackToFile("index.html");
app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints(
       c =>
       {
           c.Endpoints.ShortNames = true;
           c.Serializer.Options.PropertyNamingPolicy = null;
       });

if (app.Environment.IsDevelopment())
    app.UseSwaggerGen();

//NOTE: just run `dotnet run --generateclients true` anytime you want to update the ApiClient in the MyProj.Client project
//TODO: upgrade to kiota client generation
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