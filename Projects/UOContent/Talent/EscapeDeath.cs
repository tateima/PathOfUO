using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class EscapeDeath : BaseTalent, ITalent
    {
        public EscapeDeath() : base()
        {
            TalentDependency = typeof(KeenSenses);
            DisplayName = "Escape death";
            Description = "Avoid a deathly blow. 5 minute cooldown.";
            ImageID = 39877;
        }

    }
}
