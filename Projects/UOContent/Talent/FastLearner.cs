namespace Server.Talent
{
    public class FastLearner : BaseTalent
    {
        public FastLearner()
        {
            TalentDependencies = new[] { typeof(DivineIntellect) };
            DisplayName = "Fast learner";
            Description = "Increases your experience gain from sources by 10% per level.";
            AdditionalDetail = $"{PassiveDetail}";
            ImageID = 328;
            GumpHeight = 85;
            AddEndY = 80;
        }
    }
}
