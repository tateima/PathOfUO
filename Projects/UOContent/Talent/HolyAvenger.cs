using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class HolyAvenger : BaseTalent, ITalent
    {
        public HolyAvenger() : base()
        {
            BlockedBy = new Type[] { typeof(GreaterFireElemental) };
            TalentDependency = typeof(LightAffinity);
            DisplayName = "Holy avenger";
            Description = "Increased damage to holy spells, adds area of affect damage to healing.";
            ImageID = 30016;
        }

    }
}
