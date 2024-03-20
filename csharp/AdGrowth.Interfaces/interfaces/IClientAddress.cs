namespace AdGrowth.Interfaces
{
    public interface IClientAddress
    {
        string city { get; set; }
        string state { get; set; }
        string country { get; set; }
        double latitude { get; set; }
        double longitude { get; set; }
    }
}
