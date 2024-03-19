using AdGrowth.Interfaces;
using Godot.Collections;

internal class AndroidClientAddress : IClientAddress
{

    private readonly Godot.GodotObject _javaAdServer;

    public AndroidClientAddress(Godot.GodotObject _javaAdServer) => this._javaAdServer = _javaAdServer;

    public double latitude
    {
        get { return (double)((Dictionary)_javaAdServer.Call("getClientAddress"))["latitude"]; }
        set { _javaAdServer.Call("setClientAddressField", "latitude", value.ToString()); }
    }

    public double longitude
    {
        get { return (double)((Dictionary)_javaAdServer.Call("getClientAddress"))["longitude"]; }
        set { _javaAdServer.Call("setClientAddressField", "longitude", value.ToString()); }
    }

    public string country
    {
        get { return (string)((Dictionary)_javaAdServer.Call("getClientAddress"))["country"]; }
        set { _javaAdServer.Call("setClientAddressField", "country", value); }
    }

    public string state
    {
        get { return (string)((Dictionary)_javaAdServer.Call("getClientAddress"))["state"]; }
        set { _javaAdServer.Call("setClientAddressField", "state", value); }
    }

    public string city
    {
        get { return (string)((Dictionary)_javaAdServer.Call("getClientAddress"))["city"]; }
        set { _javaAdServer.Call("setClientAddressField", "city", value); }
    }
    public override string ToString()
    {
        var clientAddress = (Dictionary)_javaAdServer.Call("getClientAddress");

        return $"country: {clientAddress["country"]}," +
        $"\nstate: {clientAddress["state"]}," +
        $"\ncity: {clientAddress["city"]}," +
        $"\nlatitude: {clientAddress["latitude"]}," +
        $"\nlongitude: {clientAddress["longitude"]}";

    }
}