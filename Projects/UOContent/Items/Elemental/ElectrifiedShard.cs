using ModernUO.Serialization;
using Server.Mobiles;

namespace Server.Items
{
    [SerializationGenerator(0, false)]
    public partial class ElectrifiedShard : BaseShard
    {
        public override double DefaultWeight
        {
            get { return 0.1; }
        }

        public override int LabelNumber { get { return 1061199; } } // electrified shard

        [Constructible]
        public ElectrifiedShard() : this(1)
        {
        }

        [Constructible]
        public ElectrifiedShard(int amount) : base()
        {
            Amount = amount;
            Light = LightType.Circle150;
            Hue = MonsterBuff.ElectrifiedHue;
        }

        public override void GetProperties(IPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1061196, "electrical"); // This shard has ~1_val~ properties
        }

        public override void AddElementalProperties(Mobile from, BaseArmor armor)
        {
            bool use = false;
            if (armor.ShardPower < 15) {
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
            }
            else
            {
                from.SendLocalizedMessage(1061200, "electrical"); //You cannot imbue the properties of this shard with this item
            }

            if (use) {
                armor.ShardPower++;
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
