using System;
using System.Collections.Generic;
using Server.Engines.Plants;
using Server.Engines.Quests;
using Server.Engines.Quests.Hag;
using Server.Engines.Quests.Matriarch;
using Server.Mobiles;
using Server.Talent;
using Server.Multis;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Items
{
    public enum BeverageType
    {
        Ale,
        Cider,
        Liquor,
        Milk,
        Wine,
        Water
    }

    public interface IHasQuantity
    {
        int Quantity { get; set; }
    }

    public interface IWaterSource : IHasQuantity
    {
    }

    // TODO: Flippable attributes

    [TypeAlias("Server.Items.BottleAle", "Server.Items.BottleLiquor", "Server.Items.BottleWine")]
    public class BeverageBottle : BaseBeverage
    {
        [Constructible]
        public BeverageBottle(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public BeverageBottle(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1042959; // a bottle of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                switch (Content)
                {
                    case BeverageType.Ale:    return 0x99F;
                    case BeverageType.Cider:  return 0x99F;
                    case BeverageType.Liquor: return 0x99B;
                    case BeverageType.Milk:   return 0x99B;
                    case BeverageType.Wine:   return 0x9C7;
                    case BeverageType.Water:  return 0x99B;
                }
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        if (CheckType("BottleAle"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Ale;
                        }
                        else if (CheckType("BottleLiquor"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Liquor;
                        }
                        else if (CheckType("BottleWine"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Wine;
                        }
                        else
                        {
                            throw new Exception("Invalid beverage type");
                        }

                        break;
                    }
            }
        }
    }

    public class BoneBroth : BaseBeverage
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public BoneBroth(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public BoneBroth(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1022456; // a jug of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x99B;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Liquor;
        }
        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                from.SendMessage("You feel a slight increase in magic resistance");
                m_Mobile = from;
                m_Mod = new DefaultSkillMod(SkillName.MagicResist, true, 2);
                m_Mobile.AddSkillMod(m_Mod);
                Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
                base.OnDoubleClick(from);
            }
        }
        public void ExpireBuff() {
            if (m_Mobile != null && m_Mod != null) {
                m_Mobile.RemoveSkillMod(m_Mod);
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Bone",
                "broth"
            );
        }
    }

    public class EnchantedMilk : BaseBeverage
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public EnchantedMilk(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public EnchantedMilk(Serial serial)
            : base(serial)
        {
            Hue = MonsterBuff.IllusionistHue;
        }

        public override int BaseLabelNumber => 1022456; // a jug of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x9F0;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Milk;
        }
        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                from.SendMessage("You feel a slight increase in alchemy.");
                m_Mobile = from;
                m_Mod = new DefaultSkillMod(SkillName.Alchemy, true, 2);
                m_Mobile.AddSkillMod(m_Mod);
                Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
                base.OnDoubleClick(from);
            }
        }
        public void ExpireBuff() {
            if (m_Mobile != null && m_Mod != null) {
                m_Mobile.RemoveSkillMod(m_Mod);
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Enchanted",
                "milk"
            );
        }
    }

    public class MageWater : BaseBeverage
    {
        [Constructible]
        public MageWater(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public MageWater(Serial serial)
            : base(serial)
        {
            Hue = 0x53;
        }

        public override int BaseLabelNumber => 1022456; // a jug of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x1F9D;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Water;
        }
        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                from.SendMessage("Your mana is magically rejuvenated.");
                int amount = AOS.Scale(from.ManaMax, 33);
                if ((from.Mana += amount) > from.ManaMax) {
                    from.Mana = from.ManaMax;
                } else {
                    from.Mana += amount;
                }
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Mage",
                "water"
            );
        }
    }
    public class OrcishWater : BaseBeverage
    {
        private Mobile m_Mobile;
        [Constructible]
        public OrcishWater(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public OrcishWater(Serial serial)
            : base(serial)
        {
            Hue = 0x3A;
        }

        public override int BaseLabelNumber => 1022456; // a jug of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x1F9D;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Water;
        }
        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                from.SendMessage("You feel a rush or orcish magic upon you!");
                from.BodyMod = (Utility.RandomBool()) ? 138 : 140;
                m_Mobile = from;
                Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            }
        }

        public void ExpireBuff() {
            if (m_Mobile != null && m_Mobile.IsBodyMod) {
                m_Mobile.BodyMod = 0;
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Orcish",
                "water"
            );
        }
    }
    public class DemonicWater : BaseBeverage
    {
        private Mobile m_Mobile;
        [Constructible]
        public DemonicWater(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public DemonicWater(Serial serial)
            : base(serial)
        {
            Hue = 0x2C;
        }

        public override int BaseLabelNumber => 1022456; // a jug of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x1F9D;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Water;
        }

        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                from.SendMessage("You feel a rush or demonic magic upon you!");
                from.BodyMod = (Utility.RandomBool()) ? 74 : 149;
                m_Mobile = from;
                Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            }
        }
        public void ExpireBuff() {
            if (m_Mobile != null && m_Mobile.IsBodyMod) {
                m_Mobile.BodyMod = 0;
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Demonic",
                "water"
            );
        }
    }
    public class SkeletalWater : BaseBeverage
    {
        private Mobile m_Mobile;
        [Constructible]
        public SkeletalWater(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public SkeletalWater(Serial serial)
            : base(serial)
        {
            Hue = 0x2C;
        }

        public override int BaseLabelNumber => 1022456; // a jug of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x1F9D;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Water;
        }
        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                from.SendMessage("You feel a rush or skeletal magic upon you!");
                from.BodyMod = Utility.RandomList(50, 56);
                m_Mobile = from;
                Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            }
        }
        public void ExpireBuff() {
            if (m_Mobile != null && m_Mobile.IsBodyMod) {
                m_Mobile.BodyMod = 0;
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Skeletal",
                "water"
            );
        }
    }
    public class ChargedSpirits : BaseBeverage
    {
        [Constructible]
        public ChargedSpirits(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public ChargedSpirits(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1042959; // a bottle of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x99B;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Liquor;
        }
        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                foreach(Mobile mobile in from.GetMobilesInRange(1)) {
                    if (mobile != from && from.CanBeHarmful(mobile)) {
                        if (Core.AOS) {
                            AOS.Damage(mobile, Utility.RandomMinMax(1,5), true, 0, 0, 0, 0, 100);
                        } else {
                            from.Damage(Utility.RandomMinMax(1,5), from);
                        }
                        mobile.BoltEffect(0);
                    }
                }
                from.SendMessage("Electricity leaves your skin!");
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Charged",
                "spirits"
            );
        }
    }

    public class HolyWater : BaseBeverage
    {
        [Constructible]
        public HolyWater(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public HolyWater(Serial serial)
            : base(serial)
        {
            Hue = 0x1C2;
        }

        public override int BaseLabelNumber => 1042959; // a bottle of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x1F9D;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Water;
        }

        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                foreach(Mobile mobile in from.GetMobilesInRange(1)) {
                    if (mobile != from && from.CanBeHarmful(mobile)) {
                        Effects.SendLocationParticles(
                            EffectItem.Create(new Point3D(mobile.X, mobile.Y, mobile.Z), mobile.Map, EffectItem.DefaultDuration),
                            0x37C4,
                            1,
                            29,
                            0x47D,
                            2,
                            9502,
                            0
                        );
                        if (BaseTalent.IsMobileType(OppositionGroup.AbyssalGroup, mobile.GetType()) || BaseTalent.IsMobileType(OppositionGroup.UndeadGroup, mobile.GetType())) {
                            mobile.Damage(Utility.RandomMinMax(2,9), from);
                        } else {
                            mobile.Heal(Utility.RandomMinMax(2,9), mobile);
                        }
                    }
                }
                from.SendMessage("You splash holy water around you!");
                Delete();
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Holy",
                "water"
            );
        }
    }

    public class FireSpirits : BaseBeverage
    {
        [Constructible]
        public FireSpirits(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public FireSpirits(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1042959; // a bottle of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x99B;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Liquor;
        }
        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                foreach(Mobile mobile in from.GetMobilesInRange(1)) {
                    if (mobile != from && from.CanBeHarmful(mobile)) {
                        if (Core.AOS) {
                            AOS.Damage(mobile, Utility.RandomMinMax(1,5), true, 0, 100, 0, 0, 0);
                        } else {
                            mobile.Damage(Utility.RandomMinMax(1,5), from);
                        }
                        mobile.FixedParticles(0x36BD, 20, 10, 5044, 0, 0, EffectLayer.Head, 0);
                        mobile.PlaySound(0x307);
                    }
                }
                from.SendMessage("You burp out fire!");
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Fire",
                "spirits"
            );
        }
    }

    public class BlackSambucca : BaseBeverage
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public BlackSambucca(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public BlackSambucca(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1042959; // a bottle of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x99B;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Liquor;
        }

        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                from.SendMessage("You feel a slight increase in tactics");
                m_Mobile = from;
                m_Mod = new DefaultSkillMod(SkillName.Tactics, true, 2);
                m_Mobile.AddSkillMod(m_Mod);
                Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            }
        }

        public void ExpireBuff() {
            if (m_Mobile != null && m_Mod != null) {
                m_Mobile.RemoveSkillMod(m_Mod);
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Black",
                "Sambucca"
            );
        }
    }

    public class SoldiersBrew : BaseBeverage
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public SoldiersBrew(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public SoldiersBrew(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1042959; // a bottle of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x99B;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Ale;
        }
        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                from.SendMessage("You feel a slight increase in parrying");
                m_Mobile = from;
                m_Mod = new DefaultSkillMod(SkillName.Parry, true, 2);
                m_Mobile.AddSkillMod(m_Mod);
                Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            }
        }
        public void ExpireBuff() {
            if (m_Mobile != null && m_Mod != null) {
                m_Mobile.RemoveSkillMod(m_Mod);
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Soldier's",
                "brew"
            );
        }
    }

    public class NinjasBrew : BaseBeverage
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod_Hiding;
        private SkillMod m_Mod_Stealth;
        [Constructible]
        public NinjasBrew(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public NinjasBrew(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1042959; // a bottle of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x99B;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Ale;
        }
        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                from.SendMessage("You feel a slight increase in stealth");
                m_Mobile = from;
                m_Mod_Stealth = new DefaultSkillMod(SkillName.Stealth, true, 2);
                m_Mod_Hiding = new DefaultSkillMod(SkillName.Hiding, true, 2);
                m_Mobile.AddSkillMod(m_Mod_Stealth);
                m_Mobile.AddSkillMod(m_Mod_Hiding);
                Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            }
        }
        public void ExpireBuff() {
            if (m_Mobile != null) {
                if (m_Mod_Hiding != null) {
                    m_Mobile.RemoveSkillMod(m_Mod_Hiding);
                }
                if (m_Mod_Stealth != null) {
                    m_Mobile.RemoveSkillMod(m_Mod_Stealth);
                }
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Ninja's",
                "brew"
            );
        }
    }

    public class HealersBrew : BaseBeverage
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod_Anatomy;
        private SkillMod m_Mod_Healing;
        [Constructible]
        public HealersBrew(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public HealersBrew(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1042959; // a bottle of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x99B;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Ale;
        }
        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                from.SendMessage("You feel a slight increase in healing");
                m_Mobile = from;
                m_Mod_Anatomy = new DefaultSkillMod(SkillName.Anatomy, true, 2);
                m_Mod_Healing = new DefaultSkillMod(SkillName.Healing, true, 2);
                m_Mobile.AddSkillMod(m_Mod_Anatomy);
                m_Mobile.AddSkillMod(m_Mod_Healing);
                Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            }
        }
        public void ExpireBuff() {
            if (m_Mobile != null) {
                if (m_Mod_Anatomy != null) {
                    m_Mobile.RemoveSkillMod(m_Mod_Anatomy);
                }
                if (m_Mod_Healing != null) {
                    m_Mobile.RemoveSkillMod(m_Mod_Healing);
                }
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Healer's",
                "brew"
            );
        }
    }

    public class AncestralBrew : BaseBeverage
    {
        [Constructible]
        public AncestralBrew(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public AncestralBrew(Serial serial)
            : base(serial)
        {
            Hue = 0x1B1;
        }

        public override int BaseLabelNumber => 1042959; // a bottle of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x99B;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Ale;
        }
        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ && from is PlayerMobile player) {
                from.SendMessage("You feel your planar exhaustion lapse.");
                player.NextPlanarTravel = Core.Now;
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Ancestral",
                "brew"
            );
        }
    }

    public class PiratesBrew : BaseBeverage
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public PiratesBrew(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public PiratesBrew(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1042959; // a bottle of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x99B;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Ale;
        }

        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                from.SendMessage("You feel a slight increase in fishing");
                m_Mobile = from;
                m_Mod = new DefaultSkillMod(SkillName.Fishing, true, 2);
                m_Mobile.AddSkillMod(m_Mod);
                Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            }
        }
        public void ExpireBuff() {
            if (m_Mobile != null && m_Mod != null) {
                m_Mobile.RemoveSkillMod(m_Mod);
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Pirate's",
                "brew"
            );
        }
    }

    public class Guinness : BaseBeverage
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public Guinness(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public Guinness(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1042959; // a bottle of Ale
        public override int MaxQuantity => 5;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x99B;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            Quantity = MaxQuantity;
            Content = BeverageType.Ale;
        }
        public override void Pour_OnTarget(Mobile from, object targ)
        {
            base.Pour_OnTarget(from, targ);
            if (from == targ)
            {
                from.SendMessage("You feel a slight increase in wrestling.");
                m_Mobile = from;
                m_Mod = new DefaultSkillMod(SkillName.Wrestling, true, 2);
                m_Mobile.AddSkillMod(m_Mod);
                Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            }
        }
        public void ExpireBuff() {
            if (m_Mobile != null && m_Mod != null) {
                m_Mobile.RemoveSkillMod(m_Mod);
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0}",
                "Guiness"
            );
        }
    }

    public class Jug : BaseBeverage
    {
        [Constructible]
        public Jug(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public Jug(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1042965; // a jug of Ale
        public override int MaxQuantity => 10;
        public override bool Fillable => false;

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                return 0x9C8;
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class CeramicMug : BaseBeverage
    {
        [Constructible]
        public CeramicMug() => Weight = 1.0;

        [Constructible]
        public CeramicMug(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public CeramicMug(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1042982; // a ceramic mug of Ale
        public override int MaxQuantity => 1;

        public override int ComputeItemID()
        {
            if (ItemID >= 0x995 && ItemID <= 0x999)
            {
                return ItemID;
            }

            if (ItemID == 0x9CA)
            {
                return ItemID;
            }

            return 0x995;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class PewterMug : BaseBeverage
    {
        [Constructible]
        public PewterMug() => Weight = 1.0;

        [Constructible]
        public PewterMug(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public PewterMug(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1042994; // a pewter mug with Ale
        public override int MaxQuantity => 1;

        public override int ComputeItemID()
        {
            if (ItemID >= 0xFFF && ItemID <= 0x1002)
            {
                return ItemID;
            }

            return 0xFFF;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class Goblet : BaseBeverage
    {
        [Constructible]
        public Goblet() => Weight = 1.0;

        [Constructible]
        public Goblet(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public Goblet(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1043000; // a goblet of Ale
        public override int MaxQuantity => 1;

        public override int ComputeItemID()
        {
            if (ItemID == 0x99A || ItemID == 0x9B3 || ItemID == 0x9BF || ItemID == 0x9CB)
            {
                return ItemID;
            }

            return 0x99A;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    [TypeAlias(
        "Server.Items.MugAle",
        "Server.Items.GlassCider",
        "Server.Items.GlassLiquor",
        "Server.Items.GlassMilk",
        "Server.Items.GlassWine",
        "Server.Items.GlassWater"
    )]
    public class GlassMug : BaseBeverage
    {
        [Constructible]
        public GlassMug() => Weight = 1.0;

        [Constructible]
        public GlassMug(BeverageType type)
            : base(type) =>
            Weight = 1.0;

        public GlassMug(Serial serial)
            : base(serial)
        {
        }

        public override int EmptyLabelNumber => 1022456; // mug
        public override int BaseLabelNumber => 1042976;  // a mug of Ale
        public override int MaxQuantity => 5;

        public override int ComputeItemID()
        {
            if (IsEmpty)
            {
                return ItemID >= 0x1F81 && ItemID <= 0x1F84 ? ItemID : 0x1F81;
            }

            return Content switch
            {
                BeverageType.Ale    => ItemID == 0x9EF ? 0x9EF : 0x9EE,
                BeverageType.Cider  => ItemID >= 0x1F7D && ItemID <= 0x1F80 ? ItemID : 0x1F7D,
                BeverageType.Liquor => ItemID >= 0x1F85 && ItemID <= 0x1F88 ? ItemID : 0x1F85,
                BeverageType.Milk   => ItemID >= 0x1F89 && ItemID <= 0x1F8C ? ItemID : 0x1F89,
                BeverageType.Wine   => ItemID >= 0x1F8D && ItemID <= 0x1F90 ? ItemID : 0x1F8D,
                BeverageType.Water  => ItemID >= 0x1F91 && ItemID <= 0x1F94 ? ItemID : 0x1F91,
                _                   => 0
            };
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        if (CheckType("MugAle"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Ale;
                        }
                        else if (CheckType("GlassCider"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Cider;
                        }
                        else if (CheckType("GlassLiquor"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Liquor;
                        }
                        else if (CheckType("GlassMilk"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Milk;
                        }
                        else if (CheckType("GlassWine"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Wine;
                        }
                        else if (CheckType("GlassWater"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Water;
                        }
                        else
                        {
                            throw new Exception("Invalid beverage type");
                        }

                        break;
                    }
            }
        }
    }

    [TypeAlias(
        "Server.Items.PitcherAle",
        "Server.Items.PitcherCider",
        "Server.Items.PitcherLiquor",
        "Server.Items.PitcherMilk",
        "Server.Items.PitcherWine",
        "Server.Items.PitcherWater",
        "Server.Items.GlassPitcher"
    )]
    public class Pitcher : BaseBeverage
    {
        [Constructible]
        public Pitcher() => Weight = 2.0;

        [Constructible]
        public Pitcher(BeverageType type)
            : base(type) =>
            Weight = 2.0;

        public Pitcher(Serial serial)
            : base(serial)
        {
        }

        public override int BaseLabelNumber => 1048128; // a Pitcher of Ale
        public override int MaxQuantity => 5;

        public override int ComputeItemID()
        {
            if (IsEmpty)
            {
                if (ItemID == 0x9A7 || ItemID == 0xFF7)
                {
                    return ItemID;
                }

                return 0xFF6;
            }

            switch (Content)
            {
                case BeverageType.Ale:
                    {
                        if (ItemID == 0x1F96)
                        {
                            return ItemID;
                        }

                        return 0x1F95;
                    }
                case BeverageType.Cider:
                    {
                        if (ItemID == 0x1F98)
                        {
                            return ItemID;
                        }

                        return 0x1F97;
                    }
                case BeverageType.Liquor:
                    {
                        if (ItemID == 0x1F9A)
                        {
                            return ItemID;
                        }

                        return 0x1F99;
                    }
                case BeverageType.Milk:
                    {
                        if (ItemID == 0x9AD)
                        {
                            return ItemID;
                        }

                        return 0x9F0;
                    }
                case BeverageType.Wine:
                    {
                        if (ItemID == 0x1F9C)
                        {
                            return ItemID;
                        }

                        return 0x1F9B;
                    }
                case BeverageType.Water:
                    {
                        if (ItemID == 0xFF8 || ItemID == 0xFF9 || ItemID == 0x1F9E)
                        {
                            return ItemID;
                        }

                        return 0x1F9D;
                    }
            }

            return 0;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            if (CheckType("PitcherWater") || CheckType("GlassPitcher"))
            {
                InternalDeserialize(reader, false);
            }
            else
            {
                InternalDeserialize(reader, true);
            }

            var version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        if (CheckType("PitcherAle"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Ale;
                        }
                        else if (CheckType("PitcherCider"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Cider;
                        }
                        else if (CheckType("PitcherLiquor"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Liquor;
                        }
                        else if (CheckType("PitcherMilk"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Milk;
                        }
                        else if (CheckType("PitcherWine"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Wine;
                        }
                        else if (CheckType("PitcherWater"))
                        {
                            Quantity = MaxQuantity;
                            Content = BeverageType.Water;
                        }
                        else if (CheckType("GlassPitcher"))
                        {
                            Quantity = 0;
                            Content = BeverageType.Water;
                        }
                        else
                        {
                            throw new Exception("Invalid beverage type");
                        }

                        break;
                    }
            }
        }
    }

    public abstract class BaseBeverage : Item, IHasQuantity
    {
        private static readonly int[] m_SwampTiles =
        {
            0x9C4, 0x9EB,
            0x3D65, 0x3D65,
            0x3DC0, 0x3DD9,
            0x3DDB, 0x3DDC,
            0x3DDE, 0x3EF0,
            0x3FF6, 0x3FF6,
            0x3FFC, 0x3FFE
        };

        private static readonly Dictionary<Mobile, Timer> m_Table = new();

        private BeverageType m_Content;
        private int m_Quantity;

        public BaseBeverage() => ItemID = ComputeItemID();

        public BaseBeverage(BeverageType type)
        {
            m_Content = type;
            m_Quantity = MaxQuantity;
            ItemID = ComputeItemID();
        }

        public BaseBeverage(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                var num = BaseLabelNumber;

                if (IsEmpty || num == 0)
                {
                    return EmptyLabelNumber;
                }

                return BaseLabelNumber + (int)m_Content;
            }
        }

        public virtual bool ShowQuantity => MaxQuantity > 1;
        public virtual bool Fillable => true;
        public virtual bool Pourable => true;

        public virtual int EmptyLabelNumber => base.LabelNumber;
        public virtual int BaseLabelNumber => 0;

        public abstract int MaxQuantity { get; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsEmpty => m_Quantity <= 0;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ContainsAlchohol => !IsEmpty && m_Content != BeverageType.Milk && m_Content != BeverageType.Water;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsFull => m_Quantity >= MaxQuantity;

        [CommandProperty(AccessLevel.GameMaster)]
        public Poison Poison { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Poisoner { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public BeverageType Content
        {
            get => m_Content;
            set
            {
                m_Content = value;

                InvalidateProperties();

                var itemID = ComputeItemID();

                if (itemID > 0)
                {
                    ItemID = itemID;
                }
                else
                {
                    Delete();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Quantity
        {
            get => m_Quantity;
            set
            {
                m_Quantity = Math.Clamp(value, 0, MaxQuantity);

                InvalidateProperties();

                var itemID = ComputeItemID();

                if (itemID > 0)
                {
                    ItemID = itemID;
                }
                else
                {
                    Delete();
                }
            }
        }

        public abstract int ComputeItemID();

        public virtual int GetQuantityDescription()
        {
            var perc = m_Quantity * 100 / MaxQuantity;

            if (perc <= 0)
            {
                return 1042975; // It's empty.
            }

            if (perc <= 33)
            {
                return 1042974; // It's nearly empty.
            }

            if (perc <= 66)
            {
                return 1042973; // It's half full.
            }

            return 1042972;     // It's full.
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (ShowQuantity)
            {
                list.Add(GetQuantityDescription());
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            if (ShowQuantity)
            {
                LabelTo(from, GetQuantityDescription());
            }
        }

        public virtual bool ValidateUse(Mobile from, bool message)
        {
            if (Deleted)
            {
                return false;
            }

            if (!Movable && !Fillable)
            {
                var house = BaseHouse.FindHouseAt(this);

                if (house?.HasLockedDownItem(this) != true)
                {
                    if (message)
                    {
                        from.SendLocalizedMessage(502946, "", 0x59); // That belongs to someone else.
                    }

                    return false;
                }
            }

            if (from.Map != Map || !from.InRange(GetWorldLocation(), 2) || !from.InLOS(this))
            {
                if (message)
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                }

                return false;
            }

            return true;
        }

        public virtual void Fill_OnTarget(Mobile from, object targ)
        {
            if (!IsEmpty || !Fillable || !ValidateUse(from, false))
            {
                return;
            }

            if (targ is BaseBeverage bev)
            {
                if (bev.IsEmpty || !bev.ValidateUse(from, true))
                {
                    return;
                }

                Content = bev.Content;
                Poison = bev.Poison;
                Poisoner = bev.Poisoner;

                if (bev.Quantity > MaxQuantity)
                {
                    Quantity = MaxQuantity;
                    bev.Quantity -= MaxQuantity;
                }
                else
                {
                    Quantity += bev.Quantity;
                    bev.Quantity = 0;
                }
            }
            else if (targ is BaseWaterContainer bwc)
            {
                if (Quantity == 0 || Content == BeverageType.Water && !IsFull)
                {
                    var iNeed = Math.Min(MaxQuantity - Quantity, bwc.Quantity);

                    if (iNeed > 0 && !bwc.IsEmpty && !IsFull)
                    {
                        bwc.Quantity -= iNeed;
                        Quantity += iNeed;
                        Content = BeverageType.Water;

                        from.PlaySound(0x4E);
                    }
                }
            }
            else if (targ is Item item)
            {
                var src = item as IWaterSource;

                if (src == null && item is AddonComponent component)
                {
                    src = component.Addon as IWaterSource;
                }

                if (src == null || src.Quantity <= 0)
                {
                    return;
                }

                if (from.Map != item.Map || !from.InRange(item.GetWorldLocation(), 2) || !from.InLOS(item))
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                    return;
                }

                Content = BeverageType.Water;
                Poison = null;
                Poisoner = null;

                if (src.Quantity > MaxQuantity)
                {
                    Quantity = MaxQuantity;
                    src.Quantity -= MaxQuantity;
                }
                else
                {
                    Quantity += src.Quantity;
                    src.Quantity = 0;
                }

                from.SendLocalizedMessage(1010089); // You fill the container with water.
            }
            else if (targ is Cow cow)
            {
                if (cow.TryMilk(from))
                {
                    Content = BeverageType.Milk;
                    Quantity = MaxQuantity;
                    from.SendLocalizedMessage(1080197); // You fill the container with milk.
                }
            }
            else if (targ is LandTarget target)
            {
                var tileID = target.TileID;

                if (from is PlayerMobile player)
                {
                    var qs = player.Quest;

                    if (!(qs is WitchApprenticeQuest))
                    {
                        return;
                    }

                    var obj = qs.FindObjective<FindIngredientObjective>();

                    if (obj?.Completed == true && obj.Ingredient == Ingredient.SwampWater)
                    {
                        var contains = false;

                        for (var i = 0; !contains && i < m_SwampTiles.Length; i += 2)
                        {
                            contains = tileID >= m_SwampTiles[i] && tileID <= m_SwampTiles[i + 1];
                        }

                        if (contains)
                        {
                            Delete();

                            player.SendLocalizedMessage(
                                1055035
                            ); // You dip the container into the disgusting swamp water, collecting enough for the Hag's vile stew.
                            obj.Complete();
                        }
                    }
                }
            }
        }

        public virtual void Pour_OnTarget(Mobile from, object targ)
        {
            if (IsEmpty || !Pourable || !ValidateUse(from, false))
            {
                return;
            }

            if (targ is BaseBeverage bev)
            {
                if (!bev.ValidateUse(from, true))
                {
                    return;
                }

                if (bev.IsFull && bev.Content == Content)
                {
                    from.SendLocalizedMessage(500848); // Couldn't pour it there.  It was already full.
                }
                else if (!bev.IsEmpty)
                {
                    from.SendLocalizedMessage(500846); // Can't pour it there.
                }
                else
                {
                    bev.Content = Content;
                    bev.Poison = Poison;
                    bev.Poisoner = Poisoner;

                    if (Quantity > bev.MaxQuantity)
                    {
                        bev.Quantity = bev.MaxQuantity;
                        Quantity -= bev.MaxQuantity;
                    }
                    else
                    {
                        bev.Quantity += Quantity;
                        Quantity = 0;
                    }

                    from.PlaySound(0x4E);
                }
            }
            else if (from == targ)
            {
                BaseTalent optimisedConsumption = null;
                if (from is PlayerMobile player)
                {
                    optimisedConsumption = player.GetTalent(typeof(OptimisedConsumption));
                }
                if (from.Thirst < 20)
                {
                    from.Thirst += 1;
                }

                if (ContainsAlchohol)
                {
                    var bac = Content switch
                    {
                        BeverageType.Ale    => 1,
                        BeverageType.Wine   => 2,
                        BeverageType.Cider  => 3,
                        BeverageType.Liquor => 4,
                        _                   => 0
                    };

                    from.BAC = Math.Min(from.BAC + bac, 60);
                    CheckHeaveTimer(from);
                    if (optimisedConsumption != null)
                    {
                        optimisedConsumption.UpdateMobile(from);
                    }
                }

                from.PlaySound(Utility.RandomList(0x30, 0x2D6));

                if (Poison != null)
                {
                    from.ApplyPoison(Poisoner, Poison);
                }
                else
                {
                    // if they have optimised consumption return some mana
                    if (optimisedConsumption != null)
                    {
                        if (!ContainsAlchohol && from.Mana < from.ManaMax)
                        {
                            from.Mana += optimisedConsumption.Level;
                        }
                    }
                }

                --Quantity;
            }
            else if (targ is BaseWaterContainer bwc)
            {
                if (Content != BeverageType.Water)
                {
                    from.SendLocalizedMessage(500842); // Can't pour that in there.
                }
                else if (bwc.Items.Count != 0)
                {
                    from.SendLocalizedMessage(500841); // That has something in it.
                }
                else
                {
                    var itNeeds = Math.Min(bwc.MaxQuantity - bwc.Quantity, Quantity);

                    if (itNeeds > 0)
                    {
                        bwc.Quantity += itNeeds;
                        Quantity -= itNeeds;

                        from.PlaySound(0x4E);
                    }
                }
            }
            else if (targ is PlantItem item)
            {
                item.Pour(from, this);
            }
            else if (targ is AddonComponent component &&
                     (component.Addon is WaterVatEast || component.Addon is WaterVatSouth) &&
                     Content == BeverageType.Water)
            {
                if (from is PlayerMobile player)
                {
                    if (player.Quest is SolenMatriarchQuest qs)
                    {
                        QuestObjective obj = qs.FindObjective<GatherWaterObjective>();

                        if (obj?.Completed == false)
                        {
                            var vat = component.Addon;

                            if (vat.X > 5784 && vat.X < 5814 && vat.Y > 1903 && vat.Y < 1934 &&
                                (qs.RedSolen && vat.Map == Map.Trammel || !qs.RedSolen && vat.Map == Map.Felucca))
                            {
                                if (obj.CurProgress + Quantity > obj.MaxProgress)
                                {
                                    var delta = obj.MaxProgress - obj.CurProgress;

                                    Quantity -= delta;
                                    obj.CurProgress = obj.MaxProgress;
                                }
                                else
                                {
                                    obj.CurProgress += Quantity;
                                    Quantity = 0;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(500846); // Can't pour it there.
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsEmpty)
            {
                if (!Fillable || !ValidateUse(from, true))
                {
                    return;
                }

                from.BeginTarget(-1, true, TargetFlags.None, Fill_OnTarget);
                SendLocalizedMessageTo(from, 500837); // Fill from what?
            }
            else if (Pourable && ValidateUse(from, true))
            {
                from.BeginTarget(-1, true, TargetFlags.None, Pour_OnTarget);
                from.SendLocalizedMessage(1010086); // What do you want to use this on?
            }
        }

        public static bool ConsumeTotal(Container pack, BeverageType content, int quantity) =>
            ConsumeTotal(pack, typeof(BaseBeverage), content, quantity);

        public static bool ConsumeTotal(Container pack, Type itemType, BeverageType content, int quantity)
        {
            var items = pack.FindItemsByType(itemType);

            // First pass, compute total
            var total = 0;

            for (var i = 0; i < items.Length; ++i)
            {
                if (items[i] is BaseBeverage bev && bev.Content == content && !bev.IsEmpty)
                {
                    total += bev.Quantity;
                }
            }

            if (total >= quantity)
            {
                // We've enough, so consume it

                var need = quantity;

                for (var i = 0; i < items.Length; ++i)
                {
                    if (!(items[i] is BaseBeverage bev) || bev.Content != content || bev.IsEmpty)
                    {
                        continue;
                    }

                    var theirQuantity = bev.Quantity;

                    if (theirQuantity < need)
                    {
                        bev.Quantity = 0;
                        need -= theirQuantity;
                    }
                    else
                    {
                        bev.Quantity -= need;
                        return true;
                    }
                }
            }

            return false;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            writer.Write(Poisoner);

            Poison.Serialize(Poison, writer);
            writer.Write((int)m_Content);
            writer.Write(m_Quantity);
        }

        protected bool CheckType(string name) => GetType().FullName == $"Server.Items.{name}";

        public override void Deserialize(IGenericReader reader)
        {
            InternalDeserialize(reader, true);
        }

        protected void InternalDeserialize(IGenericReader reader, bool read)
        {
            base.Deserialize(reader);

            if (!read)
            {
                return;
            }

            var version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        Poisoner = reader.ReadEntity<Mobile>();
                        goto case 0;
                    }
                case 0:
                    {
                        Poison = Poison.Deserialize(reader);
                        m_Content = (BeverageType)reader.ReadInt();
                        m_Quantity = reader.ReadInt();
                        break;
                    }
            }
        }

        public static void Initialize()
        {
            EventSink.Login += EventSink_Login;
        }

        private static void EventSink_Login(Mobile m)
        {
            CheckHeaveTimer(m);
        }

        public static void CheckHeaveTimer(Mobile from)
        {
            if (from.BAC > 0 && from.Map != Map.Internal && !from.Deleted)
            {
                if (m_Table.ContainsKey(from))
                {
                    return;
                }

                if (from.BAC > 60)
                {
                    from.BAC = 60;
                }

                Timer t = new HeaveTimer(from);
                t.Start();

                m_Table[from] = t;
            }
            else if (m_Table.Remove(from, out var t))
            {
                t.Stop();
                from.SendLocalizedMessage(500850); // You feel sober.
            }
        }

        private class HeaveTimer : Timer
        {
            private readonly Mobile m_Drunk;

            public HeaveTimer(Mobile drunk)
                : base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(5.0))
            {
                m_Drunk = drunk;
            }

            protected override void OnTick()
            {
                if (m_Drunk.Deleted || m_Drunk.Map == Map.Internal)
                {
                    Stop();
                    m_Table.Remove(m_Drunk);
                }
                else if (m_Drunk.Alive)
                {
                    if (m_Drunk.BAC > 60)
                    {
                        m_Drunk.BAC = 60;
                    }

                    // chance to get sober
                    if (Utility.Random(100) < 10)
                    {
                        --m_Drunk.BAC;
                    }

                    // lose some stats
                    m_Drunk.Stam -= 1;
                    m_Drunk.Mana -= 1;

                    if (Utility.Random(1, 4) == 1)
                    {
                        if (!m_Drunk.Mounted)
                        {
                            // turn in a random direction
                            m_Drunk.Direction = (Direction)Utility.Random(8);

                            // heave
                            m_Drunk.Animate(32, 5, 1, true, false, 0);
                        }

                        // *hic*
                        m_Drunk.PublicOverheadMessage(MessageType.Regular, 0x3B2, 500849);
                    }

                    if (m_Drunk.BAC <= 0)
                    {
                        Stop();
                        m_Table.Remove(m_Drunk);

                        m_Drunk.SendLocalizedMessage(500850); // You feel sober.
                    }
                }
            }
        }
    }
}
