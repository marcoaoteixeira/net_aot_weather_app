using System.Text.Json.Serialization;

namespace Nameless.Weather.App {
    [JsonSerializable(typeof(WeatherForecast[]))]
    public partial class CustomJsonSerializerContext : JsonSerializerContext {
    }
}
