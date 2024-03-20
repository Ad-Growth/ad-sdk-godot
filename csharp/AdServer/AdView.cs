
using System;
using AdGrowth.Enums;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;


namespace AdGrowth
{
    public class AdView
    {
        private IAdView _ad;

        public event Action<IAdView> OnLoad;
        public event Action<AdRequestException> OnFailedToLoad;
        public event Action OnDismissed;
        public event Action OnClicked;
        public event Action<string> OnFailedToShow;
        public event Action OnImpression;

        public AdView(string unitId, AdSize adSize, AdOrientation orientation, int x, int y)
        {
            _ad = AdViewFactory.GetAdView(unitId, adSize, orientation, x, y);
            ConfigureEvents();
        }

        public AdView(string unitId, AdSize adSize, AdOrientation orientation, AdPosition adPosition)
        {
            _ad = AdViewFactory.GetAdView(unitId, adSize, orientation, adPosition);
            ConfigureEvents();
        }

        public void Load()
        {
            _ad.Load(OnLoad);
        }

        public void SetPosition(AdPosition adPosition)
        {
            _ad.SetPosition(adPosition);
        }
        public void SetPosition(int x, int y)
        {
            _ad.SetPosition(x, y);
        }

        public bool IsLoaded()
        {
            return _ad.IsLoaded();
        }

        public bool IsFailed()
        {
            return _ad.IsFailed();
        }

        public void Destroy()
        {
             _ad.Destroy();
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
