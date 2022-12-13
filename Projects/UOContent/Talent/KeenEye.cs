namespace Server.Talent
{
    public class KeenEye : BaseTalent
    {
        public KeenEye()
        {
            TalentDependencies = new[] { typeof(DivineDexterity) };
            DisplayName = "Keen eyes";
            Description = "Increased chance of finding special loot on corpses.";
            AdditionalDetail = $"Each level increases this chance by 1%.  There is a rare chance rich items will drop. {PassiveDetail}";
            ImageID = 123;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 80;
        }
        public override int ModifySpellMultiplier() => Level;
    }
}
