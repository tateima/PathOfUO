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
            Description = "Uses Evaluating Intelligence instead of Tactics for combat damage. Allows casting while holding weapons.";
            ImageID = 39886;
            MaxLevel = 1;
        }

    }
}
