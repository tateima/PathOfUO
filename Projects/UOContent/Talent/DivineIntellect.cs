using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class DivineIntellect : BaseTalent, ITalent
    {
        public DivineIntellect() : base()
        {
            BlockedBy = new Type[] { typeof(DivineStrength), typeof(DivineDexterity) };
            DisplayName = "Divine intellect";
            Description = "Increases intellect by a percentage.";
            ImageID = 30034;
        }

    }
}
