using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class BurningShard : BaseShard
    {
        public override double DefaultWeight
        {
            get { return 0.1; }
        }

        public override int LabelNumber { get { return 1061197; } } // burning shard

        [Constructible]
        public BurningShard() : this(1)
        {
        }

        [Constructible]
        public BurningShard(int amount) : base()
        {
            Amount = amount;
            Light = LightType.Circle150;
            Hue = MonsterBuff.BurningHue;
        }

        public BurningShard(Serial serial) : base(serial)
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
            list.Add(1061196, "burning"); // This shard has ~1_val~ properties
        }
        public override void AddElementalProperties(Mobile from, BaseArmor armor)
        {
            bool use = false;
            if (armor.ShardPower < 15) {
                if (Core.AOS && armor.GetResourceAttrs().ArmorFireResist < 100)
                {
                    armor.GetResourceAttrs().ArmorFireResist++;
                    use = true;
                }
                if (armor.Hue != MonsterBuff.BurningHue)
                {
                    armor.Hue = MonsterBuff.BurningHue;
                    use = true;
                }
            } else {
                from.SendLocalizedMessage(1061200, "burning"); //You cannot imbue the properties of this shard with this item
            }
            if (use) {
                armor.ShardPower++;
            }
            base.CheckDelete(use);
        }
        public override void AddElementalProperties(Mobile from, BaseWeapon weapon)
        {
            bool use = false;
            if (weapon.Electrified || weapon.Toxic || weapon.Frozen)
            {
                from.SendLocalizedMessage(1061200, "burning"); //You cannot imbue the properties of this shard with this item
            }
            else if (weapon.ShardPower < 10)
            {
                weapon.Burning = true;
                weapon.Hue = MonsterBuff.BurningHue;
                weapon.ShardPower++;
                use = true;
            }
            base.CheckDelete(use);
        }
    }
}
