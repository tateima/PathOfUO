using Server.Spells.First;
using Server.Spells.Fourth;
using Server.Spells.Spellweaving;
using Server.Spells.Third;

namespace Server.Talent
{
    public class LightAffinity : BaseTalent
    {
        public LightAffinity()
        {
            BlockedBy = new[] { typeof(DarkAffinity) };
            RequiredSpell = new[]
                { typeof(GreaterHealSpell), typeof(HealSpell), typeof(BlessSpell), typeof(GiftOfRenewalSpell) };
            DisplayName = "Light affinity";
            Description = "Increases healing done by spells.";
            ImageID = 116;
            GumpHeight = 70;
            AddEndY = 65;
        }

        public override double ModifySpellScalar() => Level / 100.0;

        public override bool IgnoreTalentBlock(Mobile mobile) => mobile.Skills.Spellweaving.Value > 0.0;
    }
}
