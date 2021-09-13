using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class ResourcefulHarvester : BaseTalent, ITalent
    {
        public ResourcefulHarvester() : base()
        {
            DisplayName = "Resourceful";
            Description = "Chance on harvesting to receive extra resources.";
            ImageID = 124;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 55;
        }
    }
}
