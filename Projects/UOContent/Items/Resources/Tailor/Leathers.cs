namespace Server.Items
{
    [Serializable(2, false)]
    public abstract partial class BaseLeather : Item, ICommodity
    {
        [InvalidateProperties]
        [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster)]")]
        [SerializableField(0)]
        private CraftResource _resource;

        public BaseLeather(CraftResource resource, int amount = 1) : base(0x1081)
        {
            Stackable = true;
            Weight = 1.0;
            Amount = amount;
            Hue = CraftResources.GetHue(resource);

            _resource = resource;
        }

        public override int LabelNumber
        {
            get
            {
                if (_resource >= CraftResource.SpinedLeather && _resource <= CraftResource.BarbedLeather)
                {
                    return 1049684 + (_resource - CraftResource.SpinedLeather);
                }

                return 1047022;
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
                        var info = new OreInfo(reader.ReadInt(), reader.ReadInt(), reader.ReadString());

                        _resource = CraftResources.GetFromOreInfo(info);
                        break;
                    }
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (Amount > 1)
            {
                list.Add(1050039, "{0}\t#{1}", Amount, 1024199); // ~1_NUMBER~ ~2_ITEMNAME~
            }
            else
            {
                list.Add(1024199); // cut leather
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
    [Flippable(0x1081, 0x1082)]
    public partial class Leather : BaseLeather
    {
        [Constructible]
        public Leather(int amount = 1) : base(CraftResource.RegularLeather, amount)
        {
        }
    }

    [Serializable(0, false)]
    [Flippable(0x1081, 0x1082)]
    public partial class SpinedLeather : BaseLeather
    {
        [Constructible]
        public SpinedLeather(int amount = 1) : base(CraftResource.SpinedLeather, amount)
        {
        }
    }

    [Serializable(0, false)]
    [Flippable(0x1081, 0x1082)]
    public partial class HornedLeather : BaseLeather
    {
        [Constructible]
        public HornedLeather(int amount = 1) : base(CraftResource.HornedLeather, amount)
        {
        }
    }

    [Serializable(0, false)]
    [Flippable(0x1081, 0x1082)]
    public partial class BarbedLeather : BaseLeather
    {
        [Constructible]
        public BarbedLeather(int amount = 1) : base(CraftResource.BarbedLeather, amount)
        {
        }
    }
}
