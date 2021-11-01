using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class SummonerCommand : BaseTalent, ITalent
    {
        public SummonerCommand() : base()
        {
            TalentDependency = typeof(DarkAffinity);
            DisplayName = "Summoner command";
            Description = "Increases power of minions and summoned creatures.";
            ImageID = 136;
            GumpHeight = 85;
            AddEndY = 75;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            return mobile.Skills[SkillName.Necromancy].Base >= 70;
        }
    }
}
