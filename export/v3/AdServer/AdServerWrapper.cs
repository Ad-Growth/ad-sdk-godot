using System;
using System.Globalization;
using AdGrowth;
using AdGrowth.Interfaces;
using Godot;

public class AdServerWrapper : Node
{
    public bool isInitialized { get { return AdServer.isInitialized; } }
    public int age { get => AdServer.clientProfile.age; set => AdServer.clientProfile.age = value; }
    public int minAge { get => AdServer.clientProfile.minAge; set => AdServer.clientProfile.minAge = value; }
    public int maxAge { get => AdServer.clientProfile.maxAge; set => AdServer.clientProfile.maxAge = value; }
    public string gender
    {
        get => Enum.GetName(typeof(IClientProfile.Gender), AdServer.clientProfile.gender);
        set => AdServer.clientProfile.gender = (IClientProfile.Gender)Enum.Parse(typeof(IClientProfile.Gender), value);
    }

    public string country { get => AdServer.clientProfile.clientAddress.country; set => AdServer.clientProfile.clientAddress.country = value; }
    public string state { get => AdServer.clientProfile.clientAddress.state; set => AdServer.clientProfile.clientAddress.state = value; }
    public string city { get => AdServer.clientProfile.clientAddress.city; set => AdServer.clientProfile.clientAddress.city = value; }
    public string latitude
    {
        get => AdServer.clientProfile.clientAddress.latitude.ToString().Replace(",", ".");
        set => AdServer.clientProfile.clientAddress.latitude = double.Parse(value, CultureInfo.InvariantCulture);
    }
    public string longitude
    {
        get => AdServer.clientProfile.clientAddress.longitude.ToString().Replace(",", ".");
        set => AdServer.clientProfile.clientAddress.longitude = double.Parse(value, CultureInfo.InvariantCulture);
    }


    [Signal]
    public delegate void OnInit();

    [Signal]
    public delegate void OnFailed(string code, string message);


    public void Initialize()
    {
        AdServer.Initialize(() => EmitSignal(nameof(OnInit)), (exception) => EmitSignal(nameof(OnFailed), exception.code, exception.message));
    }


    public string[] GetInterests() { return AdServer.clientProfile.GetInterests(); }

    public void AddInterest(string interest)
    {
        AdServer.clientProfile.AddInterest(interest);
    }
    public void RemoveInterest(string interest)
    {
        AdServer.clientProfile.RemoveInterest(interest);
    }

}
