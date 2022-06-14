using ModernUO.Serialization;
using Server.Mobiles;

namespace Server.Items
{
    [SerializationGenerator(0, false)]
    public partial class FrozenShard : BaseShard
    {
        public override double DefaultWeight
        {
            get { return 0.1; }
        }

        public override int LabelNumber { get { return 1061195; } } // frozen shard
        public FrozenShard() : this(1)
        {
        }

        [Constructible]
        public FrozenShard(int amount) : base()
        {
            Amount = amount;
            Light = LightType.Circle150;
            Hue = MonsterBuff.FrozenHue;
        }

        public override void GetProperties(IPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1061196, "freezing"); // This shard has ~1_val~ properties
        }
        public override void AddElementalProperties(Mobile from, BaseArmor armor)
        {
            bool use = false;
            if (armor.ShardPower < 15) {
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
            }
            else
            {
                from.SendLocalizedMessage(1061200, "frozen"); //You cannot imbue the properties of this shard with this item
            }
            if (use) {
                armor.ShardPower++;
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
