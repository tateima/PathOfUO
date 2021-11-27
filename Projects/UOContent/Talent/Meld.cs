using System;

namespace Server.Talent
{
    public class Meld : BaseTalent, ITalent
    {
        public Meld() : base()
        {
            TalentDependency = typeof(Enchant);
            DisplayName = "Meld";
            Description = "Can meld elemental shards into items to increase their power. Requires at least one combat and one crafting skill above 70+.";
            ImageID = 398;
            MaxLevel = 1;
            GumpHeight = 85;
            AddEndY = 110;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            return Disenchant.CanDisenchant(mobile, 90);
        }
    }
}

