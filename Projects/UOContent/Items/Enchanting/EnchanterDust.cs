using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class EnchanterDust : Item
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

        public EnchanterDust(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Light = LightType.Circle150;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1061203); // enchanting dust
        }
    }
}
