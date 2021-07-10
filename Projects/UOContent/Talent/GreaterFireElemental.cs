using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class GreaterFireElemental : BaseTalent, ITalent
    {
        public GreaterFireElemental() : base()
        {
            BlockedBy = new Type[] { typeof(MasterOfDeath), typeof(HolyAvenger) };
            TalentDependency = typeof(DragonAspect);
            DisplayName = "Greater fire lord";
            Description = "Summon a fire lord to assist you for 2 minutes. 5 minute cooldown.";
            ImageID = 30225;
            AddY = 130;
        }

    }
}
