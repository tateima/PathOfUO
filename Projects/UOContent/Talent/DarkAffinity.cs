using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class DarkAffinity : BaseTalent, ITalent
    {
        public DarkAffinity() : base()
        {
            BlockedBy = new Type[] { typeof(LightAffinity) };
            DisplayName = "Dark affinity";
            Description = "Increases damage done by negative energy spells.";
            ImageID = 30042;
        }

    }
}
