namespace Server.Talent
{
    public class UndeadHunter : BaseTalent
    {
        public UndeadHunter()
        {
            BlockedBy = new[] { typeof(AbyssalHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            HasDamageAbsorptionEffect = true;
            DisplayName = "Undead hunter";
            Description = "Increases damage to undead and heals damage from them.";
            ImageID = 143;
            AddEndY = 95;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.UndeadGroup, target.GetType()))
            {
                target.Damage(Utility.RandomMinMax(1, Level), attacker);
            }
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (IsMobileType(OppositionGroup.UndeadGroup, attacker.GetType()))
            {
                damage -= AOS.Scale(damage, Level * 5);
            }
            return damage;
        }
    }
}
