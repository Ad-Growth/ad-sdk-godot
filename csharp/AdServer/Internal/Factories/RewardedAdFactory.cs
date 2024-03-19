using System;
using AdGrowth.Interfaces;
using AdGrowth.Implementations;

internal class RewardedAdFactory
{
    static IPlatform platform = new Platform();
    internal static IRewardedAd GetRewardedAd(string unitId)
    {
        switch (platform.OS)
        {
            case "Android":
                return new AndroidRewardedAd(unitId);
            case "iOS":
            // TODO: return new RewardedAdIOS(unitId);
            default:
                throw new Exception("PLATFORM_NOT_SUPPORTED_YET");

        }
    }

}
