// MeticaAdError.cs

#nullable enable

using System;

namespace Metica.Ads
{
  [Serializable]
  public class MeticaAdError
  {
    public string message;
    public string? adUnitId;

    public MeticaAdError(string message, string? adUnitId)
    {
      this.message = message;
      this.adUnitId = adUnitId;
    }
  }
}