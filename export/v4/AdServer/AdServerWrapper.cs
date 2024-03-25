using AdGrowth;
using AdGrowth.Exceptions;
using AdGrowth.Interfaces;
using Godot;

public partial class AdServerWrapper : Node
{


    [Signal]
    public delegate void OnInitEventHandler();

    [Signal]
    public delegate void OnFailedEventHandler(string code, string message);

    public void Initialize()
    {
        AdServer.Initialize(() => EmitSignal(SignalName.OnInit), (exception) => EmitSignal(SignalName.OnFailed, exception.code, exception.message));
    }

    public IClientProfile clientProfile { get { return AdServer.clientProfile; } }
    public IClientAddress clientAddress { get { return AdServer.clientProfile.clientAddress; } }
    public bool isInitialized { get { return AdServer.isInitialized; } }



}
