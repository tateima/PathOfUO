namespace Server.Talent
{
    public class ArachnidHunter : BaseTalent
    {
        public ArachnidHunter()
        {
            BlockedBy = new[] { typeof(ReptilianHunter) };
            TalentDependencies = new[] { typeof(ExperiencedHunter) };
            HasDamageAbsorptionEffect = true;
            DisplayName = "Arachnid hunter";
            Description = "Increases damage to arachnids and heals damage from them.";
            AdditionalDetail = $"The damage caused is 1-X where X is the talent level. The damage absorbed from their attacks is 5% per level. {PassiveDetail}";
            ImageID = 149;
            AddEndY = 90;
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (IsMobileType(OppositionGroup.ArachnidGroup, attacker.GetType()))
            {
                damage -= AOS.Scale(damage, Level * 5);
            }
            return damage;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (IsMobileType(OppositionGroup.ArachnidGroup, target.GetType()))
            {
                damage += Utility.RandomMinMax(1, Level);
            }
        }
    }
}
