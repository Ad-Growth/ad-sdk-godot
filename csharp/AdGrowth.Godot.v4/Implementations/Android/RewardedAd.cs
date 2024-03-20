using System;
using AdGrowth.Entities;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;
using Godot;
using Godot.Collections;


public partial class AndroidRewardedAd : IRewardedAd
{
    const string PLUGIN_NAME = "RewardedAdPlugin";
    private GodotObject _javaRewardedAd = Engine.GetSingleton(PLUGIN_NAME);
    private readonly string _instanceId;

    private event Action<IRewardedAd>? OnLoad;
    public event Action<AdRequestException>? OnFailedToLoad;
    public event Action? OnClicked;
    public event Action<string>? OnFailedToShow;
    public event Action? OnImpression;
    public event Action? OnDismissed;
    protected event Action<RewardItem>? OnEarnedReward;

    public AndroidRewardedAd(string unitId)
    {
        _instanceId = (string)_javaRewardedAd.Call("getInstance", unitId);

        _javaRewardedAd.Connect("onLoad", Callable.From(GetOnLoad()));
        _javaRewardedAd.Connect("onFailedToLoad", Callable.From(GetOnFailedToLoad()));
        _javaRewardedAd.Connect("onClicked", Callable.From(GetOnClicked()));
        _javaRewardedAd.Connect("onDismissed", Callable.From(GetOnDismissed()));
        _javaRewardedAd.Connect("onFailedToShow", Callable.From(GetOnFailedToShow()));
        _javaRewardedAd.Connect("onImpression", Callable.From(GetOnImpression()));
        _javaRewardedAd.Connect("onEarnedReward", Callable.From(GetOnEarnedReward()));
    }

    public void Load(Action<IRewardedAd> OnLoad)
    {
        this.OnLoad += OnLoad;
        _javaRewardedAd.Call("load", _instanceId);
    }

    public void Show(Action<RewardItem> OnEarnedReward)
    {
        this.OnEarnedReward += OnEarnedReward;
        _javaRewardedAd.Call("show", _instanceId);
    }


    public bool IsLoaded()
    {
        if (_instanceId != null) return false;

        return (bool)_javaRewardedAd.Call("isLoaded", _instanceId!);
    }

    public bool IsFailed()
    {
        if (_instanceId == null) return true;

        return (bool)_javaRewardedAd.Call("isFailed", _instanceId);
    }

    private void Destroy()
    {
        _javaRewardedAd.Dispose();
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
    private Action<string, Dictionary> GetOnEarnedReward()
    {
        return (string instanceId, Dictionary rewardItemMap) =>
        {
            if (_instanceId != instanceId) return;
            OnEarnedReward?.Invoke(new RewardItem((string)rewardItemMap["item"], (int)rewardItemMap["value"]));
        };
    }

}
