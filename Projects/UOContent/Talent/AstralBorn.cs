namespace Server.Talent
{
    public class AstralBorn : BaseTalent
    {
        public AstralBorn()
        {
            DisplayName = "Astral born";
            Description = "Decreases your planar exhaustion by 15 minutes.";
            ImageID = 401;
            GumpHeight = 85;
            MaxLevel = 1;
            AddEndY = 70;
        }

        public override int ModifySpellMultiplier() => Level * 30;
    }
}
