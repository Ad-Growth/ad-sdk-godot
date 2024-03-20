using System;
using System.Diagnostics;
using AdGrowth.Enums;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;
using Godot;
using Godot.Collections;

public partial class AndroidAdView : IAdView
{
    const string PLUGIN_NAME = "AdViewPlugin";
    private GodotObject _javaAdView = Engine.GetSingleton(PLUGIN_NAME);

    private string _instanceId;

    public event Action<IAdView>? OnLoad;
    public event Action<AdRequestException>? OnFailedToLoad;
    public event Action? OnClicked;
    public event Action<string>? OnFailedToShow;
    public event Action? OnImpression;
    public event Action? OnDismissed;

    public AndroidAdView(string unitId, AdSize adSize, AdOrientation adOrientation, int x, int y)
    {
        _javaAdView.Connect("onLoad", Callable.From(GetOnLoad()));
        _javaAdView.Connect("onFailedToLoad", Callable.From(GetOnFailedToLoad()));
        _javaAdView.Connect("onClicked", Callable.From(GetOnClicked()));
        _javaAdView.Connect("onDismissed", Callable.From(GetOnDismissed()));
        _javaAdView.Connect("onFailedToShow", Callable.From(GetOnFailedToShow()));
        _javaAdView.Connect("onImpression", Callable.From(GetOnImpression()));

        _instanceId = (string)_javaAdView.Call(
            "getInstanceXY",
            unitId,
            Enum.GetName(typeof(AdSize), adSize)!,
            Enum.GetName(typeof(AdOrientation), adOrientation)!,
            x,
            y
        );

        Debug.Print($"instanceId: {_instanceId}, x: {x}, y: {y}");
    }

    private void NewMethod()
    {

    }

    public AndroidAdView(string unitId, AdSize adSize, AdOrientation adOrientation, AdPosition adPosition)
    {
        _javaAdView.Connect("onLoad", Callable.From(GetOnLoad()));
        _javaAdView.Connect("onFailedToLoad", Callable.From(GetOnFailedToLoad()));
        _javaAdView.Connect("onClicked", Callable.From(GetOnClicked()));
        _javaAdView.Connect("onDismissed", Callable.From(GetOnDismissed()));
        _javaAdView.Connect("onFailedToShow", Callable.From(GetOnFailedToShow()));
        _javaAdView.Connect("onImpression", Callable.From(GetOnImpression()));

        _instanceId = (string)_javaAdView.Call(
            "getInstance",
            unitId,
            Enum.GetName(typeof(AdSize), adSize)!,
            Enum.GetName(typeof(AdOrientation), adOrientation)!,
            Enum.GetName(typeof(AdPosition), adPosition)!
        );


    }

    public void SetPosition(AdPosition adPosition)
    {
        _javaAdView.Call("setPosition", _instanceId, Enum.GetName(typeof(AdPosition), adPosition)!);
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

        return (bool)_javaAdView.Call("isLoaded", _instanceId!);
    }

    public bool IsFailed()
    {
        if (_instanceId == null) return true;

        return (bool)_javaAdView.Call("isFailed", _instanceId);
    }

    public void Destroy()
    {
        _javaAdView.Call("destroy");
        _javaAdView.Dispose();
    }

    public Action<string> GetOnLoad()
    {
        return (string instanceId) =>
        {
            if (_instanceId != instanceId) return;
            OnLoad?.Invoke(this);
        };
    }

    public Action<string, Dictionary> GetOnFailedToLoad()
    {
        return (string instanceId, Dictionary exceptionMap) =>
        {
            if (_instanceId != instanceId) return;
            OnFailedToLoad?.Invoke(new AdRequestException((string)exceptionMap["code"], (string)exceptionMap["message"]));
        };
    }

    public Action<string> GetOnClicked()
    {
        return (string instanceId) =>
        {
            if (_instanceId != instanceId) return;
            OnClicked?.Invoke();
        };
    }

    public Action<string> GetOnDismissed()
    {
        return (string instanceId) =>
        {

            if (_instanceId != instanceId) return;
            OnDismissed?.Invoke();
        };
    }

    public Action<string, string> GetOnFailedToShow()
    {
        return (string instanceId, string code) =>
        {

            if (_instanceId != instanceId) return;
            OnFailedToShow?.Invoke(code);
        };
    }

    public Action<string> GetOnImpression()
    {
        return (string instanceId) =>
        {
            if (_instanceId != instanceId) return;
            OnImpression?.Invoke();
        };
    }
}
