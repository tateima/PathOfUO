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
