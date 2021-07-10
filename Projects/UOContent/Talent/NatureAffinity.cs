using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class FireAffinity : BaseTalent, ITalent
    {
        public FireAffinity() : base()
        {
            DisplayName = "Fire affinity";
            Description = "Increases damage done by fire abilities/spells.";
            ImageID = 30212;
        }

    }
}
