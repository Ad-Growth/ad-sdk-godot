
using System;
using AdGrowth.Entities;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;

namespace AdGrowth
{
    public class RewardedAd
    {
        private readonly IRewardedAd _ad;

        public event Action<AdRequestException> OnFailedToLoad;
        public event Action OnDismissed;
        public event Action OnClicked;
        public event Action<string> OnFailedToShow;
        public event Action OnImpression;

        public RewardedAd(string unitId)
        {
            _ad = RewardedAdFactory.GetRewardedAd(unitId);
            ConfigureEvents();
        }

        public void Load(Action<IRewardedAd> OnLoad)
        {
            _ad.Load(OnLoad);
        }

        public void Show(Action<RewardItem> OnEarnedReward)
        {
            _ad.Show(OnEarnedReward);
        }

        public bool IsLoaded()
        {
            return _ad.IsLoaded();
        }

        public bool IsFailed()
        {
            return _ad.IsFailed();
        }


        private void ConfigureEvents()
        {
            _ad.OnFailedToLoad += delegate (AdRequestException exception)
            {
                if (OnFailedToLoad != null)
                    OnFailedToLoad(exception);
            };

            _ad.OnDismissed += delegate
            {
                if (OnDismissed != null)
                    OnDismissed();
            };

            _ad.OnClicked += delegate
            {
                if (OnClicked != null)
                    OnClicked();
            };

            _ad.OnFailedToShow += delegate (string code)
            {
                if (OnFailedToShow != null)
                    OnFailedToShow(code);
            };

            _ad.OnImpression += delegate
            {
                if (OnImpression != null)
                    OnImpression();
            };

        }
    }

}
