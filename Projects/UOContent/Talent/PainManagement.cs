using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class PainManagement : BaseTalent, ITalent
    {
        public PainManagement() : base()
        {
            TalentDependency = typeof(BoneBreaker);
            DisplayName = "Pain management";
            Description = "Decrease bleeding effects and poison damage.";
            ImageID = 30104;
            GumpHeight = 85;
            AddEndY = 80;
        }
        public virtual int ModifySpellMultiplier()
        {
            return Level * 2; // 2% per point
        }
    }
}
