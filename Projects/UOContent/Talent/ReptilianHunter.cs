namespace Server.Talent
{
    public class ReptilianHunter : BaseTalent
    {
        public ReptilianHunter()
        {
            BlockedBy = new[] { typeof(ArachnidHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            HasDamageAbsorptionEffect = true;
            DisplayName = "Reptilian hunter";
            Description = "Increases damage to reptiles and heals damage from them.";
            AdditionalDetail = $"The damage caused is 1-X where X is the talent level. The damage absorbed from their attacks is 5% per level. {PassiveDetail}";
            ImageID = 187;
            AddEndY = 95;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.ReptilianGroup, target.GetType()))
            {
                target.Damage(Utility.RandomMinMax(1, Level), attacker);
            }
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (IsMobileType(OppositionGroup.ReptilianGroup, attacker.GetType()))
            {
                damage -= AOS.Scale(damage, Level * 5);
            }
            return damage;
        }
    }
}
