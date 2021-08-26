namespace Server.Items
{
    [Serializable(2, false)]
    public abstract partial class BaseIngot : Item, ICommodity
    {
        [InvalidateProperties]
        [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster)]")]
        [SerializableField(0)]
        private CraftResource _resource;

        public BaseIngot(CraftResource resource, int amount = 1) : base(0x1BF2)
        {
            Stackable = true;
            Amount = amount;
            Hue = CraftResources.GetHue(resource);

            _resource = resource;
        }

        public override double DefaultWeight => 0.1;

        public override int LabelNumber
        {
            get
            {
                if (_resource >= CraftResource.DullCopper && _resource <= CraftResource.Valorite)
                {
                    return 1042684 + (_resource - CraftResource.DullCopper);
                }

                return 1042692;
            }
        }

        int ICommodity.DescriptionNumber => LabelNumber;
        bool ICommodity.IsDeedable => true;

        private void Deserialize(IGenericReader reader, int version)
        {
            switch (version)
            {
                case 1:
                    {
                        _resource = (CraftResource)reader.ReadInt();
                        break;
                    }
                case 0:
                    {
                        var info = reader.ReadInt() switch
                        {
                            0 => OreInfo.Iron,
                            1 => OreInfo.DullCopper,
                            2 => OreInfo.ShadowIron,
                            3 => OreInfo.Copper,
                            4 => OreInfo.Bronze,
                            5 => OreInfo.Gold,
                            6 => OreInfo.Agapite,
                            7 => OreInfo.Verite,
                            8 => OreInfo.Valorite,
                            _ => null
                        };

                        _resource = CraftResources.GetFromOreInfo(info);
                        break;
                    }
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (Amount > 1)
            {
                list.Add(1050039, "{0}\t#{1}", Amount, 1027154); // ~1_NUMBER~ ~2_ITEMNAME~
            }
            else
            {
                list.Add(1027154); // ingots
            }
        }

        public override void GetProperties(ObjectPropertyList list)
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

    [Serializable(0, false)]
    [Flippable(0x1BF2, 0x1BEF)]
    public partial class IronIngot : BaseIngot
    {
        [Constructible]
        public IronIngot(int amount = 1) : base(CraftResource.Iron, amount)
        {
        }
    }

    [Serializable(0, false)]
    [Flippable(0x1BF2, 0x1BEF)]
    public partial class DullCopperIngot : BaseIngot
    {
        [Constructible]
        public DullCopperIngot(int amount = 1) : base(CraftResource.DullCopper, amount)
        {
        }
    }

    [Serializable(0, false)]
    [Flippable(0x1BF2, 0x1BEF)]
    public partial class ShadowIronIngot : BaseIngot
    {
        [Constructible]
        public ShadowIronIngot(int amount = 1) : base(CraftResource.ShadowIron, amount)
        {
        }
    }

    [Serializable(0, false)]
    [Flippable(0x1BF2, 0x1BEF)]
    public partial class CopperIngot : BaseIngot
    {
        [Constructible]
        public CopperIngot(int amount = 1) : base(CraftResource.Copper, amount)
        {
        }
    }

    [Serializable(0, false)]
    [Flippable(0x1BF2, 0x1BEF)]
    public partial class BronzeIngot : BaseIngot
    {
        [Constructible]
        public BronzeIngot(int amount = 1) : base(CraftResource.Bronze, amount)
        {
        }
    }

    [Serializable(0, false)]
    [Flippable(0x1BF2, 0x1BEF)]
    public partial class GoldIngot : BaseIngot
    {
        [Constructible]
        public GoldIngot(int amount = 1) : base(CraftResource.Gold, amount)
        {
        }
    }

    [Serializable(0, false)]
    [Flippable(0x1BF2, 0x1BEF)]
    public partial class AgapiteIngot : BaseIngot
    {
        [Constructible]
        public AgapiteIngot(int amount = 1) : base(CraftResource.Agapite, amount)
        {
        }
    }

    [Serializable(0, false)]
    [Flippable(0x1BF2, 0x1BEF)]
    public partial class VeriteIngot : BaseIngot
    {
        [Constructible]
        public VeriteIngot(int amount = 1) : base(CraftResource.Verite, amount)
        {
        }
    }

    [Serializable(0, false)]
    [Flippable(0x1BF2, 0x1BEF)]
    public partial class ValoriteIngot : BaseIngot
    {
        [Constructible]
        public ValoriteIngot(int amount = 1) : base(CraftResource.Valorite, amount)
        {
        }
    }
}
