using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class SwordsmanshipFocus : BaseTalent, ITalent
    {
        public SwordsmanshipFocus() : base()
        {
            BlockedBy = new Type[] { typeof(ArcherFocus) };
            DisplayName = "Swordsmanship focus";
            Description = "Chance of getting a critical strike with  swordsmanship weapons.";
            ImageID = 39862;
        }

    }
}
