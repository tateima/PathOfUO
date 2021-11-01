using Server.Mobiles;
using System;

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
            ImageID = 297;
            AddEndY = 90;
        }

        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.AbyssalGroup, target.GetType()))
            {
                defender.Heal(AOS.Scale(damage, Level));
            }
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.AbyssalGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }

    }
}
