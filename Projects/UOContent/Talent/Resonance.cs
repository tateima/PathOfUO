using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class Resonance : BaseTalent, ITalent
    {
        public Resonance() : base()
        {
            TalentDependency = typeof(SonicAffinity);
            DisplayName = "Resonance";
            Description = "Chance on barding success that target is damaged by sonic energy.";
            ImageID = 30093;
        }

    }
}
