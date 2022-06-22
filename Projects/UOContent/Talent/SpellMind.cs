namespace Server.Talent
{
    public class SpellMind : BaseTalent
    {
        public SpellMind()
        {
            BlockedBy = new[] { typeof(PlanarShift) };
            TalentDependency = typeof(ManaShield);
            DisplayName = "Spell mind";
            Description = "Reduces damage loss from spells cast without reagents. Increases damage by spells with reagents.";
            AdditionalDetail = $"{PassiveDetail}";
            ImageID = 128;
        }
    }
}
