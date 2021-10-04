using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class ReptilianHunter : BaseTalent, ITalent
    {
        public ReptilianHunter() : base()
        {
            BlockedBy = new Type[] { typeof(ArachnidHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Reptilian hunter";
            Description = "Increases damage to reptiles and heals damage from them.";
            ImageID = 187;
            AddEndY = 95;
        }

        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.ReptilianGroup, target.GetType()))
            {
                defender.Heal(AOS.Scale(damage, Level));
            }
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.ReptilianGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }

    }
}
