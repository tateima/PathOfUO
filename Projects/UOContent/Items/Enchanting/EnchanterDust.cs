using ModernUO.Serialization;
using Server.Mobiles;

namespace Server.Items
{
    [SerializationGenerator(0, false)]
    public partial class EnchanterDust : Item
    {
        public override double DefaultWeight
        {
            get { return 0.1; }
        }
        [Constructible]
        public EnchanterDust() : base(1)
        {
        }
        public override int LabelNumber { get { return 1061202; } } // enchanting dust
        [Constructible]
        public EnchanterDust(int amount) : base(0x0F8E)
        {
            Amount = amount;
            Stackable = true;
            Light = LightType.Circle150;
            Hue = MonsterBuff.IllusionistHue;
        }

        public override void GetProperties(IPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1061203); // enchanting dust
        }
    }
}
