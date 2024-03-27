using System;
using System.Diagnostics;
using AdGrowth.Enums;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;
using Godot;
using Godot.Collections;

public class AndroidAdView : Node, IAdView
{
    const string PLUGIN_NAME = "AdViewPlugin";
    private Godot.Object _javaAdView = Engine.GetSingleton(PLUGIN_NAME);

    private string _instanceId;

    public event Action<IAdView> OnLoad;
    public event Action<AdRequestException> OnFailedToLoad;
    public event Action OnClicked;
    public event Action<string> OnFailedToShow;
    public event Action OnImpression;
    public event Action OnDismissed;

    public AndroidAdView(string unitId, AdSize adSize, AdOrientation adOrientation, int x, int y)
    {
        _javaAdView.Connect("onLoad", this, nameof(_OnLoad));
        _javaAdView.Connect("onFailedToLoad", this, nameof(_OnFailedToLoad));
        _javaAdView.Connect("onClicked", this, nameof(_OnClicked));
        _javaAdView.Connect("onDismissed", this, nameof(_OnDismissed));
        _javaAdView.Connect("onFailedToShow", this, nameof(_OnFailedToShow));
        _javaAdView.Connect("onImpression", this, nameof(_OnImpression));

        _instanceId = (string)_javaAdView.Call(
            "getInstanceXY",
            unitId,
            Enum.GetName(typeof(AdSize), adSize),
            Enum.GetName(typeof(AdOrientation), adOrientation),
            x,
            y
        );
    }

    public AndroidAdView(string unitId, AdSize adSize, AdOrientation adOrientation, AdPosition adPosition)
    {
        _javaAdView.Connect("onLoad", this, nameof(_OnLoad));
        _javaAdView.Connect("onFailedToLoad", this, nameof(_OnFailedToLoad));
        _javaAdView.Connect("onClicked", this, nameof(_OnClicked));
        _javaAdView.Connect("onDismissed", this, nameof(_OnDismissed));
        _javaAdView.Connect("onFailedToShow", this, nameof(_OnFailedToShow));
        _javaAdView.Connect("onImpression", this, nameof(_OnImpression));

        _instanceId = (string)_javaAdView.Call(
            "getInstance",
            unitId,
            Enum.GetName(typeof(AdSize), adSize),
            Enum.GetName(typeof(AdOrientation), adOrientation),
            Enum.GetName(typeof(AdPosition), adPosition)
        );


    }


    public void SetPosition(AdPosition adPosition)
    {
        _javaAdView.Call("setPosition", _instanceId, Enum.GetName(typeof(AdPosition), adPosition));
    }

    public void SetPosition(int x, int y)
    {
        _javaAdView.Call("setPositionXY", _instanceId, x, y);
    }

    public void EnableSafeArea(bool enable)
    {
        _javaAdView.Call("enableSafeArea", _instanceId, enable);
    }

    public void Load(Action<IAdView> OnLoad)
    {
        this.OnLoad += OnLoad;
        _javaAdView.Call("load", _instanceId);
    }

    public void Reload()
    {
        _javaAdView.Call("reload", _instanceId);
    }

    public bool IsLoaded()
    {
        if (_instanceId != null) return false;

        return (bool)_javaAdView.Call("isLoaded", _instanceId);
    }

    public bool IsFailed()
    {
        if (_instanceId == null) return true;

        return (bool)_javaAdView.Call("isFailed", _instanceId);
    }

    public void Destroy()
    {
        _javaAdView.Call("destroy");
        QueueFree();
    }

    public void _OnLoad(string instanceId)
    {
        if (_instanceId != instanceId) return;
        OnLoad?.Invoke(this);
    }

    public void _OnFailedToLoad(string instanceId, Dictionary exceptionMap)
    {

        if (_instanceId != instanceId) return;

        OnFailedToLoad?.Invoke(new AdRequestException((string)exceptionMap["code"], (string)exceptionMap["message"]));
    }

    public void _OnClicked(string instanceId)
    {
        if (_instanceId != instanceId) return;
        OnClicked?.Invoke();
    }

    public void _OnDismissed(string instanceId)
    {

        if (_instanceId != instanceId) return;
        OnDismissed?.Invoke();
    }

    public void _OnFailedToShow(string instanceId, string code)
    {

        if (_instanceId != instanceId) return;
        OnFailedToShow?.Invoke(code);
    }

    public void _OnImpression(string instanceId)
    {
        if (_instanceId != instanceId) return;
        OnImpression?.Invoke();
    }
}
