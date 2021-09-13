using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class EfficientSmelter : BaseTalent, ITalent
    {
        public EfficientSmelter() : base()
        {
            TalentDependency = typeof(ResourcefulHarvester);
            DisplayName = "Metal worker";
            Description = "0.5% Chance per level to rhance on harvesting to receive extra resources.";
            ImageID = 356;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 90;
            MaxLevel = 10;
        }
    }
}
