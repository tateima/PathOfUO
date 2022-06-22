namespace Server.Talent
{
    public class MageCombatant : BaseTalent
    {
        public MageCombatant()
        {
            BlockedBy = new[] { typeof(ManaShield) };
            TalentDependency = typeof(FastLearner);
            DisplayName = "Mage combatant";
            Description = "Uses mage based skills to calculate combat damage and allows weapon casting.";
            AdditionalDetail = $"Instead of using traditional tactics, evaluating intelligence is used. {PassiveDetail}";
            ImageID = 348;
            MaxLevel = 1;
            GumpHeight = 230;
            AddEndY = 130;
        }
    }
}
