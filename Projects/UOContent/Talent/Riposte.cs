using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class Riposte : BaseTalent, ITalent
    {
        public Riposte() : base()
        {
            TalentDependency = typeof(FencingFocus);
            DisplayName = "Riposte";
            Description = "Chance to deal damage to them if enemy misses.";
            ImageID = 30221;
        }

    }
}
