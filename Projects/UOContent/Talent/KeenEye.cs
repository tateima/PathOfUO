using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class KeenEye : BaseTalent, ITalent
    {
        public KeenEye() : base()
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
