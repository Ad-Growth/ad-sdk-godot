
using System;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;

namespace AdGrowth
{
    public class AdServer
    {
        public static event Action OnInit;
        public static bool isInitialized
        {
            get { return AdServerFactory.GetAdServer().isInitialized; }
        }

        public static IClientProfile clientProfile
        {
            get { return AdServerFactory.GetAdServer().clientProfile; }
        }

        public static void Initialize(Action OnInit, Action<SDKInitException> OnFailed)
        {
            AdServerFactory.GetAdServer().Initialize(() => { AdServer.OnInit?.Invoke(); OnInit?.Invoke(); }, OnFailed);
        }

    }

}
