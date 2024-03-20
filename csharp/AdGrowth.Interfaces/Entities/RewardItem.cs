
namespace AdGrowth.Entities
{
    public class RewardItem
    {
        public int value { get; }
        public string item { get; }

        public RewardItem(string item, int value)
        {
            this.item = item;
            this.value = value;
        }

        public override string ToString()
        {
            return $"RewardedItem: {value}x {item}";
        }

    }
}
