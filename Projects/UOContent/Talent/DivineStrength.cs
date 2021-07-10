using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class DivineStrength : BaseTalent, ITalent
    {
        public DivineStrength() : base()
        {
            BlockedBy = new Type[] { typeof(DivineIntellect), typeof(DivineDexterity) };
            DisplayName = "Divine strength";
            Description = "Increases strength by a percentage.";
            ImageID = 39870;
        }

    }
}
