#nullable enable

using UnityEngine;

namespace Metica
{
public class MeticaInitResponse
{
    public MeticaSmartFloors? SmartFloors { get; private set; }
    public string? UserId { get; private set; }

    public MeticaInitResponse(MeticaSmartFloors? smartFloors, string? userId)
    {
        SmartFloors = smartFloors;
        UserId = userId;
    }

    public static MeticaInitResponse FromJson(string json)
    {
        return JsonUtility.FromJson<MeticaInitResponse>(json);
    }
}
}
