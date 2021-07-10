using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class HandFinesse : BaseTalent, ITalent
    {
        public HandFinesse() : base()
        {
            TalentDependency = typeof(EscapeDeath);
            DisplayName = "Hand finesse";
            Description = "Decrease time between attacks and reduce damage to items.";
            ImageID = 30203;
        }

    }
}
