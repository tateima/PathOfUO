using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class HumanoidHunter : BaseTalent, ITalent
    {
        public HumanoidHunter() : base()
        {
            BlockedBy = new Type[] { typeof(ElementalHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Humanoid hunter";
            Description = "Increases damage to humanoids and lowers damage from them.";
            ImageID = 40158;
        }

    }
}
