using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class LoreDisciples : BaseTalent, ITalent
    {
        public LoreDisciples() : base()
        {
            TalentDependency = typeof(LoreTeacher);
            DisplayName = "Lore disciples";
            Description = "Summon random humanoids to fight alongside you for 3 minutes (5 minute cooldown).";
            ImageID = 24035;
        }

    }
}
