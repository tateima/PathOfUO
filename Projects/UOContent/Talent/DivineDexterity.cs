using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class DivineDexterity : BaseTalent, ITalent
    {
        public DivineDexterity() : base()
        {
            BlockedBy = new Type[] { typeof(DivineStrength), typeof(DivineIntellect) };
            DisplayName = "Divine dexterity";
            Description = "Increases dexterity by a percentage.";
            ImageID = 30992;
        }

    }
}
