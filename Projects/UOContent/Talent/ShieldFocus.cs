namespace Server.Talent
{
    public class ShieldFocus : BaseTalent
    {
        public ShieldFocus()
        {
            DisplayName = "Shield focus";
            Description = "Unlocks proficiencies with shields.";
            AdditionalDetail = $"Can now use shields. Decreases damage taken by spells and attack while shield equipped. This damage is decreased by 5% per level. {PassiveDetail}";
            ImageID = 146;
            GumpHeight = 70;
            AddEndY = 65;
        }
    }
}
