namespace AdGrowth.Interfaces
{
    public abstract class IClientProfile
    {
        public abstract int age { get; set; }
        public abstract int minAge { get; set; }
        public abstract int maxAge { get; set; }
        public abstract Gender gender { get; set; }
        public abstract IClientAddress clientAddress { get; }

        public abstract void AddInterest(string interest);
        public abstract void RemoveInterest(string interest);
        public abstract string[] GetInterests();
        public enum Gender
        {
            ALL, MALE, FEMALE
        }
    }
}
