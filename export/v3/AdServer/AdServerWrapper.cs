using AdGrowth;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;
using Godot;

public class AdServerWrapper : Node
{


    [Signal]
    public delegate void OnInit();

    [Signal]
    public delegate void OnFailed(string code, string message);

    public void Initialize()
    {
        AdServer.Initialize(() => EmitSignal(nameof(OnInit)), (exception) => EmitSignal(nameof(OnFailed), exception.code, exception.message));
    }

    public IClientProfile clientProfile { get { return AdServer.clientProfile; } }
    public IClientAddress clientAddress { get { return AdServer.clientProfile.clientAddress; } }
    public bool isInitialized { get { return AdServer.isInitialized; } }



}
