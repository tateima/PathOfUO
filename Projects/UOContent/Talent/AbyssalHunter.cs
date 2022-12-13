namespace Server.Talent
{
    public class AbyssalHunter : BaseTalent
    {
        public AbyssalHunter()
        {
            BlockedBy = new[] { typeof(UndeadHunter) };
            TalentDependencies = new[] { typeof(ExperiencedHunter) };
            HasDamageAbsorptionEffect = true;
            DisplayName = "Abyssal hunter";
            Description = "Increases damage to abyssal and absorbs damage from them.";
            AdditionalDetail = $"The damage caused is 1-X where X is the talent level. The damage absorbed from their attacks is 5% per level. {PassiveDetail}";
            ImageID = 297;
            AddEndY = 90;
        }
        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (IsMobileType(OppositionGroup.ChaosAndOrder[1], attacker.GetType()))
            {
                damage -= AOS.Scale(damage, Level * 5);
            }
            return damage;
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (IsMobileType(OppositionGroup.ChaosAndOrder[1], target.GetType()))
            {
                damage += Utility.RandomMinMax(1, Level);
            }
        }
    }
}
