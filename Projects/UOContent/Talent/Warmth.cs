namespace Server.Talent
{
    public class Warmth : BaseTalent
    {
        public Warmth()
        {
            TalentDependency = typeof(DragonAspect);
            DisplayName = "Warmth";
            Description = "Reduces chance to be frozen by 15% per level.";
            ImageID = 387;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override int ModifySpellMultiplier() => Level * 15;
    }
}
