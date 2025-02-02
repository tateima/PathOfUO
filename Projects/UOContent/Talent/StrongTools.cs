namespace Server.Talent
{
    public class StrongTools : BaseTalent
    {
        public StrongTools()
        {
            TalentDependencies = new[] { typeof(WarCraftFocus) };
            DisplayName = "Strong tools";
            Description = "Your tools are less likely to lose durability on use.";
            AdditionalDetail = $"The chance for this save increases by 5% per level. {PassiveDetail}";
            ImageID = 199;
            AddEndY = 85;
        }

        public bool CheckSave() => Utility.Random(100) < Level * 5;
    }
}
