using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class ResourcefulCrafter : BaseTalent, ITalent
    {
        public ResourcefulCrafter() : base()
        {
            TalentDependency = typeof(WarCraftFocus);
            DisplayName = "Resourceful";
            Description = "Reduce material costs for crafting.";
            ImageID = 30208;
        }

    }
}
