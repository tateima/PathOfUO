using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class SpearSpecialist : BaseTalent, ITalent
    {
        public SpearSpecialist() : base()
        {
            TalentDependency = typeof(FencingFocus);
            DisplayName = "Spear specialist";
            Description = "Increases damage and hit chance of spear weapons.";
            ImageID = 30230;
        }

    }
}
