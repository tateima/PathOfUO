namespace Server.Talent
{
    public class EfficientCarver : BaseTalent
    {
        public EfficientCarver()
        {
            TalentDependencies = new[] { typeof(ResourcefulHarvester) };
            DisplayName = "Carver";
            Description = "Chance to receive extra plank materials when carving.";
            AdditionalDetail = $"The chance for this increases by 3% per level. {PassiveDetail}";
            ImageID = 357;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 80;
            MaxLevel = 10;
        }

        public override int ModifySpellMultiplier() => Level * 3;
    }
}
