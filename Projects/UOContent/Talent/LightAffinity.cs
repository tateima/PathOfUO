using Server.Mobiles;
using Server.Spells.First;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Spellweaving;
using System;

namespace Server.Talent
{
    public class LightAffinity : BaseTalent, ITalent
    {
        public LightAffinity() : base()
        {
            BlockedBy = new Type[] { typeof(DarkAffinity) };
            RequiredSpell = new Type[] { typeof(GreaterHealSpell), typeof(HealSpell), typeof(BlessSpell), typeof(GiftOfRenewalSpell) };
            DisplayName = "Light affinity";
            Description = "Increases healing done by spells.";
            ImageID = 116;
            GumpHeight = 70;
            AddEndY = 65;
        }
        public override double ModifySpellScalar()
        {
            return (Level / 100);
        }

        public override bool IgnoreTalentBlock(Mobile mobile)
        {
            return mobile.Skills.Spellweaving.Value > 0.0;
        }
    }
}
