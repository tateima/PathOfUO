using ModernUO.Serialization;
using Server.Mobiles;

namespace Server.Items
{
    [SerializationGenerator(0, false)]
    public partial class ToxicShard : BaseShard
    {
        public override double DefaultWeight
        {
            get { return 0.1; }
        }
        [Constructible]
        public ToxicShard() : this(1)
        {
        }
        public override int LabelNumber { get { return 1061198; } } // toxic shard
        [Constructible]
        public ToxicShard(int amount) : base()
        {
            Amount = amount;
            Light = LightType.Circle150;
            Hue = MonsterBuff.ToxicHue;
        }

        public override void GetProperties(IPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1061196, "toxic"); // This shard has ~1_val~ properties
        }
        public override void AddElementalProperties(Mobile from, BaseArmor armor)
        {
            bool use = false;
            if (armor.ShardPower < 15) {
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
