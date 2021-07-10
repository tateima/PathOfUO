using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class ElementalHunter : BaseTalent, ITalent
    {
        public ElementalHunter() : base()
        {
            BlockedBy = new Type[] { typeof(HumanoidHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Elemental hunter";
            Description = "Increases damage to elementals and lowers damage from them.";
            ImageID = 30091;
        }

    }
}
