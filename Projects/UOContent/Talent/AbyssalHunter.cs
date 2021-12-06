namespace Server.Talent
{
    public class AbyssalHunter : BaseTalent
    {
        public AbyssalHunter()
        {
            BlockedBy = new[] { typeof(UndeadHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Abyssal hunter";
            Description = "Increases damage to abyssal and heals damage from them.";
            ImageID = 297;
            AddEndY = 90;
        }

        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.AbyssalGroup, target.GetType()))
            {
                defender.Heal(AOS.Scale(damage, Level));
            }
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.AbyssalGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }
    }
}
