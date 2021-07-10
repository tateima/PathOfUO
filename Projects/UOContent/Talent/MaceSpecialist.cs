using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class MaceSpecialist : BaseTalent, ITalent
    {
        public MaceSpecialist() : base()
        {
            BlockedBy = new Type[] { typeof(MageCombatant), typeof(TwoHandedMaceSpecialist) };
            TalentDependency = typeof(MacefightingFocus);
            DisplayName = "Mace specialist";
            Description = "Increases damage to one handed macefighting weapons.";
            ImageID = 30226;
        }

    }
}
