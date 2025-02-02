using Server.Mobiles;

namespace Server.Talent
{
    public class HumanoidHunter : BaseTalent
    {
        public HumanoidHunter()
        {
            BlockedBy = new[] { typeof(ElementalHunter) };
            TalentDependencies = new[] { typeof(ExperiencedHunter) };
            HasDamageAbsorptionEffect = true;
            DisplayName = "Humanoid hunter";
            Description = "Increases damage to humanoids and lowers damage from them.";
            AdditionalDetail = $"The damage caused is 1-X where X is the talent level. The damage absorbed from their attacks is 5% per level. {PassiveDetail}";
            ImageID = 183;
            AddEndY = 90;
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (IsMobileType(OppositionGroup.DarknessAndLight[0], attacker.GetType()) || attacker is PlayerMobile)
            {
                damage -= AOS.Scale(damage, Level * 5);
            }
            return damage;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (IsMobileType(OppositionGroup.DarknessAndLight[0], target.GetType()) || target is PlayerMobile)
            {
                damage += Utility.RandomMinMax(1, Level);
            }
        }
    }
}
