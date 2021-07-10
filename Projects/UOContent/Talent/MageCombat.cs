using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class MageCombatant : BaseTalent, ITalent
    {
        public MageCombatant() : base()
        {
            BlockedBy = new Type[] { typeof(MaceSpecialist), typeof(TwoHandedMaceSpecialist) };
            TalentDependency = typeof(MacefightingFocus);
            DisplayName = "Mage combatant";
            Description = "Increases damage to spells when maces equipped.";
            ImageID = 39886;
        }

    }
}
