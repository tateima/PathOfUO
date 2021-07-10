using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class TycoonCrafter : BaseTalent, ITalent
    {
        public TycoonCrafter() : base()
        {
            TalentDependency = typeof(ResourcefulCrafter);
            DisplayName = "Tycoon";
            Description = "Increase gold find from all sources.";
            ImageID = 40022;
        }

    }
}
