namespace Server.Talent
{
    public class UndeadHunter : BaseTalent
    {
        public UndeadHunter()
        {
            BlockedBy = new[] { typeof(AbyssalHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Undead hunter";
            Description = "Increases damage to undead and heals damage from them.";
            ImageID = 143;
            AddEndY = 95;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.UndeadGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }

        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.UndeadGroup, target.GetType()))
            {
                defender.Heal(AOS.Scale(damage, Level));
            }
        }
    }
}
