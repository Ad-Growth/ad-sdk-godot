using System;
using System.Diagnostics;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;
using Godot;
using Godot.Collections;


public partial class AndroidInterstitialAd : IInterstitialAd
{
    const string PLUGIN_NAME = "InterstitialAdPlugin";
    private Godot.GodotObject _javaInterstitialAd = Engine.GetSingleton(PLUGIN_NAME);
    private readonly string _instanceId;
    private event Action<IInterstitialAd>? OnLoad;
    public event Action<AdRequestException>? OnFailedToLoad;
    public event Action? OnClicked;
    public event Action<string>? OnFailedToShow;
    public event Action? OnImpression;
    public event Action? OnDismissed;
    public AndroidInterstitialAd(string unitId)
    {
        _instanceId = (string)_javaInterstitialAd.Call("getInstance", unitId);

        _javaInterstitialAd.Connect("onLoad", Callable.From(GetOnLoad()));
        _javaInterstitialAd.Connect("onFailedToLoad", Callable.From(GetOnFailedToLoad()));
        _javaInterstitialAd.Connect("onClicked", Callable.From(GetOnClicked()));
        _javaInterstitialAd.Connect("onDismissed", Callable.From(GetOnDismissed()));
        _javaInterstitialAd.Connect("onFailedToShow", Callable.From(GetOnFailedToShow()));
        _javaInterstitialAd.Connect("onImpression", Callable.From(GetOnImpression()));
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

        return (bool)_javaInterstitialAd.Call("isLoaded", _instanceId!);
    }

    public bool IsFailed()
    {
        if (_instanceId == null) return true;

        return (bool)_javaInterstitialAd.Call("isFailed", _instanceId);
    }

    private void Destroy()
    {
        _javaInterstitialAd.Dispose();
    }

    private Action<string> GetOnLoad()
    {
        return (string instanceId) =>
        {
            if (_instanceId != instanceId) return;
            OnLoad?.Invoke(this);
        };
    }

    private Action<string, Dictionary> GetOnFailedToLoad()
    {
        return (string instanceId, Dictionary exceptionMap) =>
        {
            if (_instanceId != instanceId) return;
            OnFailedToLoad?.Invoke(new AdRequestException((string)exceptionMap["code"], (string)exceptionMap["message"]));
        };
    }

    private Action<string> GetOnClicked()
    {
        return (string instanceId) =>
        {
            if (_instanceId != instanceId) return;
            OnClicked?.Invoke();
        };
    }

    private Action<string> GetOnDismissed()
    {
        return (string instanceId) =>
        {
            if (_instanceId != instanceId) return;
            OnDismissed?.Invoke();
            Destroy();
        };
    }

    private Action<string, string> GetOnFailedToShow()
    {
        return (string instanceId, string code) =>
        {
            if (_instanceId != instanceId) return;
            OnFailedToShow?.Invoke(code);
        };
    }

    private Action<string> GetOnImpression()
    {
        return (string instanceId) =>
        {
            if (_instanceId != instanceId) return;
            OnImpression?.Invoke();
        };
    }

}
