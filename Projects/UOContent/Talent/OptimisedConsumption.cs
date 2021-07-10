using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class OptimisedConsumption : BaseTalent, ITalent
    {
        public OptimisedConsumption() : base()
        {
            TalentDependency = typeof(CraftFocus);
            DisplayName = "Optimised consumption";
            Description = "Increases effectiveness of consumed goods.";
            ImageID = 39888;
        }

    }
}
