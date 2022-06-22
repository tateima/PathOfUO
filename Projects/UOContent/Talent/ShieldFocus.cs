namespace Server.Talent
{
    public class ShieldFocus : BaseTalent
    {
        public ShieldFocus()
        {
            DisplayName = "Shield focus";
            Description = "Decreases damage taken by spells and attack while shield equipped.";
            AdditionalDetail = $"This damage is decreased by 2% per level. {PassiveDetail}";
            ImageID = 146;
            GumpHeight = 70;
            AddEndY = 65;
        }
    }
}
