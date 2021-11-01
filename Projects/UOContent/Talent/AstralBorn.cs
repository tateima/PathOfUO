
namespace Server.Talent
{
    public class AstralBorn : BaseTalent, ITalent
    {
        public AstralBorn() : base()
        {
            DisplayName = "Astral born";
            Description = "Decreases your planar exhaustion by 30 minutes.";
            ImageID = 401;
            GumpHeight = 85;
            MaxLevel = 1;
            AddEndY = 70;
        }
        public override int ModifySpellMultiplier()
        {
            return Level * 30;
        }
    }
}
