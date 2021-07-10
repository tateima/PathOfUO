using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class BowSpecialist : BaseTalent, ITalent
    {
        public BowSpecialist() : base()
        {
            TalentDependency = typeof(ArcherFocus);
            DisplayName = "Bow specialist";
            Description = "Increases damage and hit chance of bow weapons.";
            ImageID = 30219;
        }

    }
}
