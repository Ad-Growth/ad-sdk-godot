using AdGrowth.Interfaces;

namespace AdGrowth.Implementations;

public class Platform : IPlatform
{
    public Platform(){
        
    }
    public string OS => Godot.OS.GetName();
}