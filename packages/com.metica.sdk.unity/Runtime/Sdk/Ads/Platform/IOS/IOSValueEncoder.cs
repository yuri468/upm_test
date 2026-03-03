
using Newtonsoft.Json;

public static class IOSValueEncoder
{
    public static string? ToJson(object? value)
    {
        if (value == null)
            return null;

        return JsonConvert.SerializeObject(value);
    }
}
