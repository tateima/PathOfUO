using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class PlanarShift : BaseTalent, ITalent
    {
        public PlanarShift() : base()
        {
            TalentDependency = typeof(MindMatter);
            DisplayName = "Planar shift";
            Description = "Reduces damage by 90% for 10 seconds. 3 minute cooldown.";
            ImageID = 30029;
        }

    }
}
