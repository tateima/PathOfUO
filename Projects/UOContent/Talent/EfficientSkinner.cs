namespace Server.Talent
{
    public class EfficientSkinner : BaseTalent
    {
        public EfficientSkinner()
        {
            TalentDependency = typeof(ResourcefulHarvester);
            DisplayName = "Skinner";
            Description = "Chance to receive extra hide material when skinning creatures.";
            AdditionalDetail = $"The chance for this increases by 3% per level. {PassiveDetail}";
            ImageID = 359;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 85;
            MaxLevel = 10;
        }
        public override int ModifySpellMultiplier() => Level * 3;
    }
}
