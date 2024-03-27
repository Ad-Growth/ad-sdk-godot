using System;
using System.Diagnostics;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;
using Godot;
using Godot.Collections;


public class AndroidInterstitialAd : Godot.Object, IInterstitialAd
{
    const string PLUGIN_NAME = "InterstitialAdPlugin";
    private Godot.Object _javaInterstitialAd = Engine.GetSingleton(PLUGIN_NAME);
    private readonly string _instanceId;
    private event Action<IInterstitialAd> OnLoad;
    public event Action<AdRequestException> OnFailedToLoad;
    public event Action OnClicked;
    public event Action<string> OnFailedToShow;
    public event Action OnImpression;
    public event Action OnDismissed;
    public AndroidInterstitialAd(string unitId)
    {
        _instanceId = (string)_javaInterstitialAd.Call("getInstance", unitId);

        _javaInterstitialAd.Connect("onLoad", this, nameof(_OnLoad));
        _javaInterstitialAd.Connect("onFailedToLoad", this, nameof(_OnFailedToLoad));
        _javaInterstitialAd.Connect("onClicked", this, nameof(_OnClicked));
        _javaInterstitialAd.Connect("onDismissed", this, nameof(_OnDismissed));
        _javaInterstitialAd.Connect("onFailedToShow", this, nameof(_OnFailedToShow));
        _javaInterstitialAd.Connect("onImpression", this, nameof(_OnImpression));
    }

    public void Load(Action<IInterstitialAd> OnLoad)
    {
        this.OnLoad += OnLoad;
        _javaInterstitialAd.Call("load", _instanceId);
    }

    public void Show()
    {
        _javaInterstitialAd.Call("show", _instanceId);
    }

    public bool IsLoaded()
    {
        if (_instanceId != null) return false;

        return (bool)_javaInterstitialAd.Call("isLoaded", _instanceId);
    }

    public bool IsFailed()
    {
        if (_instanceId == null) return true;

        return (bool)_javaInterstitialAd.Call("isFailed", _instanceId);
    }

    private void Destroy()
    {
        Free();
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
        Destroy();
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
