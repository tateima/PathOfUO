using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class MacefightingFocus : BaseTalent, ITalent
    {
        public MacefightingFocus() : base()
        {
            BlockedBy = new Type[] { typeof(ArcherFocus) };
            DisplayName = "Macefighting focus";
            Description = "Chance of getting a critical strike with macing weapons.";
            ImageID = 30196;
        }

    }
}
