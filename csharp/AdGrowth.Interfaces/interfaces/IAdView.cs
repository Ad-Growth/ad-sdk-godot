using System;
using AdGrowth.Enums;

namespace AdGrowth.Interfaces
{
    public interface IAdView : IBaseAd<IAdView>
    {
        event Action<IAdView> OnLoad;

        void Reload();
        void Load(Action<IAdView> OnLoad);
        void Destroy();
        void SetPosition(AdPosition adPosition);
        void SetPosition(int x, int y);
        void EnableSafeArea(bool enable);
    }
}
