using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class EfficientSpinner : BaseTalent, ITalent
    {
        public EfficientSpinner() : base()
        {
            TalentDependency = typeof(ResourcefulHarvester);
            DisplayName = "Thread master";
            Description = "0.5% Chance per level to receive extra tailoring materials when spinning.";
            ImageID = 358;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 80;
            MaxLevel = 10;
        }
    }
}
