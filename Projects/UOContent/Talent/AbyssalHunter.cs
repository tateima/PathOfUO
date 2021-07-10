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
            Description = "Increases damage to abyssals and heals damage from them.";
            ImageID = 30232;
        }

        public void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(target, OppositionGroup.AbyssalGroup, target.GetType()))
            {
                defender.Heal((damage / 100) * Level);
            }
        }

        public void CheckHitEffect(Mobile attacker, Mobile target)
        {
            if (IsMobileType(target, OppositionGroup.AbyssalGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }

    }
}
