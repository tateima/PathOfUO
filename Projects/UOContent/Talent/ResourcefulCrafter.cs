using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class ResourcefulCrafter : BaseTalent, ITalent
    {
        public ResourcefulCrafter() : base()
        {
            TalentDependency = typeof(WarCraftFocus);
            DisplayName = "Efficient crafting";
            Description = "Reduce material costs for crafting.";
            ImageID = 37;
            GumpHeight = 85;
            AddEndY = 80;
        }

    }
}
