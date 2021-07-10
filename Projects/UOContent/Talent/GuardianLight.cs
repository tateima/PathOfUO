using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class GuardianLight : BaseTalent, ITalent
    {
        public GuardianLight() : base()
        {
            TalentDependency = typeof(LightAffinity);
            DisplayName = "Guardian of light";
            Description = "Chance to be healed or cured when damaged.";
            ImageID = 30032;
        }

    }
}
