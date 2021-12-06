namespace Server.Talent
{
    public class ShieldFocus : BaseTalent
    {
        public ShieldFocus()
        {
            DisplayName = "Shield focus";
            Description = "Decreases damage taken by spells and attack while shield equipped.";
            ImageID = 146;
            GumpHeight = 70;
            AddEndY = 65;
        }
    }
}
