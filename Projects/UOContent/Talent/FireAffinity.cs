using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class NatureAffinity : BaseTalent, ITalent
    {
        public NatureAffinity() : base()
        {
            DisplayName = "Nature affinity";
            Description = "Increases damage done by nature based abilities/spells.";
            ImageID = 30210;
        }

    }
}
