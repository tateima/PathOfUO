using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class TwoHandedMaceSpecialist : BaseTalent, ITalent
    {
        public TwoHandedMaceSpecialist() : base()
        {
            BlockedBy = new Type[] { typeof(MaceSpecialist), typeof(MageCombatant) };
            TalentDependency = typeof(MacefightingFocus);
            DisplayName = "Two handed macing specialist";
            Description = "Increases damage to two handed macefighting weapons.";
            ImageID = 39885;
        }

    }
}
