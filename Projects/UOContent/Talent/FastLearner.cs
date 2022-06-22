namespace Server.Talent
{
    public class FastLearner : BaseTalent
    {
        public FastLearner()
        {
            TalentDependency = typeof(DivineIntellect);
            DisplayName = "Fast learner";
            Description = "Increases your experience gain from sources by 2% per level.";
            AdditionalDetail = $"{PassiveDetail}";
            ImageID = 328;
            GumpHeight = 85;
            AddEndY = 80;
        }
    }
}
