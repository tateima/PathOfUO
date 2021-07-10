using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class PolearmSpecialist : BaseTalent, ITalent
    {
        public PolearmSpecialist() : base()
        {
            TalentDependency = typeof(SwordsmanshipFocus);
            DisplayName = "Polearm specialist";
            Description = "Increases damage and hit chance of polearm weapons.";
            ImageID = 30038;
        }

    }
}
