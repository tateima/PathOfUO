using Server.Talent;
using Server.Mobiles;
using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(2, false)]
public abstract partial class BaseHides : Item, ICommodity
{
    [InvalidateProperties]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    [SerializableField(0)]
    private CraftResource _resource;

    public BaseHides(CraftResource resource, int amount = 1) : base(0x1079)
    {
        Stackable = true;
        Weight = 5.0;
        Amount = amount;
        Hue = CraftResources.GetHue(resource);

        _resource = resource;
    }

    public static int CheckEfficientSkinner(Mobile from, int amount)
    {
        BaseTalent skinMaster = null;
        if (from is PlayerMobile player)
        {
            skinMaster = player.GetTalent(typeof(EfficientSkinner));
            if (skinMaster != null)
            {
                return skinMaster.GetExtraResourceCheck(amount);
            }
        }
        return 0;
    }

    public override int LabelNumber
    {
        get
        {
            if (_resource >= CraftResource.SpinedLeather && _resource <= CraftResource.BarbedLeather)
            {
                return 1049687 + (_resource - CraftResource.SpinedLeather);
            }

            return 1047023;
        }
    }

    int ICommodity.DescriptionNumber => LabelNumber;
    bool ICommodity.IsDeedable => true;

    private void Deserialize(IGenericReader reader, int version)
    {
        _resource = (CraftResource)reader.ReadInt();
    }

    public override void AddNameProperty(IPropertyList list)
    {
        if (Amount > 1)
        {
            list.Add(1050039, $"{Amount}\t{1024216:#}"); // ~1_NUMBER~ ~2_ITEMNAME~
        }
        else
        {
            list.Add(1024216); // pile of hides
        }
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (!CraftResources.IsStandard(_resource))
        {
            var num = CraftResources.GetLocalizationNumber(_resource);

            if (num > 0)
            {
                list.Add(num);
            }
            else
            {
                list.Add(CraftResources.GetName(_resource));
            }
        }
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1079, 0x1078)]
public partial class Hides : BaseHides, IScissorable
{
    [Constructible]
    public Hides(int amount = 1) : base(CraftResource.RegularLeather, amount)
    {
    }

    public bool Scissor(Mobile from, Scissors scissors)
    {
        if (Deleted || !from.CanSee(this))
        {
            return false;
        }

        if (Core.AOS && !IsChildOf(from.Backpack))
        {
            from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
            return false;
        }

        ScissorHelper(from, new Leather(), 1);

        return true;
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1079, 0x1078)]
public partial class SpinedHides : BaseHides, IScissorable
{
    [Constructible]
    public SpinedHides(int amount = 1) : base(CraftResource.SpinedLeather, amount)
    {
    }

    public bool Scissor(Mobile from, Scissors scissors)
    {
        if (Deleted || !from.CanSee(this))
        {
            return false;
        }

        if (Core.AOS && !IsChildOf(from.Backpack))
        {
            from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
            return false;
        }

        ScissorHelper(from, new SpinedLeather(), 1);

        return true;
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1079, 0x1078)]
public partial class HornedHides : BaseHides, IScissorable
{
    [Constructible]
    public HornedHides(int amount = 1) : base(CraftResource.HornedLeather, amount)
    {
    }

    public bool Scissor(Mobile from, Scissors scissors)
    {
        if (Deleted || !from.CanSee(this))
        {
            return false;
        }

        if (Core.AOS && !IsChildOf(from.Backpack))
        {
            from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
            return false;
        }

        ScissorHelper(from, new HornedLeather(), 1);

        return true;
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1079, 0x1078)]
public partial class BarbedHides : BaseHides, IScissorable
{
    [Constructible]
    public BarbedHides(int amount = 1) : base(CraftResource.BarbedLeather, amount)
    {
    }

    public bool Scissor(Mobile from, Scissors scissors)
    {
        if (Deleted || !from.CanSee(this))
        {
            return false;
        }

        if (Core.AOS && !IsChildOf(from.Backpack))
        {
            from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack
            return false;
        }

        ScissorHelper(from, new BarbedLeather(), 1);

        return true;
    }
}
