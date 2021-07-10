using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class MasterOfDeath : BaseTalent, ITalent
    {
        public MasterOfDeath() : base()
        {
            BlockedBy = new Type[] { typeof(GreaterFireElemental) };
            TalentDependency = typeof(SummonerCommand);
            DisplayName = "Master of death";
            Description = "Chance to summon nearby corpses as allies killed by player.";
            ImageID = 39863;
        }

    }
}
