using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class HumanoidHunter : BaseTalent, ITalent
    {
        public HumanoidHunter() : base()
        {
            BlockedBy = new Type[] { typeof(ElementalHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Humanoid hunter";
            Description = "Increases damage to humanoids and lowers damage from them.";
            ImageID = 183;
        }

        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.HumanoidGroup, target.GetType()))
            {
                
                defender.Heal(AOS.Scale(damage, Level));
            }
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.HumanoidGroup, target.GetType()) || target is PlayerMobile)
            {
                target.Damage(Level, attacker);
            }
        }


    }
}
