using System;

namespace AdGrowth.Interfaces
{
    public interface IInterstitialAd : IBaseAd<IInterstitialAd>
    {
        void Show();
        void Load(Action<IInterstitialAd> OnLoad);
    }
}
