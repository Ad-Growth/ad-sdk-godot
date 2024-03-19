using System;
using AdGrowth.Enums;
using AdGrowth.Interfaces;
using AdGrowth.Implementations;


internal class AdViewFactory
{
    static IPlatform platform = new Platform();

    internal static IAdView GetAdView(string unitId, AdSize adSize, AdOrientation orientation, int x, int y)
    {
        switch (platform.OS)
        {
            case "Android":
              return new AndroidAdView(unitId, adSize, orientation, x, y);
            case "iOS":
            // TODO: return new AdViewIOS(unitId, adSize, orientation, x, y);
            default:
                throw new Exception("PLATFORM_NOT_SUPPORTED_YET");

        }
    }
    internal static IAdView GetAdView(string unitId, AdSize adSize, AdOrientation orientation, AdPosition adPosition)
    {
        switch (platform.OS)
        {
            case "Android":
              return new AndroidAdView(unitId, adSize, orientation, adPosition);
            case "iOS":
            // TODO: return new AdViewIOSunitId, adSize, orientation, adPosition);
            default:
                throw new Exception("PLATFORM_NOT_SUPPORTED_YET");

        }
    }

}
