namespace Server.Talent
{
    public class MageCombatant : BaseTalent
    {
        public MageCombatant()
        {
            BlockedBy = new[] { typeof(ManaShield) };
            TalentDependency = typeof(FastLearner);
            DisplayName = "Mage combatant";
            Description = "Uses Eval Int instead of Tactics for combat damage. Allows weapon casting.";
            ImageID = 348;
            MaxLevel = 1;
            GumpHeight = 230;
            AddEndY = 130;
        }
    }
}
