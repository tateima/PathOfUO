using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class AxeSpecialist : BaseTalent, ITalent
    {
        public AxeSpecialist() : base()
        {
            TalentDependency = typeof(SwordsmanshipFocus);
            DisplayName = "Axe specialist";
            Description = "Increases damage and hit chance of axe weapons.";
            ImageID = 30231;
        }

    }
}
