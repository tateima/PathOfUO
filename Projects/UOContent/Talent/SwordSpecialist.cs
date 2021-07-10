using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class SwordSpecialist : BaseTalent, ITalent
    {
        public SwordSpecialist() : base()
        {
            TalentDependency = typeof(SwordsmanshipFocus);
            DisplayName = "Spar";
            Description = "Gives your swords a chance to parry an attack.";
            ImageID = 30197;
        }

    }
}
