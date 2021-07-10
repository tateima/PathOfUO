using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class HunterOfWild : BaseTalent, ITalent
    {
        public HunterOfWild() : base()
        {
            DisplayName = "Hunter";
            Description = "Lowers damage from animals.";
            ImageID = 30218;
        }

    }
}
