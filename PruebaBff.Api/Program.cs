
var builder = WebApplication.CreateBuilder(args);

// config
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// HttpClient for Rick & Morty API
builder.Services.AddHttpClient("rick", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("RickAndMortyApi:BaseUrl") ?? "https://rickandmortyapi.com/api/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// DI registrations
builder.Services.AddScoped<PruebaBff.Core.Interfaces.IRickAndMortyService, PruebaBff.Infrastructure.RickAndMortyService>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    var http = factory.CreateClient("rick");
    return new PruebaBff.Infrastructure.RickAndMortyService(http);
});

// CORS (allow dev Angular origin)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
    });
});

var app = builder.Build();

app.UseSwagger();
///app.UseSwaggerUI();

app.UseCors("AllowAngularDev");

app.UseHttpsRedirection();
app.MapControllers();

app.Run();