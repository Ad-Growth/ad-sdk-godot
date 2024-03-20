using System;

using AdGrowth.Entities;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;
using Godot;
using Godot.Collections;


public class AndroidRewardedAd : Node, IRewardedAd
{
    const string PLUGIN_NAME = "RewardedAdPlugin";
    private Godot.Object _javaRewardedAd = Engine.GetSingleton(PLUGIN_NAME);
    private readonly string _instanceId;

    private event Action<IRewardedAd> OnLoad;
    public event Action<AdRequestException> OnFailedToLoad;
    public event Action OnClicked;
    public event Action<string> OnFailedToShow;
    public event Action OnImpression;
    public event Action OnDismissed;
    private event Action<RewardItem> OnEarnedReward;

    public AndroidRewardedAd(string unitId)
    {
        _instanceId = (string)_javaRewardedAd.Call("getInstance", unitId);

        _javaRewardedAd.Connect("onLoad", this, nameof(_OnLoad));
        _javaRewardedAd.Connect("onFailedToLoad", this, nameof(_OnFailedToLoad));
        _javaRewardedAd.Connect("onClicked", this, nameof(_OnClicked));
        _javaRewardedAd.Connect("onDismissed", this, nameof(_OnDismissed));
        _javaRewardedAd.Connect("onFailedToShow", this, nameof(_OnFailedToShow));
        _javaRewardedAd.Connect("onImpression", this, nameof(_OnImpression));
        _javaRewardedAd.Connect("onEarnedReward", this, nameof(_OnEarnedReward));
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

        return (bool)_javaRewardedAd.Call("isLoaded", _instanceId);
    }

    public bool IsFailed()
    {
        if (_instanceId == null) return true;

        return (bool)_javaRewardedAd.Call("isFailed", _instanceId);
    }

    private void Destroy()
    {
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
    public void _OnEarnedReward(string instanceId, Dictionary rewardItemMap)
    {
        if (_instanceId != instanceId) return;

        OnEarnedReward?.Invoke(new RewardItem((string)rewardItemMap["item"], (int)rewardItemMap["value"]));
    }

}
