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
            Description = "Increases damage to undead and heals damage from them.";
            ImageID = 30147;
        }

        public void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(target, OppositionGroup.UndeadGroup, target.GetType()))
            {
                defender.Heal((damage / 100) * Level);
            }
        }

        public void CheckHitEffect(Mobile attacker, Mobile target)
        {
            if (IsMobileType(target, OppositionGroup.UndeadGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }

    }
}
