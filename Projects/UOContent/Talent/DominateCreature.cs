using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class DominateCreature : BaseTalent, ITalent
    {
        public DominateCreature() : base()
        {
            TalentDependency = typeof(Resonance);
            DisplayName = "Dominate creature";
            Description = "Chance on to control target for up to 1 minute. 10 minute cooldown.";
            ImageID = 30049;
        }

    }
}
