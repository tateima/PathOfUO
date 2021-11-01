using System;

namespace Server.Talent
{
    public class Meld : BaseTalent, ITalent
    {
        public Meld() : base()
        {
            TalentDependency = typeof(Enchant);
            DisplayName = "Meld";
            Description = "Can meld elemental shards into items to increase their power.";
            ImageID = 398;
            MaxLevel = 1;
            GumpHeight = 85;
            AddEndY = 80;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            return Disenchant.CanDisenchant(mobile, 90);
        }
    }
}

