namespace Server.Talent
{
    public class ExperiencedHunter : BaseTalent
    {
        public ExperiencedHunter()
        {
            DisplayName = "Experienced hunter";
            Description = "Increases damage to animals.";
            ImageID = 164;
            AddEndY = 45;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.AnimalGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }
    }
}
