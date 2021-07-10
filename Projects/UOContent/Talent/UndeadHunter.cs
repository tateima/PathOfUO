using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class UndeadHunter : BaseTalent, ITalent
    {
        public UndeadHunter() : base()
        {
            BlockedBy = new Type[] { typeof(AbyssalHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Undead hunter";
            Description = "Increases damage to undead and lowers damage from them.";
            ImageID = 30147;
        }

    }
}
