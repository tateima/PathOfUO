namespace Server.Talent
{
    public class ArachnidHunter : BaseTalent
    {
        public ArachnidHunter()
        {
            BlockedBy = new[] { typeof(ReptilianHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Arachnid hunter";
            Description = "Increases damage to arachnids and heals damage from them.";
            ImageID = 149;
            AddEndY = 100;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.ArachnidGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }

        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.ArachnidGroup, target.GetType()))
            {
                defender.Heal(AOS.Scale(damage, Level));
            }
        }
    }
}
