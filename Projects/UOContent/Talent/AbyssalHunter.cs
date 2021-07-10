using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class AbyssalHunter : BaseTalent, ITalent
    {
        public AbyssalHunter() : base()
        {
            BlockedBy = new Type[] { typeof(UndeadHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Abyssal hunter";
            Description = "Increases damage to abyssals and lowers damage from them.";
            ImageID = 30232;
        }

    }
}
