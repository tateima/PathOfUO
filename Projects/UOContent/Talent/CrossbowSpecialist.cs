using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class CrossbowSpecialist : BaseTalent, ITalent
    {
        public CrossbowSpecialist() : base()
        {
            TalentDependency = typeof(ArcherFocus);
            DisplayName = "Crossbow specialist";
            Description = "Increases damage and hit chance of crossbow weapons.";
            ImageID = 39899;
        }

    }
}
