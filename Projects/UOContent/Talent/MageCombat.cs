using Server.Mobiles;
using Server.Items;
using System;
namespace Server.Talent
{
    public class MageCombatant : BaseTalent, ITalent
    {
        public MageCombatant() : base()
        {
            BlockedBy = new Type[] { typeof(ManaShield) };
            TalentDependency = typeof(FastLearner);
            DisplayName = "Mage combatant";
            Description = "Uses Eval Int instead of Tactics for combat damage. Allows weapon casting.";
            ImageID = 348;
            MaxLevel = 1;
            GumpHeight = 230;
            AddEndY = 145;
        }

    }
}
