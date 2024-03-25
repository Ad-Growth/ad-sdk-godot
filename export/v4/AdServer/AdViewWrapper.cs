using System;
using AdGrowth;
using AdGrowth.Enums;
using Godot;

public partial class AdViewWrapper : Node
{
    private AdView _ad;

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

    public void Load(string unitId, string adSize, string adOrientation, string adPosition)
    {
        if (_ad != null) _ad.Destroy();

        _ad = new AdView(unitId,
        (AdSize)Enum.Parse(typeof(AdSize), adSize),
        (AdOrientation)Enum.Parse(typeof(AdOrientation), adOrientation),
        (AdPosition)Enum.Parse(typeof(AdPosition), adPosition)
        );

        _ad.OnLoad += (ad) => EmitSignal(SignalName.OnLoad);
        _ad.OnFailedToLoad += (exception) => EmitSignal(SignalName.OnFailedToLoad, exception.code, (string)exception.message);
        _ad.OnClicked += () => EmitSignal(SignalName.OnClicked);
        _ad.OnDismissed += () => EmitSignal(SignalName.OnDismissed);
        _ad.OnImpression += () => EmitSignal(SignalName.OnImpression);
        _ad.OnFailedToShow += (code) => EmitSignal(SignalName.OnFailedToShow, code);
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

        _ad.OnLoad += (ad) => EmitSignal(SignalName.OnLoad);
        _ad.OnFailedToLoad += (exception) => EmitSignal(SignalName.OnFailedToLoad, exception.code, (string)exception.message);
        _ad.OnClicked += () => EmitSignal(SignalName.OnClicked);
        _ad.OnDismissed += () => EmitSignal(SignalName.OnDismissed);
        _ad.OnImpression += () => EmitSignal(SignalName.OnImpression);
        _ad.OnFailedToShow += (code) => EmitSignal(SignalName.OnFailedToShow, code);
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
