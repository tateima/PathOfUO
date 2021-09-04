using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class SonicAffinity : BaseTalent, ITalent
    {
        public SonicAffinity() : base()
        {
            DisplayName = "Sonic affinity";
            Description = "Increases effectiveness of provocation, peacemaking and discordance.";
            ImageID = 30234;
        }

        public override int ModifySpellMultiplier()
        {
            return Level * 2; // 2 per point
        }

        public override double ModifySpellScalar()
        {
            return (Level / 100); // 1% per point
        }

    }
}
