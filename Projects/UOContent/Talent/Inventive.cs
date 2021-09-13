using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class Inventive : BaseTalent, ITalent
    {
        public Inventive() : base()
        {
            TalentDependency = typeof(MerchantPorter);
            DisplayName = "Inventive";
            Description = "Increases potency of devices and inventions.";
            ImageID = 29;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 80;
        }
    }
}
