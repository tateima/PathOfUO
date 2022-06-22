namespace Server.Talent
{
    public class ExperiencedHunter : BaseTalent
    {
        public ExperiencedHunter()
        {
            DisplayName = "Experienced hunter";
            Description = "Increases damage to animals.";
            AdditionalDetail = $"The damage caused is 1-X where X is the talent level. {PassiveDetail}";
            ImageID = 164;
            AddEndY = 45;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.AnimalGroup, target.GetType()))
            {
                target.Damage(Utility.RandomMinMax(1, Level), attacker);
            }
        }
    }
}
