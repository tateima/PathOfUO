namespace Server.Talent
{
    public class SpellMind : BaseTalent
    {
        public SpellMind()
        {
            BlockedBy = new[] { typeof(PlanarShift) };
            TalentDependencies = new[] { typeof(ManaShield) };
            DisplayName = "Spell mind";
            Description = "Reduces damage loss from spells cast without reagents. Increases damage by spells with reagents.";
            AdditionalDetail = $"{PassiveDetail}";
            ImageID = 128;
        }

        public override double ModifySpellScalar() => Level / 100.0 * 3;

        public override int ModifySpellMultiplier() => Level * 3;
    }
}
