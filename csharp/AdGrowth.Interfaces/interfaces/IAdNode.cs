
using AdGrowth.Exceptions;

namespace AdGrowth.Interfaces
{
    public abstract class IAdNode
    {
        public abstract void OnLoad();
        public abstract void OnFailedToLoad(AdRequestException exception);
        public virtual void OnClicked() { }
        public virtual void OnFailedToShow(string code) { }
        public virtual void OnImpression() { }
        public virtual void OnDismissed() { }
    }
}
