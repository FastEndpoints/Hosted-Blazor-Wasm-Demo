namespace Endpoints;

public class GetForcastsRequest
{
    [BindFrom("amount")]
    public int AmountToGet { get; set; }
}

public class GetForecasts : Endpoint<GetForcastsRequest, WeatherForecast[]>
{
    public override void Configure()
    {
        Get("forecasts/{amount}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetForcastsRequest r, CancellationToken c)
    {
        var list = new List<WeatherForecast>();
        for (int i = 1; i <= r.AmountToGet; i++)
        {
            list.Add(new()
            {
                Date = DateTime.UtcNow.AddDays(i),
                Summary = $"i am {i}",
                TemperatureC = i + 34
            });
        }
        await SendAsync(list.ToArray());
    }
}