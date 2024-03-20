using AdGrowth;
using Godot;

public class InterstitialAdWrapper : Node
{
    private InterstitialAd _ad;

    [Signal]
    public delegate void OnLoad();
    [Signal]
    public delegate void OnFailedToLoad(string code, string message);
    [Signal]
    public delegate void OnFailedToShow(string code);
    [Signal]
    public delegate void OnClicked();
    [Signal]
    public delegate void OnDismissed();
    [Signal]
    public delegate void OnImpression();

    public void Load(string unitId)
    {
        _ad = new InterstitialAd(unitId);

        _ad.OnFailedToLoad += (exception) => EmitSignal(nameof(OnFailedToLoad), exception.code, exception.message);
        _ad.OnClicked += () => EmitSignal(nameof(OnClicked));
        _ad.OnDismissed += () => EmitSignal(nameof(OnDismissed));
        _ad.OnImpression += () => EmitSignal(nameof(OnImpression));
        _ad.OnFailedToShow += (code) => EmitSignal(nameof(OnFailedToShow), code);
        _ad.Load((ad) => EmitSignal(nameof(OnLoad)));
    }

    public bool IsFailed() { return _ad.IsFailed(); }
    public bool IsLoaded() { return _ad.IsLoaded(); }
    public void Show() { _ad.Show(); }



}
