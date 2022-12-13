namespace Server.Talent
{
    public class MountedCombat : BaseTalent
    {
        public MountedCombat()
        {
            DisplayName = "Mounted combat";
            Description = "Reduces chance of being dismounted by normal attacks by 5% per level.";
            ImageID = 382;
            MaxLevel = 7;
            GumpHeight = 230;
            AddEndY = 80;
        }

        public override int ModifySpellMultiplier() => Level * 2;
    }
}
