using AdGrowth;
using Godot;

public class RewardedAdWrapper : Node
{
    private RewardedAd _ad;

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
    [Signal]
    public delegate void OnEarnedReward(string item, int value);

    public void Load(string unitId)
    {
        _ad = new RewardedAd(unitId);

        _ad.OnFailedToLoad += (exception) => EmitSignal(nameof(OnFailedToLoad), exception.code, exception.message);
        _ad.OnClicked += () => EmitSignal(nameof(OnClicked));
        _ad.OnDismissed += () => EmitSignal(nameof(OnDismissed));
        _ad.OnImpression += () => EmitSignal(nameof(OnImpression));
        _ad.OnFailedToShow += (code) => EmitSignal(nameof(OnFailedToShow), code);
        _ad.Load((ad) => EmitSignal(nameof(OnLoad)));
    }

    public bool IsFailed() { return _ad.IsFailed(); }
    public bool IsLoaded() { return _ad.IsLoaded(); }
    public void Show() { _ad.Show((rewardItem) => EmitSignal(nameof(OnEarnedReward), rewardItem.item, rewardItem.value)); }



}
