using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class SpellMind : BaseTalent, ITalent
    {
        public SpellMind() : base()
        {
            BlockedBy = new Type[] { typeof(PlanarShift) };
            TalentDependency = typeof(ManaShield);
            DisplayName = "Spell mind";
            Description = "Reduces damage loss from spells cast without reagents. Increases damage by spells with reagents.";
            ImageID = 39882;
        }

    }
}

