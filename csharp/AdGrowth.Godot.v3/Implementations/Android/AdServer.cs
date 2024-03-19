using System;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;
using Godot;
using Godot.Collections;

namespace AdGrowth.Implementations
{

    public class AndroidAdServer : Godot.Object, IAdServerObject
    {
        const string PLUGIN_NAME = "AdServerPlugin";
        private Godot.Object _javaAdServer = Engine.GetSingleton(PLUGIN_NAME);
        private event Action OnInit;
        private event Action<SDKInitException> OnFailed;
        public AndroidAdServer()
        {

        }

        public void Initialize(Action OnInit, Action<SDKInitException> OnFailed)
        {
            this.OnInit += OnInit;
            this.OnFailed += OnFailed;
            _javaAdServer.Call("initialize");
            _javaAdServer.Connect("onInit", this, nameof(_OnInit));
            _javaAdServer.Connect("onFailed", this, nameof(_OnFailed));
        }


        public IClientProfile clientProfile
        {
            get { return new AndroidClientProfile(_javaAdServer); }
        }

        public bool isInitialized
        {
            get { return (bool)_javaAdServer.Call("isInitialized"); }
        }

        public void _OnInit()
        {
            OnInit.Invoke();
        }
        public void _OnFailed(Dictionary exceptionMap)
        {
            OnFailed.Invoke(new SDKInitException((string)exceptionMap["code"], (string)exceptionMap["message"]));
        }

    }
}

