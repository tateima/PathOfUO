namespace Server.Talent
{
    public class StrongTools : BaseTalent
    {
        public StrongTools()
        {
            TalentDependency = typeof(WarCraftFocus);
            DisplayName = "Strong tools";
            Description = "Your tools are less likely to lose durability on use.";
            ImageID = 199;
            AddEndY = 85;
        }

        public bool CheckSave() => Utility.Random(100) < Level * 5;
    }
}
