namespace Server.Talent
{
    public class KeenEye : BaseTalent
    {
        public KeenEye()
        {
            TalentDependency = typeof(DivineDexterity);
            DisplayName = "Keen eyes";
            Description = "Increased chance of finding special loot on corpses.";
            ImageID = 123;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 80;
        }
    }
}
