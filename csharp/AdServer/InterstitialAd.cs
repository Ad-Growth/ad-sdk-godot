
using System;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;

namespace AdGrowth
{
    public class InterstitialAd
    {
        private readonly IInterstitialAd _ad;

        public event Action<AdRequestException> OnFailedToLoad;
        public event Action OnDismissed;
        public event Action OnClicked;
        public event Action<string> OnFailedToShow;
        public event Action OnImpression;

        public InterstitialAd(string unitId)
        {
            _ad = InterstitialAdFactory.GetInterstitialAd(unitId);
            ConfigureEvents();
        }

        public void Load(Action<IInterstitialAd> OnLoad)
        {
            _ad.Load(OnLoad);
        }

        public void Show()
        {
            _ad.Show();
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
