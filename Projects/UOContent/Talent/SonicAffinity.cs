using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class SonicAffinity : BaseTalent, ITalent
    {
        public SonicAffinity() : base()
        {
            DisplayName = "Sonic affinity";
            Description = "Increases power of provocation, peacemaking and discordance.";
            ImageID = 30234;
        }

    }
}
