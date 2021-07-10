using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class SummonerCommand : BaseTalent, ITalent
    {
        public SummonerCommand() : base()
        {
            TalentDependency = typeof(DarkAffinity);
            DisplayName = "Summoner command";
            Description = "Increases power of minions and summoned creatures.";
            ImageID = 49987;
        }

    }
}
