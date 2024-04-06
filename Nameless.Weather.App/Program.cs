namespace Nameless.Weather.App
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var stopwatch = Stopwatch.GetTimestamp();
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            // NOTE: If we're using AOT, unfortunately we lost SWAGGER =/
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.ConfigureHttpJsonOptions(opts =>
            {
                opts.SerializerOptions.TypeInfoResolver = new CustomJsonSerializerContext();
            });

            var app = builder.Build();

            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            lifetime.ApplicationStarted.Register(() =>
            {
                Console.WriteLine($"Total ms to start: {Stopwatch.GetElapsedTime(stopwatch).TotalMilliseconds}");
            });

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment()) {
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            var summaries = new[]
            {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        };

            app.MapGet("/weatherforecast", () =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

            app.Run();
        }
    }

    public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

}