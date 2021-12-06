namespace Server.Talent
{
    public class Riposte : BaseTalent
    {
        public Riposte()
        {
            RequiredWeaponSkill = SkillName.Fencing;
            TalentDependency = typeof(FencingFocus);
            DisplayName = "Riposte";
            Description = "Chance to deal damage to them if enemy misses.";
            ImageID = 339;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckDefenderMissEffect(Mobile attacker, Mobile target)
        {
            // 5% chance
            if (Utility.Random(100) < Level)
            {
                // max 10 damage (2 per level)
                attacker.Damage(Level * 2, target);
            }
        }
    }
}
