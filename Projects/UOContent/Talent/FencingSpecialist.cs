using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class FencingSpecialist : BaseTalent, ITalent
    {
        public FencingSpecialist() : base()
        {
            TalentDependency = typeof(FencingFocus);
            DisplayName = "Fencing specialist";
            Description = "Increases damage and hit chance of one handed fencing weapons.";
            ImageID = 30145;
        }

    }
}
