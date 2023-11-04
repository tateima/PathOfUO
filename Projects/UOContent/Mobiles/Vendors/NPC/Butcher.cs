using ModernUO.Serialization;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Butcher : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new();

        [Constructible]
        public Butcher() : base("the butcher")
        {
            SetSkill(SkillName.Anatomy, 45.0, 68.0);
        }

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBButcher());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            AddItem(new HalfApron());
            AddItem(new Cleaver());
        }
    }
}
