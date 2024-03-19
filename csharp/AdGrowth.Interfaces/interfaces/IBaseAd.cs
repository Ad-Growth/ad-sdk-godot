using System;
using AdGrowth.Entities;
using AdGrowth.Exceptions;

namespace AdGrowth.Interfaces
{
    public interface IBaseAd<T>
    {
        event Action<AdRequestException> OnFailedToLoad;
        event Action OnClicked;
        event Action<string> OnFailedToShow;
        event Action OnImpression;
        event Action OnDismissed;

        bool IsLoaded();
        bool IsFailed();

    }
}
