namespace Server.Talent
{
    public class AbyssalHunter : BaseTalent
    {
        public AbyssalHunter()
        {
            BlockedBy = new[] { typeof(UndeadHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            HasDamageAbsorptionEffect = true;
            DisplayName = "Abyssal hunter";
            Description = "Increases damage to abyssal and heals damage from them.";
            ImageID = 297;
            AddEndY = 90;
        }
        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (IsMobileType(OppositionGroup.AbyssalGroup, attacker.GetType()))
            {
                damage -= AOS.Scale(damage, Level * 5);
            }
            return damage;
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.AbyssalGroup, target.GetType()))
            {
                target.Damage(Utility.RandomMinMax(1, Level), attacker);
            }
        }
    }
}
