using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class CarefulShooter : BaseTalent, ITalent
    {
        public CarefulShooter() : base()
        {
            TalentDependency = typeof(ArcherFocus);
            DisplayName = "Careful shooter";
            Description = "Lowers chance for arrow to be lost on miss.";
            ImageID = 30118;
        }

    }
}
