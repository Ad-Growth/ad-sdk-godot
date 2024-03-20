using System;
using AdGrowth.Interfaces;
using Godot.Collections;

public class AndroidClientProfile : IClientProfile
{

    private readonly Godot.GodotObject _javaAdServer;

    public AndroidClientProfile(Godot.GodotObject _javaAdServer) => this._javaAdServer = _javaAdServer;

    public override int age
    {

        get { return (int)((Dictionary)_javaAdServer.Call("getClientProfile"))["age"]; }
        set { _javaAdServer.Call("setClientProfileField", "age", value.ToString()); }
    }

    public override int minAge
    {
        get { return (int)((Dictionary)_javaAdServer.Call("getClientProfile"))["minAge"]; }
        set { _javaAdServer.Call("setClientProfileField", "minAge", value.ToString()); }
    }

    public override int maxAge
    {
        get { return (int)((Dictionary)_javaAdServer.Call("getClientProfile"))["getMaxAge"]; }
        set { _javaAdServer.Call("setClientProfileField", "getMaxAge", value.ToString()); }
    }

    public override Gender gender
    {
        get { return (Gender)Enum.Parse(typeof(Gender), (string)((Dictionary)_javaAdServer.Call("getClientProfile"))["getMaxAge"]); }
        set { _javaAdServer.Call("setClientProfileField", "gender", Enum.GetName(typeof(Gender), value)!); }
    }
    public override IClientAddress clientAddress
    {
        get { return new AndroidClientAddress(_javaAdServer); }
    }


    public override void AddInterest(string interest)
    {
        _javaAdServer.Call("addInterest", interest);
    }

    public override void RemoveInterest(string interest)
    {
        _javaAdServer.Call("removeInterest", interest);
    }

    public override string ToString()
    {
        var clientProfile = (Dictionary)_javaAdServer.Call("getClientProfile");

        var interests = (string[])_javaAdServer.Call("getInterests");

        return $"gender: {clientProfile["gender"]}," +
        $"\nage: {clientProfile["age"]}," +
        $"\nminAge: {clientProfile["minAge"]}," +
        $"\nmaxAge: {clientProfile["maxAge"]}," +
        $"\ninterests: [{string.Join(",", interests)}]," +
        $"\nclientAddress: {clientAddress}";
    }
}