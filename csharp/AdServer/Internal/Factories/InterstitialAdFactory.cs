using System;
using AdGrowth.Implementations;
using AdGrowth.Interfaces;

internal class InterstitialAdFactory
{
    static IPlatform platform = new Platform();
    internal static IInterstitialAd GetInterstitialAd(string unitId)
    {
        switch (platform.OS)
        {
            case "Android":
             return new AndroidInterstitialAd(unitId);
            case "iOS":
            // TODO: return new IOSInterstitialAd(unitId);
            default:
                throw new Exception("PLATFORM_NOT_SUPPORTED_YET");

        }
    }

}
