
using System;
using AdGrowth.Exceptions;

namespace AdGrowth.Interfaces
{
    public interface IAdServerObject
    {
        bool isInitialized { get; }
        IClientProfile clientProfile { get; }
        void Initialize(Action OnInit, Action<SDKInitException> OnFailed);

    }
}
