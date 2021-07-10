using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class ShieldBash : BaseTalent, ITalent
    {
        public ShieldBash() : base()
        {
            TalentDependency = typeof(ShieldFocus);
            DisplayName = "Shield bash";
            Description = "Stun target if hits for 5 seconds. 30 second cooldown";
            ImageID = 39889;
        }

    }
}
