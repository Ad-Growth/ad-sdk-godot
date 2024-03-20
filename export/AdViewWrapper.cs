using System;
using AdGrowth;
using AdGrowth.Enums;
using Godot;

public class AdViewWrapper : Node
{
    private AdView _ad;

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

    public void Load(string unitId, string adSize, string adOrientation, string adPosition)
    {
        if (_ad != null) _ad.Destroy();

        _ad = new AdView(unitId,
        (AdSize)Enum.Parse(typeof(AdSize), adSize),
        (AdOrientation)Enum.Parse(typeof(AdOrientation), adOrientation),
        (AdPosition)Enum.Parse(typeof(AdPosition), adPosition)
        );

        _ad.OnLoad += (ad) => EmitSignal(nameof(OnLoad));
        _ad.OnFailedToLoad += (exception) => EmitSignal(nameof(OnFailedToLoad), exception.code, exception.message);
        _ad.OnClicked += () => EmitSignal(nameof(OnClicked));
        _ad.OnDismissed += () => EmitSignal(nameof(OnDismissed));
        _ad.OnImpression += () => EmitSignal(nameof(OnImpression));
        _ad.OnFailedToShow += (code) => EmitSignal(nameof(OnFailedToShow), code);
        _ad.Load();
    }

    public void Load(string unitId, string adSize, string adOrientation, int x, int y)
    {
        if (_ad != null) _ad.Destroy();

        _ad = new AdView(unitId,
               (AdSize)Enum.Parse(typeof(AdSize), adSize),
               (AdOrientation)Enum.Parse(typeof(AdOrientation), adOrientation),
                x, y
               );

        _ad.OnLoad += (ad) => EmitSignal(nameof(OnLoad));
        _ad.OnFailedToLoad += (exception) => EmitSignal(nameof(OnFailedToLoad), exception.code, exception.message);
        _ad.OnClicked += () => EmitSignal(nameof(OnClicked));
        _ad.OnDismissed += () => EmitSignal(nameof(OnDismissed));
        _ad.OnImpression += () => EmitSignal(nameof(OnImpression));
        _ad.OnFailedToShow += (code) => EmitSignal(nameof(OnFailedToShow), code);
        _ad.Load();
    }

    public bool IsFailed()
    {
        return _ad.IsFailed();
    }

    public bool IsLoaded()
    {
        return _ad.IsLoaded();
    }

    public void Destroy()
    {
        _ad.Destroy();
    }

    public void SetPosition(int x, int y)
    {
        _ad.SetPosition(x, y);
    }

    public void SetPosition(string adPosition)
    {
        _ad.SetPosition((AdPosition)Enum.Parse(typeof(AdPosition), adPosition));
    }

}
