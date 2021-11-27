using System;
using System.Collections.Generic;
using Server.Items;
using Server.Engines.BulkOrders;

namespace Server.Mobiles
{
    public class Cook : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new();

        [Constructible]
        public Cook() : base("the cook")
        {
            SetSkill(SkillName.Cooking, 90.0, 100.0);
            SetSkill(SkillName.TasteID, 75.0, 98.0);
        }

        public Cook(Serial serial) : base(serial)
        {
        }

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override VendorShoeType ShoeType => Utility.RandomBool() ? VendorShoeType.Sandals : VendorShoeType.Shoes;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCook());

            if (IsTokunoVendor)
            {
                m_SBInfos.Add(new SBSECook());
            }
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            AddItem(new HalfApron());
        }

        public override bool SupportsBulkOrders(Mobile from) => true;

        public override Item CreateBulkOrder(Mobile from, bool fromContextMenu)
        {
            if (from is PlayerMobile pm && pm.NextCookBulkOrder == TimeSpan.Zero &&
                (fromContextMenu || Utility.RandomDouble() < 0.2))
            {
                var theirSkill = pm.Skills.Cooking.Base;

                pm.NextCookBulkOrder = theirSkill switch
                {
                    >= 70.1 => TimeSpan.FromHours(6.0),
                    >= 50.1 => TimeSpan.FromHours(2.0),
                    _       => TimeSpan.FromHours(1.0)
                };

                if (theirSkill >= 70.1 && (theirSkill - 40.0) / 300.0 > Utility.RandomDouble())
                {
                    return new LargeCookingBOD();
                }

                return SmallCookingBOD.CreateRandomFor(from);
            }

            return null;
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
