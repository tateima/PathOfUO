using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class RangerCommand : BaseTalent, ITalent
    {

        public RangerCommand() : base()
        {
            TalentDependency = typeof(NatureAffinity);
            DisplayName = "Ranger command";
            Description = "Decreases stat and skill loss of tamed creatures by 1% per level.";
            ImageID = 188;
        }
        public override double ModifySpellScalar()
        {
            return (Level / 100);
        }
    }
}
