using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class ElectrifiedShard : BaseShard
    {
        public override double DefaultWeight
        {
            get { return 0.1; }
        }

        public override int LabelNumber { get { return 1061199; } } // electrified shard

        [Constructible]
        public ElectrifiedShard() : base()
        {
            Light = LightType.Circle150;
            Hue = MonsterBuff.ElectrifiedHue;
        }

        public ElectrifiedShard(Serial serial) : base(serial)
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
            list.Add(1061196, "electrical"); // This shard has ~1_val~ properties
        }

        public override void AddElementalProperties(Mobile from, BaseArmor armor)
        {
            bool use = false;
            if (Core.AOS && armor.GetResourceAttrs().ArmorEnergyResist < 100)
            {
                armor.GetResourceAttrs().ArmorEnergyResist++;
                use = true;
            }
            if (armor.Hue != MonsterBuff.ElectrifiedHue)
            {
                armor.Hue = MonsterBuff.ElectrifiedHue;
                use = true;
            }
            base.CheckDelete(use);
        }
        public override void AddElementalProperties(Mobile from, BaseWeapon weapon)
        {
            bool use = false;
            if (weapon.Burning || weapon.Toxic || weapon.Frozen)
            {
                from.SendLocalizedMessage(1061200, "electrical"); //You cannot imbue the properties of this shard with this item
            }
            else if (weapon.ShardPower < 10)
            {
                weapon.Electrified = true;
                weapon.Hue = MonsterBuff.ElectrifiedHue;
                weapon.ShardPower++;
                use = true;
            }
            base.CheckDelete(use);
        }
    }
}
