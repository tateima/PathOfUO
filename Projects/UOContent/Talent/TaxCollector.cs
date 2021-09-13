using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class TaxCollector : BaseTalent, ITalent
    {
        public TaxCollector() : base()
        {
            TalentDependency = typeof(SmoothTalker);
            DisplayName = "Land Lord";
            Description = "Receive tax payments from a maximum of 10 vendors every 3h, can result in gold loss.";
            ImageID = 364;
            GumpHeight = 85;
            AddEndY = 105;
            MaxLevel = 10;
        }

        public bool VendorCantPay()
        {
            return (Utility.Random(100) < 11 - Level);
        }

        public override double ModifySpellScalar()
        {
            return (Level / 100) * 2; // 2% per point
        }
    }
}
