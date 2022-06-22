using ModernUO.Serialization;

namespace Server.Items.Misc
{
    [SerializationGenerator(0)]
    public partial class CityKey : Item
    {
        [Constructible]
        public CityKey() : base(0x1012)
        {
            Weight = 1.0;
            Name = "Key to the city";
        }
    }
}
