using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class LightAffinity : BaseTalent, ITalent
    {
        public LightAffinity() : base()
        {
            BlockedBy = new Type[] { typeof(DarkAffinity) };
            DisplayName = "Light affinity";
            Description = "Increases healing done by spells.";
            ImageID = 39861;
        }

    }
}
