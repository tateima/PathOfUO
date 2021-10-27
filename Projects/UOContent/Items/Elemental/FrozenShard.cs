using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class FrozenShard : BaseShard
    {
        public override double DefaultWeight
        {
            get { return 0.1; }
        }

        public override int LabelNumber { get { return 1061195; } } // frozen shard
        [Constructible]

        public FrozenShard() : base()
        {
            Light = LightType.Circle150;
            Hue = MonsterBuff.FrozenHue;
        }

        public FrozenShard(Serial serial) : base(serial)
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
            list.Add(1061196, "freezing"); // This shard has ~1_val~ properties
        }
        public override void AddElementalProperties(Mobile from, BaseArmor armor)
        {
            bool use = false;
            if (Core.AOS && armor.GetResourceAttrs().ArmorColdResist < 100)
            {
                armor.GetResourceAttrs().ArmorColdResist++;
                use = true;
            }
            if (armor.Hue != MonsterBuff.FrozenHue)
            {
                armor.Hue = MonsterBuff.FrozenHue;
                use = true;
            }
            base.CheckDelete(use);
        }
        public override void AddElementalProperties(Mobile from, BaseWeapon weapon)
        {
            bool use = false;
            if (weapon.Burning || weapon.Toxic || weapon.Electrified)
            {
                from.SendLocalizedMessage(1061200, "frozen"); //You cannot imbue the properties of this shard with this item
            }
            else if (weapon.ShardPower < 10)
            {
                weapon.Frozen = true;
                weapon.Hue = MonsterBuff.FrozenHue;
                weapon.ShardPower++;
                use = true;
            }
            base.CheckDelete(use);
        }
    }
}
