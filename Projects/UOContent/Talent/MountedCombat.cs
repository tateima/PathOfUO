using Server.Mobiles;
using Server.Items;
using System;
namespace Server.Talent
{
    public class MountedCombat : BaseTalent, ITalent
    {
        public MountedCombat() : base()
        {
            DisplayName = "Mounted combat";
            Description = "Reduces chance of being dismounted by normal attacks by 2% per level.";
            ImageID = 382;
            MaxLevel = 7;
            GumpHeight = 230;
            AddEndY = 80;
        }
        public override int ModifySpellMultiplier()
        {
            return Level * 2;
        }
    }
}
