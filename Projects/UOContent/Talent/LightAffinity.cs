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
            AdditionalDetail = $"Each level increases the amount by 2%. Light affinity also passively improves guardian of light and holy avenger. {PassiveDetail}";
            ImageID = 116;
            GumpHeight = 70;
            AddEndY = 65;
            AddEndAdditionalDetailsY = 100;
        }

        public override double ModifySpellScalar() => Level / 100.0 * 2;

        public override bool IgnoreTalentBlock(Mobile mobile) => mobile.Skills.Spellweaving.Value > 0.0;
    }
}
