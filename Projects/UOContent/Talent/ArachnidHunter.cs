namespace Server.Talent
{
    public class ArachnidHunter : BaseTalent
    {
        public ArachnidHunter()
        {
            BlockedBy = new[] { typeof(ReptilianHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            HasDamageAbsorptionEffect = true;
            DisplayName = "Arachnid hunter";
            Description = "Increases damage to arachnids and heals damage from them.";
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

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.ArachnidGroup, target.GetType()))
            {
                target.Damage(Utility.RandomMinMax(1, Level), attacker);
            }
        }
    }
}
