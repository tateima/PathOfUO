using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class ToxicShard : BaseShard
    {
        public override double DefaultWeight
        {
            get { return 0.1; }
        }

        public override int LabelNumber { get { return 1061198; } } // toxic shard
        [Constructible]
        public ToxicShard() : base()
        {
            Light = LightType.Circle150;
            Hue = MonsterBuff.ToxicHue;
        }

        public ToxicShard(Serial serial) : base(serial)
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
            list.Add(1061196, "toxic"); // This shard has ~1_val~ properties
        }
        public override void AddElementalProperties(Mobile from, BaseArmor armor)
        {
            bool use = false;
            if (Core.AOS && armor.GetResourceAttrs().ArmorPoisonResist < 100)
            {
                armor.GetResourceAttrs().ArmorPoisonResist++;
                use = true;
            }
            if (armor.Hue != MonsterBuff.ToxicHue)
            {
                armor.Hue = MonsterBuff.ToxicHue;
                use = true;
            }
            base.CheckDelete(use);
        }
        public override void AddElementalProperties(Mobile from, BaseWeapon weapon)
        {
            bool use = false;
            if (weapon.Electrified || weapon.Burning || weapon.Frozen)
            {
                from.SendLocalizedMessage(1061200, "toxic"); //You cannot imbue the properties of this shard with this item
            }
            else if (weapon.ShardPower < 10)
            {
                weapon.Toxic = true;
                weapon.Hue = MonsterBuff.ToxicHue;
                weapon.ShardPower++;
                use = true;
            }
            base.CheckDelete(use);
        }
    }
}
