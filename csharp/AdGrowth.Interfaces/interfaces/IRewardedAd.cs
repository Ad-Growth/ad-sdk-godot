using System;
using AdGrowth.Entities;

namespace AdGrowth.Interfaces
{
    public interface IRewardedAd : IBaseAd<IRewardedAd>
    {
        void Show(Action<RewardItem> OnEarnedReward);
        void Load(Action<IRewardedAd> OnLoad);

    }
}
