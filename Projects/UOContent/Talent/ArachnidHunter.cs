using Server.Mobiles;
using System;


namespace Server.Talent
{
    public class ArachnidHunter : BaseTalent, ITalent
    {
        public ArachnidHunter() : base()
        {
            BlockedBy = new Type[] { typeof(ReptilianHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Arachnid hunter";
            Description = "Increases damage to arachnids and heals damage from them.";
            ImageID = 30213;
        }
        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.ArachnidGroup, target.GetType()))
            {
                defender.Heal(AOS.Scale(damage, Level));
            }
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.ArachnidGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }
    }
}
