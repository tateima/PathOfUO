using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class TycoonCrafter : BaseTalent, ITalent
    {
        public TycoonCrafter() : base()
        {
            TalentDependency = typeof(ResourcefulCrafter);
            DisplayName = "Tycoon";
            Description = "Increases value of crafted armor and weapon items sold to vendor.";
            ImageID = 40022;
        }

        public override double ModifySpellScalar()
        {
            return (Level / 100) * 2; // 2% per point
        }
    }
}
