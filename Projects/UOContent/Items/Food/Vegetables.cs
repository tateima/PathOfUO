using System.Collections.Generic;
using ModernUO.Serialization;
using Server;
using Server.ContextMenus;

namespace Server.Items;

public abstract partial class Food : Item
{
    [SerializableField(0)]
    [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster)]")]
    private Mobile _poisoner;

    [SerializableField(1)]
    [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster)]")]
    private Poison _poison;

    [SerializableField(2)]
    [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster)]")]
    private int _fillFactor;

    public Food(int itemID, int amount = 1) : base(itemID)
    {
        Stackable = true;
        Amount = amount;
        FillFactor = 1;
    }

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

    public override bool CanStackWith(Item dropped)
    {
        if (dropped is Food food)
        {
            if (Poison != food.Poison || Poisoner != food.Poisoner)
            {
                return false;
            }
        }
        return base.CanStackWith(dropped);
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
}

[Flippable(0xc77, 0xc78)]
[SerializationGenerator(0, false)]
public partial class Carrot : Food
{
    [Constructible]
    public Carrot(int amount = 1) : base(0xc78, amount)
    {
        Weight = 1.0;
        FillFactor = 1;
    }
}
[Flippable(0xc77, 0xc78)]
[SerializationGenerator(0, false)]
public partial class Chilli : Food
{
    private Mobile m_Mobile;
    private ResistanceMod m_Mod;

    [Constructible]
    public Chilli(int amount = 1) : base(0xc78, amount)
    {
        Hue = 0x8A;
        Weight = 1.0;
        FillFactor = 1;
    }
    public bool Eat(Mobile from) {
        from.SendMessage("You feel a slight increase in fire resistance");
        m_Mobile = from;
        m_Mod = new ResistanceMod(ResistanceType.Fire, +1);
        m_Mobile.AddResistanceMod(m_Mod);
        Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
        return base.Eat(from);
    }
    public void ExpireBuff() {
        if (m_Mobile != null && m_Mod != null) {
            m_Mobile.RemoveResistanceMod(m_Mod);
        }
    }
    public override void GetProperties(IPropertyList list)
    {
        list.Add(
            1060847,
            "Chilli",
            ""
        );
    }
}

[SerializationGenerator(0, false)]
public partial class FrozenCabbage : Food
{
    private Mobile m_Mobile;
    private ResistanceMod m_Mod;

    [Constructible]
    public FrozenCabbage(int amount = 1) : base(0xc78, amount)
    {
        Hue = 0xBC;
        Weight = 1.0;
        FillFactor = 1;
    }
    public override bool Eat(Mobile from) {
        from.SendMessage("You feel a slight increase in cold resistance");
        m_Mobile = from;
        m_Mod = new ResistanceMod(ResistanceType.Cold, +5);
        m_Mobile.AddResistanceMod(m_Mod);
        Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
        return base.Eat(from);
    }
    public void ExpireBuff() {
        if (m_Mobile != null && m_Mod != null) {
            m_Mobile.RemoveResistanceMod(m_Mod);
        }
    }
    public override void GetProperties(IPropertyList list)
    {
        list.Add(
            1060847,
            "{0} {1}",
            "Frozen",
            "cabbage"
        );
    }
}
[Flippable(0xc7b, 0xc7c)]
[SerializationGenerator(0, false)]
public partial class Cabbage : Food
{
    [Constructible]
    public Cabbage(int amount = 1) : base(0xc7b, amount)
    {
        Weight = 1.0;
        FillFactor = 1;
    }
}

[Flippable(0xc6d, 0xc6e)]
[SerializationGenerator(0, false)]
public partial class Onion : Food
{
    [Constructible]
    public Onion(int amount = 1) : base(0xc6d, amount)
    {
        Weight = 1.0;
        FillFactor = 1;
    }
}

[Flippable(0xc70, 0xc71)]
[SerializationGenerator(0, false)]
public partial class Lettuce : Food
{
    [Constructible]
    public Lettuce(int amount = 1) : base(0xc70, amount)
    {
        Weight = 1.0;
        FillFactor = 1;
    }
}

[Flippable(0xC6A, 0xC6B)]
[SerializationGenerator(0, false)]
public partial class Pumpkin : Food
{
    [Constructible]
    public Pumpkin(int amount = 1) : base(0xC6A, amount)
    {
        Weight = 1.0;
        FillFactor = 8;
    }
}

[SerializationGenerator(0, false)]
public partial class SmallPumpkin : Food
{
    [Constructible]
    public SmallPumpkin(int amount = 1) : base(0xC6C, amount)
    {
        Weight = 1.0;
        FillFactor = 8;
    }
}
