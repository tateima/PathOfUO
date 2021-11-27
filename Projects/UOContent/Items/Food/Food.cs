using System.Collections.Generic;
using Server.ContextMenus;
using Server.Talent;
using Server.Mobiles;
using System;

namespace Server.Items
{
    public abstract class Food : Item
    {
        public Food(int itemID, int amount = 1) : base(itemID)
        {
            Stackable = true;
            Amount = amount;
            FillFactor = 1;
        }

        public Food(Serial serial) : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Poisoner { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Poison Poison { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FillFactor { get; set; }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive)
            {
                list.Add(new EatEntry(from, this));
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
            {
                return;
            }

            if (from.InRange(GetWorldLocation(), 1))
            {
                Eat(from);
            }
        }

        public virtual bool Eat(Mobile from)
        {
            // Fill the Mobile with FillFactor
            if (CheckHunger(from))
            {
                // Play a random "eat" sound
                from.PlaySound(Utility.Random(0x3A, 3));

                if (from.Body.IsHuman && !from.Mounted)
                {
                    from.Animate(34, 5, 1, true, false, 0);
                }

                if (Poison != null)
                {
                    from.ApplyPoison(Poisoner, Poison);
                } else if (from is PlayerMobile player) 
                {
                    // if they have optimised consumption heal them slightly if hurt
                    BaseTalent optimisedConsumption = player.GetTalent(typeof(OptimisedConsumption));
                    if (optimisedConsumption != null)
                    {
                        if (player.Hits < player.HitsMax)
                        {
                            player.Hits += optimisedConsumption.Level;
                        }
                        // if they are quite hungry give them a stat buff
                        if (from.Hunger < 15) {
                            from.AddStatMod(new StatMod(StatType.All, "optimisedConsumption", optimisedConsumption.Level, TimeSpan.FromMinutes(optimisedConsumption.Level * 2)));
                        }
                    }
                }

                Consume();

                return true;
            }

            return false;
        }

        public virtual bool CheckHunger(Mobile from) => FillHunger(from, FillFactor);

        public static bool FillHunger(Mobile from, int fillFactor)
        {
            if (from.Hunger >= 20)
            {
                from.SendLocalizedMessage(500867); // You are simply too full to eat any more!
                return false;
            }

            var iHunger = from.Hunger + fillFactor;

            if (from.Stam < from.StamMax)
            {
                from.Stam += Utility.Random(6, 3) + fillFactor / 5;
            }

            if (iHunger >= 20)
            {
                from.Hunger = 20;
                from.SendLocalizedMessage(500872); // You manage to eat the food, but you are stuffed!
            }
            else
            {
                from.Hunger = iHunger;

                if (iHunger < 5)
                {
                    from.SendLocalizedMessage(500868); // You eat the food, but are still extremely hungry.
                }
                else if (iHunger < 10)
                {
                    from.SendLocalizedMessage(500869); // You eat the food, and begin to feel more satiated.
                }
                else if (iHunger < 15)
                {
                    from.SendLocalizedMessage(500870); // After eating the food, you feel much less hungry.
                }
                else
                {
                    from.SendLocalizedMessage(500871); // You feel quite full after consuming the food.
                }
            }

            return true;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(4); // version

            writer.Write(Poisoner);

            Poison.Serialize(Poison, writer);
            writer.Write(FillFactor);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        Poison = reader.ReadInt() switch
                        {
                            0 => null,
                            1 => Poison.Lesser,
                            2 => Poison.Regular,
                            3 => Poison.Greater,
                            4 => Poison.Deadly,
                            _ => Poison
                        };

                        break;
                    }
                case 2:
                    {
                        Poison = Poison.Deserialize(reader);
                        break;
                    }
                case 3:
                    {
                        Poison = Poison.Deserialize(reader);
                        FillFactor = reader.ReadInt();
                        break;
                    }
                case 4:
                    {
                        Poisoner = reader.ReadEntity<Mobile>();
                        goto case 3;
                    }
            }
        }
    }

    public class BreadLoaf : Food
    {
        [Constructible]
        public BreadLoaf(int amount = 1) : base(0x103B, amount)
        {
            Weight = 1.0;
            FillFactor = 3;
        }

        public BreadLoaf(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class GarlicBread : Food
    {
        private Mobile m_Mobile;
        private ResistanceMod m_Mod;

        [Constructible]
        public GarlicBread(int amount = 1) : base(0x103B, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public GarlicBread(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in energy resistance");
            m_Mobile = from;
            m_Mod = new ResistanceMod(ResistanceType.Energy, +1);
            m_Mobile.AddResistanceMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
        }
        public void ExpireBuff() {
            if (m_Mobile != null && m_Mod != null) {
                m_Mobile.RemoveResistanceMod(m_Mod);
            }            
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "Garlic",
                "bread"
            );
        }
    }

    public class Bacon : Food
    {
        [Constructible]
        public Bacon(int amount = 1) : base(0x979, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public Bacon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class SlabOfBacon : Food
    {
        [Constructible]
        public SlabOfBacon(int amount = 1) : base(0x976, amount)
        {
            Weight = 1.0;
            FillFactor = 3;
        }

        public SlabOfBacon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class FishSteak : Food
    {
        [Constructible]
        public FishSteak(int amount = 1) : base(0x97B, amount) => FillFactor = 3;

        public FishSteak(Serial serial) : base(serial)
        {
        }

        public override double DefaultWeight => 0.1;

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class SingingFillet : Food
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public SingingFillet(int amount = 1) : base(0x97B, amount) => FillFactor = 3;

        public SingingFillet(Serial serial) : base(serial)
        {
        }

        public override double DefaultWeight => 0.1;

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in a barding skill");
            m_Mobile = from;
            switch(Utility.RandomMinMax(1, 3)) {
                case 1:
                    m_Mod = new DefaultSkillMod(SkillName.Peacemaking, true, 5);
                break;
                case 2:
                    m_Mod = new DefaultSkillMod(SkillName.Discordance, true, 5);
                break;
                case 3:
                    m_Mod = new DefaultSkillMod(SkillName.Provocation, true, 5);
                break;
            }
            m_Mobile.AddSkillMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
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
                "Singing",
                "fillet"
            );
        }
    }

    public class CheeseWheel : Food
    {
        [Constructible]
        public CheeseWheel() : this(1) 
        {            
        }
        [Constructible]
        public CheeseWheel(int amount = 1) : base(0x97E, amount) => FillFactor = 3;

        public CheeseWheel(Serial serial) : base(serial)
        {
        }

        public override double DefaultWeight => 0.1;

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class IronRichCheese : Food
    {
        private Mobile m_Mobile;
        private ResistanceMod m_Mod;

        [Constructible]
        public IronRichCheese(int amount = 1) : base(0x97E, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public IronRichCheese(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in physical resistance");
            m_Mobile = from;
            m_Mod = new ResistanceMod(ResistanceType.Physical, +1);
            m_Mobile.AddResistanceMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
        }
        public void ExpireBuff() {
            if (m_Mobile != null && m_Mod != null) {
                m_Mobile.RemoveResistanceMod(m_Mod);
            }            
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "Iron-rich",
                "cheese"
            );
        }
    }

    public class CheeseWedge : Food
    {
        [Constructible]
        public CheeseWedge(int amount = 1) : base(0x97D, amount) => FillFactor = 3;

        public CheeseWedge(Serial serial) : base(serial)
        {
        }

        public override double DefaultWeight => 0.1;

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class CheeseSlice : Food
    {
        [Constructible]
        public CheeseSlice(int amount = 1) : base(0x97C, amount) => FillFactor = 1;

        public CheeseSlice(Serial serial) : base(serial)
        {
        }

        public override double DefaultWeight => 0.1;

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class FrenchBread : Food
    {
        [Constructible]
        public FrenchBread(int amount = 1) : base(0x98C, amount)
        {
            Weight = 2.0;
            FillFactor = 3;
        }

        public FrenchBread(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class SourDough : Food
    {
        private Mobile m_Mobile;
        private ResistanceMod m_Mod;

        [Constructible]
        public SourDough(int amount = 1) : base(0x98C, amount)
        {
            Weight = 2.0;
            FillFactor = 1;
        }

        public SourDough(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
           public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in poison resistance");
            m_Mobile = from;
            m_Mod = new ResistanceMod(ResistanceType.Poison, +1);
            m_Mobile.AddResistanceMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
        }
        public void ExpireBuff() {
            if (m_Mobile != null && m_Mod != null) {
                m_Mobile.RemoveResistanceMod(m_Mod);
            }            
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "Sourdough",
                "bread"
            );
        }
    }

    public class FriedEggs : Food
    {
        [Constructible]
        public FriedEggs(int amount = 1) : base(0x9B6, amount)
        {
            Weight = 1.0;
            FillFactor = 4;
        }

        public FriedEggs(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class BraveEggs : Food
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public BraveEggs(int amount = 1) : base(0x9B6, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public BraveEggs(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in swordsman skill");
            m_Mobile = from;
            m_Mod = new DefaultSkillMod(SkillName.Swords, true, 5);
            m_Mobile.AddSkillMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
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
                "Brave",
                "eggs"
            );
        }
    }

    public class CookedBird : Food
    {
        [Constructible]
        public CookedBird(int amount = 1) : base(0x9B7, amount)
        {
            Weight = 1.0;
            FillFactor = 5;
        }

        public CookedBird(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class StickyChicken : Food
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public StickyChicken(int amount = 1) : base(0x9B7, amount)
        {
            Weight = 1.0;
            FillFactor = 5;
        }

        public StickyChicken(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in taming skill");
            m_Mobile = from;
            m_Mod = new DefaultSkillMod(SkillName.AnimalTaming, true, 5);
            m_Mobile.AddSkillMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
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
                "Sticky",
                "chicken"
            );
        }
    }

    public class RoastPig : Food
    {
        [Constructible]
        public RoastPig(int amount = 1) : base(0x9BB, amount)
        {
            Weight = 45.0;
            FillFactor = 20;
        }

        public RoastPig(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class DecoratedRoastPig : Food
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;

        [Constructible]
        public DecoratedRoastPig(int amount = 1) : base(0x9BB, amount)
        {
            Weight = 20.0;
            FillFactor = 1;
        }

        public DecoratedRoastPig(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in macing skill");
            m_Mobile = from;
            m_Mod = new DefaultSkillMod(SkillName.Macing, true, 5);
            m_Mobile.AddSkillMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
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
                "Decorated roast",
                "pig"
            );
        }
    }

    public class Sausage : Food
    {
        [Constructible]
        public Sausage(int amount = 1) : base(0x9C0, amount)
        {
            Weight = 1.0;
            FillFactor = 4;
        }

        public Sausage(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class EnchantedSausage : Food
    {

        [Constructible]
        public EnchantedSausage(int amount = 1) : base(0x9C0, amount)
        {
            Hue = 0x7D;
            Weight = 1.0;
            FillFactor = 1;
        }

        public EnchantedSausage(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }

        public override bool Eat(Mobile from) {
            
            from.SendMessage("Your hitpoints is magically rejuvenated.");    
            int hitsAmount = AOS.Scale(from.HitsMax, 33);
            if ((from.Hits += hitsAmount) > from.HitsMax) {
                from.Hits = from.HitsMax;
            } else {
                from.Hits += hitsAmount;
            }
            return base.Eat(from);
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "Enchanted",
                "sausage"
            );
        }
    }

    public class Ham : Food
    {
        [Constructible]
        public Ham(int amount = 1) : base(0x9C9, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public Ham(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class GoldenHam : Food
    {
        [Constructible]
        public GoldenHam(int amount = 1) : base(0x9C9, amount)
        {
            Hue = 0x94;
            Weight = 1.0;
            FillFactor = 1;
        }

        public GoldenHam(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            
            from.SendMessage("Your stamina is magically rejuvenated.");    
            int amount = AOS.Scale(from.StamMax, 33);
            if ((from.Stam += amount) > from.StamMax) {
                from.Stam = from.StamMax;
            } else {
                from.Stam += amount;
            }
            return base.Eat(from);
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "Golden",
                "ham"
            );
        }
    }

    public class Cake : Food
    {
        [Constructible]
        public Cake() : base(0x9E9, 1)
        {
            Stackable = false;
            Weight = 1.0;
            FillFactor = 10;
        }

        public Cake(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class MandrakeCake : Food
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public MandrakeCake() : base(0x9E9, 1)
        {
            Hue = 0x8E;
            Stackable = false;
            Weight = 1.0;
            FillFactor = 10;
        }

        public MandrakeCake(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }

        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in magery skill");
            m_Mobile = from;
            m_Mod = new DefaultSkillMod(SkillName.Magery, true, 5);
            m_Mobile.AddSkillMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
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
                "Mandrake",
                "cake"
            );
        }
    }

    public class Ribs : Food
    {
        [Constructible]
        public Ribs(int amount = 1) : base(0x9F2, amount)
        {
            Weight = 1.0;
            FillFactor = 5;
        }

        public Ribs(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class BatEncrustedRibs : Food
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public BatEncrustedRibs(int amount = 1) : base(0x9F2, amount)
        {
            Hue = 0x21E;
            Weight = 1.0;
            FillFactor = 1;
        }

        public BatEncrustedRibs(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in fencing skill");
            m_Mobile = from;
            m_Mod = new DefaultSkillMod(SkillName.Fencing, true, 5);
            m_Mobile.AddSkillMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
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
                "Bat-encrusted",
                "ribs"
            );
        }
    }

    public class Cookies : Food
    {
        [Constructible]
        public Cookies() : base(0x160b, 1)
        {
            Stackable = Core.ML;
            Weight = 1.0;
            FillFactor = 4;
        }

        public Cookies(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class Muffins : Food
    {
        [Constructible]
        public Muffins() : base(0x9eb, 1)
        {
            Stackable = false;
            Weight = 1.0;
            FillFactor = 4;
        }

        public Muffins(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    [TypeAlias("Server.Items.Pizza")]
    public class CheesePizza : Food
    {
        [Constructible]
        public CheesePizza() : base(0x1040, 1)
        {
            Stackable = false;
            Weight = 1.0;
            FillFactor = 6;
        }

        public CheesePizza(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1044516; // cheese pizza

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class SausagePizza : Food
    {
        [Constructible]
        public SausagePizza() : base(0x1040, 1)
        {
            Stackable = false;
            Weight = 1.0;
            FillFactor = 6;
        }

        public SausagePizza(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1044517; // sausage pizza

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class FruitPie : Food
    {
        [Constructible]
        public FruitPie() : base(0x1041, 1)
        {
            Stackable = false;
            Weight = 1.0;
            FillFactor = 5;
        }

        public FruitPie(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1041346; // baked fruit pie

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class MeatPie : Food
    {
        [Constructible]
        public MeatPie() : base(0x1041, 1)
        {
            Stackable = false;
            Weight = 1.0;
            FillFactor = 5;
        }

        public MeatPie(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1041347; // baked meat pie

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class AthletesPie : Food
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public AthletesPie() : base(0x1041, 1)
        {
            Stackable = false;
            Weight = 1.0;
            FillFactor = 5;
        }

        public AthletesPie(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1041347; // baked meat pie

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in anatomy skill");
            m_Mobile = from;
            m_Mod = new DefaultSkillMod(SkillName.Anatomy, true, 5);
            m_Mobile.AddSkillMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
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
                "Athletes",
                "pie"
            );
        }
    }

    public class PumpkinPie : Food
    {
        [Constructible]
        public PumpkinPie() : base(0x1041, 1)
        {
            Stackable = false;
            Weight = 1.0;
            FillFactor = 5;
        }

        public PumpkinPie(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1041348; // baked pumpkin pie

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class ApplePie : Food
    {
        [Constructible]
        public ApplePie() : base(0x1041, 1)
        {
            Stackable = false;
            Weight = 1.0;
            FillFactor = 5;
        }

        public ApplePie(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1041343; // baked apple pie

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class LemonPie : Food
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public LemonPie() : base(0x1041, 1)
        {
            Hue = 0x229;
            Stackable = false;
            Weight = 1.0;
            FillFactor = 1;
        }

        public LemonPie(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1041343; // baked apple pie

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in archery skill");
            m_Mobile = from;
            m_Mod = new DefaultSkillMod(SkillName.Archery, true, 5);
            m_Mobile.AddSkillMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
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
                "Lemon",
                "pie"
            );
        }
    }

    public class PeachCobbler : Food
    {
        [Constructible]
        public PeachCobbler() : base(0x1041, 1)
        {
            Stackable = false;
            Weight = 1.0;
            FillFactor = 5;
        }

        public PeachCobbler(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1041344; // baked peach cobbler

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class Quiche : Food
    {
        [Constructible]
        public Quiche() : base(0x1041, 1)
        {
            Stackable = Core.ML;
            Weight = 1.0;
            FillFactor = 5;
        }

        public Quiche(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1041345; // baked quiche

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class PhilosophersQuiche : Food
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public PhilosophersQuiche() : base(0x1041, 1)
        {
            Stackable = Core.ML;
            Weight = 1.0;
            FillFactor = 5;
        }

        public PhilosophersQuiche(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber => 1041345; // baked quiche

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in evaluating intelligence skill");
            m_Mobile = from;
            m_Mod = new DefaultSkillMod(SkillName.EvalInt, true, 5);
            m_Mobile.AddSkillMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
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
                "Philosophers",
                "quiche"
            );
        }
    }

    public class LambLeg : Food
    {
        [Constructible]
        public LambLeg(int amount = 1) : base(0x160a, amount)
        {
            Weight = 2.0;
            FillFactor = 5;
        }

        public LambLeg(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class SacrificialLambLeg : Food
    {
        private Mobile m_Mobile;
        private SkillMod m_Mod;
        [Constructible]
        public SacrificialLambLeg(int amount = 1) : base(0x160a, amount)
        {
            Hue = 0x215;
            Weight = 2.0;
            FillFactor = 1;
        }

        public SacrificialLambLeg(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in necromancy skill");
            m_Mobile = from;
            m_Mod = new DefaultSkillMod(SkillName.Archery, true, 5);
            m_Mobile.AddSkillMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
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
                "Sacrificial lamb",
                "leg"
            );
        }
    }

    public class ChickenLeg : Food
    {
        [Constructible]
        public ChickenLeg(int amount = 1) : base(0x1608, amount)
        {
            Weight = 1.0;
            FillFactor = 4;
        }

        public ChickenLeg(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    [Flippable(0xC74, 0xC75)]
    public class HoneydewMelon : Food
    {
        [Constructible]
        public HoneydewMelon(int amount = 1) : base(0xC74, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public HoneydewMelon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    [Flippable(0xC64, 0xC65)]
    public class YellowGourd : Food
    {
        [Constructible]
        public YellowGourd(int amount = 1) : base(0xC64, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public YellowGourd(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    [Flippable(0xC66, 0xC67)]
    public class GreenGourd : Food
    {
        [Constructible]
        public GreenGourd(int amount = 1) : base(0xC66, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public GreenGourd(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    [Flippable(0xC7F, 0xC81)]
    public class EarOfCorn : Food
    {
        [Constructible]
        public EarOfCorn(int amount = 1) : base(0xC81, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public EarOfCorn(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class Turnip : Food
    {
        [Constructible]
        public Turnip(int amount = 1) : base(0xD3A, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public Turnip(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class SheafOfHay : Item
    {
        [Constructible]
        public SheafOfHay() : base(0xF36) => Weight = 10.0;

        public SheafOfHay(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }
}
