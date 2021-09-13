using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class SmoothTalker : BaseTalent, ITalent
    {
        public SmoothTalker() : base()
        {
            BlockedBy = new Type[] { typeof(MerchantPorter) };
            DisplayName = "Smooth talker";
            Description = "Receive discounts on items from vendors, scales with level.";
            ImageID = 366;
            GumpHeight = 85;
            AddEndY = 80;
            MaxLevel = 10;
        }

        public override double ModifySpellScalar()
        {
            return (Level / 100) * 2; // 2% per point
        }
    }
}
