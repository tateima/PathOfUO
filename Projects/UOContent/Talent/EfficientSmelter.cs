namespace Server.Talent
{
    public class EfficientSmelter : BaseTalent
    {
        public EfficientSmelter()
        {
            TalentDependencies = new[] { typeof(ResourcefulHarvester) };
            DisplayName = "Metal worker";
            Description = "Chance to receive extra resources when smelting.";
            AdditionalDetail = $"The chance for this increases by 3% per level. {PassiveDetail}";
            ImageID = 356;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 90;
            MaxLevel = 10;
        }
        public override int ModifySpellMultiplier() => Level * 3;
    }
}
