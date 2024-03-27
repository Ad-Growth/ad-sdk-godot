using AdGrowth.Interfaces;
using Godot.Collections;

internal class AndroidClientAddress : IClientAddress
{

    private readonly Godot.GodotObject _javaAdServer;

    public AndroidClientAddress(Godot.GodotObject _javaAdServer) => this._javaAdServer = _javaAdServer;

    public double latitude
    {
        get
        {
            var latitude = ((Dictionary)_javaAdServer.Call("getClientAddress"))["latitude"];

            if (latitude is float) return (double)(float)latitude;
            return (double)latitude;
        }
        set { _javaAdServer.Call("setClientAddressField", "latitude", value.ToString().Replace(",", ".")); }
    }

    public double longitude
    {
        get
        {
            var longitude = ((Dictionary)_javaAdServer.Call("getClientAddress"))["longitude"];

            if (longitude is float) return (double)(float)longitude;
            return (double)longitude;
        }
        set { _javaAdServer.Call("setClientAddressField", "longitude", value.ToString().Replace(",", ".")); }
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