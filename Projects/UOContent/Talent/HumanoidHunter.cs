using Server.Mobiles;

namespace Server.Talent
{
    public class HumanoidHunter : BaseTalent
    {
        public HumanoidHunter()
        {
            BlockedBy = new[] { typeof(ElementalHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            HasDamageAbsorptionEffect = true;
            DisplayName = "Humanoid hunter";
            Description = "Increases damage to humanoids and lowers damage from them.";
            ImageID = 183;
            AddEndY = 90;
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (IsMobileType(OppositionGroup.HumanoidGroup, attacker.GetType()) || attacker is PlayerMobile)
            {
                damage -= AOS.Scale(damage, Level * 5);
            }
            return damage;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.HumanoidGroup, target.GetType()) || target is PlayerMobile)
            {
                target.Damage(Utility.RandomMinMax(1, Level), attacker);
            }
        }
    }
}
