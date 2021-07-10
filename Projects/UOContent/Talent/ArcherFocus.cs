using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class ArcherFocus : BaseTalent, ITalent
    {
        public ArcherFocus() : base()
        {
            BlockedBy = new Type[] { typeof(MacefightingFocus), typeof(SwordsmanshipFocus), typeof(FencingFocus) };
            DisplayName = "Archer focus";
            Description = "Chance of getting a critical strike with ranged weapons.";
            ImageID = 39880;
        }

    }
}
