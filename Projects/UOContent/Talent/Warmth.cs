namespace Server.Talent
{
    public class Warmth : BaseTalent
    {
        public Warmth()
        {
            TalentDependencies = new[] { typeof(DragonAspect) };
            DisplayName = "Warmth";
            Description = "Reduces chance to be frozen by 15% per level.";
            AdditionalDetail = $"{PassiveDetail}";
            ImageID = 141;
            GumpHeight = 85;
            AddEndY = 80;
            MaxLevel = 3;
        }

        public override int ModifySpellMultiplier() => Level * 15;
    }
}
