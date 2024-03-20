using System;
using AdGrowth.Interfaces;
using AdGrowth.Implementations;

internal class AdServerFactory
{
    static IPlatform platform = new Platform();
    internal static IAdServerObject GetAdServer()
    {
        switch (platform.OS)
        {
            case "Android":
                return new AndroidAdServer();
            case "iOS":
            // TODO: return new AdServerIOS();
            default:
                throw new Exception("PLATFORM_NOT_SUPPORTED_YET");
        }
    }

}
