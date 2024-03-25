using AdGrowth;
using Godot;

public partial class InterstitialAdWrapper : Node
{
    private InterstitialAd _ad;

    [Signal]
    public delegate void OnLoadEventHandler();
    [Signal]
    public delegate void OnFailedToLoadEventHandler(string code, string message);
    [Signal]
    public delegate void OnFailedToShowEventHandler(string code);
    [Signal]
    public delegate void OnClickedEventHandler();
    [Signal]
    public delegate void OnDismissedEventHandler();
    [Signal]
    public delegate void OnImpressionEventHandler();

    public void Load(string unitId)
    {
        _ad = new InterstitialAd(unitId);

        _ad.OnFailedToLoad += (exception) => EmitSignal(SignalName.OnFailedToLoad, exception.code, (string)exception.message);
        _ad.OnClicked += () => EmitSignal(SignalName.OnClicked);
        _ad.OnDismissed += () => EmitSignal(SignalName.OnDismissed);
        _ad.OnImpression += () => EmitSignal(SignalName.OnImpression);
        _ad.OnFailedToShow += (code) => EmitSignal(SignalName.OnFailedToShow, code);
        _ad.Load((ad) => EmitSignal(SignalName.OnLoad));
    }

    public bool IsFailed() { return _ad.IsFailed(); }
    public bool IsLoaded() { return _ad.IsLoaded(); }
    public void Show() { _ad.Show(); }



}
