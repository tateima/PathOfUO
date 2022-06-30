using ModernUO.Serialization;
using Server.Pantheon;
using Server.Talent;
using Server.Targeting;

namespace Server.Items
{
    [SerializationGenerator(0, false)]
    public partial class PantheonScroll : Item
    {
        [InvalidateProperties]
        [SerializableField(0)]
        [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster)]")]
        private string _alignmentRaw;

        // [SerializableField(1)]
        // [CommandProperty(AccessLevel.GameMaster)]
        // public string AlignmentRaw
        // {
        //     get => _alignmentRaw;
        //     set
        //     {
        //         if (_alignmentRaw is not null)
        //         {
        //             _alignmentRaw = value;
        //             InvalidateProperties();
        //             this.MarkDirty();
        //         }
        //     }
        // }

        private int _talentIndex;
        [EncodedInt]
        [SerializableField(1)]
        [CommandProperty(AccessLevel.GameMaster)]
        public int TalentIndex
        {
            get => _talentIndex;
            set
            {
                if (_talentIndex != value)
                {
                    _talentIndex = value;
                    Talent = _talentIndex < BaseTalent.InvalidTalentIndex ? TalentConstructor.ConstructFromIndex(_talentIndex) : null;
                    InvalidateProperties();
                    this.MarkDirty();
                }
            }
        }
        private int _talentLevel;

        [EncodedInt]
        [SerializableField(2)]
        [CommandProperty(AccessLevel.GameMaster)]
        public int TalentLevel
        {
            get => _talentLevel;
            set
            {
                if (_talentLevel != value)
                {
                    _talentLevel = value;
                    InvalidateProperties();
                    this.MarkDirty();
                }
            }
        }
        public BaseTalent Talent { get; set; }
        [Constructible]
        public PantheonScroll() : base(0x1F2E)
        {
            Amount = 1;
            ItemID = Utility.Random(12) switch
            {
                1  => 0x0E35,
                2  => 0x0E36,
                3  => 0x0E37,
                4  => 0x0E38,
                5  => 0x0E39,
                6  => 0x0E3A,
                7  => 0x0EF4,
                8  => 0x0EF5,
                9  => 0x0EF6,
                10 => 0x0EF7,
                11 => 0x0EF8,
                12 => 0x0EF9,
                _  => ItemID
            };
            Name = $"a pantheon scroll";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else
            {
                from.Target = new InternalTarget(from, this);
            }
        }
        private void Deserialize(IGenericReader reader, int version)
        {
            base.Deserialize(reader);

            _alignmentRaw = reader.ReadString();
            _talentIndex = reader.ReadInt();
            Talent = _talentIndex < BaseTalent.InvalidTalentIndex ? TalentConstructor.ConstructFromIndex(_talentIndex) : null;
            _talentLevel = reader.ReadInt();
        }

        public override void GetProperties(IPropertyList list)
        {
            base.GetProperties(list);
            if (_alignmentRaw != null)
            {
                list.Add(1114057, $"Alignment: {AlignmentRaw}"); // ~1_val~
            }
            if (_talentIndex < BaseTalent.InvalidTalentIndex && TalentLevel > 0)
            {
                list.Add(1114057, $"{Talent.DisplayName} + {TalentLevel.ToString()}"); // ~1_val~
            }
        }

        private class InternalTarget : Target
        {
            private readonly Mobile m_Mobile;
            private PantheonScroll m_PantheonScroll;

            public InternalTarget(Mobile mobile, PantheonScroll scroll) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                m_Mobile = mobile;
                m_PantheonScroll = scroll;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is (Cloak or BaseMiddleTorso or BaseJewel or BaseWeapon) and IPantheonItem item) // limit it for now
                {
                    item.AlignmentRaw = m_PantheonScroll.AlignmentRaw;
                    item.TalentIndex = m_PantheonScroll.TalentIndex;
                    item.TalentLevel = m_PantheonScroll.TalentLevel;
                    from.PlaySound(0x1FA);
                    m_PantheonScroll.Delete();
                }
                else
                {
                    from.SendMessage("This item cannot be infused with a pantheon blessing.");
                }
            }
        }
    }
}
