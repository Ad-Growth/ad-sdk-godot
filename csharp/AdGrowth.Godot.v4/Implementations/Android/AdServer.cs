using System;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;
using Godot;
using Godot.Collections;


public partial class AndroidAdServer : IAdServerObject
{
    const string PLUGIN_NAME = "AdServerPlugin";
    private GodotObject _javaAdServer = Engine.GetSingleton(PLUGIN_NAME);

    public AndroidAdServer() { }

    public void Initialize(Action OnInit, Action<SDKInitException> OnFailed)
    {
        _javaAdServer.Call("initialize");
        _javaAdServer.Connect("onInit", Callable.From(OnInit));
        _javaAdServer.Connect("onFailed", Callable.From((Dictionary exceptionMap) =>
        {
            OnFailed(new SDKInitException((string)exceptionMap["code"], (string)exceptionMap["message"]));
        }));
    }



    public IClientProfile clientProfile
    {
        get { return new AndroidClientProfile(_javaAdServer); }
    }

    public bool isInitialized
    {
        get { return (bool)_javaAdServer.Call("isInitialized"); }
    }


}
