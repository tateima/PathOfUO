using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class SpellMind : BaseTalent, ITalent
    {
        public SpellMind() : base()
        {
            TalentDependency = typeof(PlanarShift);
            DisplayName = "Spell mind";
            Description = "Reduces damage loss from spells cast without reagents.";
            ImageID = 39882;
        }

    }
}
