using System;
using ModernUO.Serialization;
using Server.Engines.VeteranRewards;
using Server.Gumps;
using Server.Multis;

namespace Server.Items;

public enum MiningCartType
{
    OreSouth = 100,
    OreEast = 101,
    GemSouth = 102,
    GemEast = 103
}

[SerializationGenerator(3)]
public partial class MiningCart : BaseAddon, IRewardItem
{
    [SerializableField(0, setter: "private")]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private MiningCartType _cartType;

    [InvalidateProperties]
    [SerializableField(1)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private bool _isRewardItem;

    [SerializableField(4, getter: "private", setter: "private")]
    private DateTime _lastResourceTime;

    [Constructible]
    public MiningCart(MiningCartType type)
    {
        _cartType = type;

        switch (type)
        {
            case MiningCartType.OreSouth:
                {
                    AddComponent(new AddonComponent(0x1A83), 0, 0, 0);
                    AddComponent(new AddonComponent(0x1A82), 0, 1, 0);
                    AddComponent(new AddonComponent(0x1A86), 0, -1, 0);
                    break;
                }
            case MiningCartType.OreEast:
                {
                    AddComponent(new AddonComponent(0x1A88), 0, 0, 0);
                    AddComponent(new AddonComponent(0x1A87), 1, 0, 0);
                    AddComponent(new AddonComponent(0x1A8B), -1, 0, 0);
                    break;
                }
            case MiningCartType.GemSouth:
                {
                    AddComponent(new LocalizedAddonComponent(0x1A83, 1080388), 0, 0, 0);
                    AddComponent(new LocalizedAddonComponent(0x1A82, 1080388), 0, 1, 0);
                    AddComponent(new LocalizedAddonComponent(0x1A86, 1080388), 0, -1, 0);

                    AddComponent(new AddonComponent(0xF2C), 0, 0, 6);
                    AddComponent(new AddonComponent(0xF1D), 0, 0, 5);
                    AddComponent(new AddonComponent(0xF2B), 0, 0, 2);
                    AddComponent(new AddonComponent(0xF21), 0, 0, 1);
                    AddComponent(new AddonComponent(0xF22), 0, 0, 4);
                    AddComponent(new AddonComponent(0xF2F), 0, 0, 5);
                    AddComponent(new AddonComponent(0xF26), 0, 0, 6);
                    AddComponent(new AddonComponent(0xF27), 0, 0, 3);
                    AddComponent(new AddonComponent(0xF29), 0, 0, 0);
                    break;
                }
            case MiningCartType.GemEast:
                {
                    AddComponent(new LocalizedAddonComponent(0x1A88, 1080388), 0, 0, 0);
                    AddComponent(new LocalizedAddonComponent(0x1A87, 1080388), 1, 0, 0);
                    AddComponent(new LocalizedAddonComponent(0x1A8B, 1080388), -1, 0, 0);

                    AddComponent(new AddonComponent(0xF2E), 0, 0, 6);
                    AddComponent(new AddonComponent(0xF12), 0, 0, 3);
                    AddComponent(new AddonComponent(0xF29), 0, 0, 1);
                    AddComponent(new AddonComponent(0xF24), 0, 0, 5);
                    AddComponent(new AddonComponent(0xF21), 0, 0, 1);
                    AddComponent(new AddonComponent(0xF2B), 0, 0, 3);
                    AddComponent(new AddonComponent(0xF2F), 0, 0, 4);
                    AddComponent(new AddonComponent(0xF23), 0, 0, 3);
                    AddComponent(new AddonComponent(0xF27), 0, 0, 3);
                    break;
                }
        }

        _lastResourceTime = Core.Now;
    }

    public override BaseAddonDeed Deed
    {
        get
        {
            GiveResources();
            return new MiningCartDeed { IsRewardItem = IsRewardItem, Gems = _gems, Ore = _ore };
        }
    }

    [SerializableProperty(2)]
    [CommandProperty(AccessLevel.GameMaster)]
    public int Gems
    {
        get
        {
            GiveResources();
            return _gems;
        }
        set
        {
            _gems = value;
            this.MarkDirty();
        }
    }

    [SerializableProperty(3)]
    [CommandProperty(AccessLevel.GameMaster)]
    public int Ore
    {
        get
        {
            GiveResources();
            return _ore;
        }
        set
        {
            _ore = value;
            this.MarkDirty();
        }
    }

    private void GiveResources()
    {
        var amount = (Core.Now - _lastResourceTime).Days;
        if (amount <= 0)
        {
            return;
        }

        switch (_cartType)
        {
            case MiningCartType.OreSouth:
            case MiningCartType.OreEast:
                {
                    _ore = Math.Min(100, _ore + amount * 10);
                    break;
                }
            case MiningCartType.GemSouth:
            case MiningCartType.GemEast:
                {
                    _gems = Math.Min(50, _gems + amount * 5);
                    break;
                }
        }

        _lastResourceTime += TimeSpan.FromDays(amount);
    }

    public override void OnComponentUsed(AddonComponent c, Mobile from)
    {
        var house = BaseHouse.FindHouseAt(this);

        /*
         * Unique problems have unique solutions.  OSI does not have a problem with 1000s of mining carts
         * due to the fact that they have only a miniscule fraction of the number of 10 year vets that a
         * typical RunUO shard will have (RunUO's scaled down account aging system makes this a unique problem),
         * and the "freeness" of free accounts. We also dont have mitigating factors like inactive (unpaid)
         * accounts not gaining veteran time.
         *
         * The lack of high end vets and vet rewards on OSI has made testing the *exact* ranging/stacking
         * behavior of these things all but impossible, so either way its just an estimation.
         *
         * If youd like your shard's carts/stumps to work the way they did before, simply replace the check
         * below with this line of code:
         *
         * if (!from.InRange(GetWorldLocation(), 2)
         *
         * However, I am sure these checks are more accurate to OSI than the former version was.
         *
         */

        if (!from.InRange(GetWorldLocation(), 2) || !from.InLOS(this) || !(from.Z - Z > -3 && from.Z - Z < 3))
        {
            from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            return;
        }

        if (house?.HasSecureAccess(from, SecureLevel.Friends) != true)
        {
            from.SendLocalizedMessage(1061637); // You are not allowed to access this.
            return;
        }

        GiveResources();

        switch (CartType)
        {
            case MiningCartType.OreSouth:
            case MiningCartType.OreEast:
                {
                    if (_ore > 0)
                    {
                        var ingots = Utility.Random(9) switch
                        {
                            0 => (Item)new IronIngot(),
                            1 => new DullCopperIngot(),
                            2 => new ShadowIronIngot(),
                            3 => new CopperIngot(),
                            4 => new BronzeIngot(),
                            5 => new GoldIngot(),
                            6 => new AgapiteIngot(),
                            7 => new VeriteIngot(),
                            _ => new ValoriteIngot(),
                        };

                        var amount = Math.Min(10, _ore);
                        ingots.Amount = amount;

                        if (!from.PlaceInBackpack(ingots))
                        {
                            ingots.Delete();
                            // Your backpack is full! Please make room and try again.
                            from.SendLocalizedMessage(1078837);
                        }
                        else
                        {
                            PublicOverheadMessage(MessageType.Regular, 0, 1094724, amount.ToString()); // Ore: ~1_COUNT~
                            _ore -= amount;
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(1094725); // There are no more resources available at this time.
                    }

                    break;
                }
            case MiningCartType.GemSouth:
            case MiningCartType.GemEast:
                {
                    if (_gems > 0)
                    {
                        Item gems = Utility.Random(15) switch
                        {
                            0 => new Amber(),
                            1 => new Amethyst(),
                            2 => new Citrine(),
                            3 => new Diamond(),
                            4 => new Emerald(),
                            5 => new Ruby(),
                            6 => new Sapphire(),
                            7 => new StarSapphire(),
                            8 => new Tourmaline(),

                            // Mondain's Legacy gems
                            9  => new PerfectEmerald(),
                            10 => new DarkSapphire(),
                            11 => new Turquoise(),
                            12 => new EcruCitrine(),
                            13 => new FireRuby(),
                            _  => new BlueDiamond() // 14
                        };

                        var amount = Math.Min(5, _gems);
                        gems.Amount = amount;

                        if (!from.PlaceInBackpack(gems))
                        {
                            gems.Delete();
                            // Your backpack is full! Please make room and try again.
                            from.SendLocalizedMessage(1078837);
                        }
                        else
                        {
                            PublicOverheadMessage(MessageType.Regular, 0, 1094723, amount.ToString()); // Gems: ~1_COUNT~
                            _gems -= amount;
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(1094725); // There are no more resources available at this time.
                    }

                    break;
                }
        }
    }

    private void Deserialize(IGenericReader reader, int version)
    {
        _cartType = (MiningCartType)reader.ReadInt();

        _isRewardItem = reader.ReadBool();
        _gems = reader.ReadInt();
        _ore = reader.ReadInt();

        _lastResourceTime = reader.ReadDateTime();
    }

    [AfterDeserialization]
    private void AfterDeserialization()
    {
        GiveResources();
    }
}

[SerializationGenerator(0)]
public partial class MiningCartDeed : BaseAddonDeed, IRewardItem, IRewardOption
{
    [InvalidateProperties]
    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private bool _isRewardItem;

    [SerializableField(1)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _gems;

    [SerializableField(2)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _ore;

    private MiningCartType _cartType;

    [Constructible]
    public MiningCartDeed() => LootType = LootType.Blessed;

    public override int LabelNumber => 1080385; // deed for a mining cart decoration

    public override BaseAddon Addon =>
        new MiningCart(_cartType) { IsRewardItem = _isRewardItem, Gems = Gems, Ore = Ore };

    public void GetOptions(RewardOptionList list)
    {
        list.Add((int)MiningCartType.OreSouth, 1080391);
        list.Add((int)MiningCartType.OreEast, 1080390);
        list.Add((int)MiningCartType.GemSouth, 1080500);
        list.Add((int)MiningCartType.GemEast, 1080499);
    }

    public void OnOptionSelected(Mobile from, int choice)
    {
        _cartType = (MiningCartType)choice;

        if (!Deleted)
        {
            base.OnDoubleClick(from);
        }
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (_isRewardItem)
        {
            list.Add(1080457); // 10th Year Veteran Reward
        }
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (_isRewardItem && !RewardSystem.CheckIsUsableBy(from, this))
        {
            return;
        }

        if (IsChildOf(from.Backpack))
        {
            from.CloseGump<RewardOptionGump>();
            from.SendGump(new RewardOptionGump(this));
        }
        else
        {
            from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
        }
    }
}
