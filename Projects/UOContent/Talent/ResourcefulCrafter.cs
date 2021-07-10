using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class ResourcefulCrafter : BaseTalent, ITalent
    {
        public ResourcefulCrafter() : base()
        {
            TalentDependency = typeof(CraftFocus);
            DisplayName = "Resourceful";
            Description = "Reduce material costs for crafting.";
            ImageID = 30208;
        }

    }
}
