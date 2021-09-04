using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class GiantsHeritage : BaseTalent, ITalent
    {
        public GiantsHeritage() : base()
        {
            TalentDependency = typeof(DivineStrength);
            DisplayName = "Giant's Heritage";
            Description = "Increases max hit points and stamina per level. The more stamina you have the more damage you do.";
            ImageID = 30036;
        }
        public override int CalculateResetValue(int value)
        {
            if (Activated)
            {
                // been activated once before, so remove last levels values
                value -= (Level - 1) * 10;
            }
            return value;
        }
        public override int CalculateNewValue(int value)
        {
            Activated = true;
            return value += (Level) * 10;
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            int extraDamage =  (int)(attacker.Stam * SpecialDamageScalar);
            target.Damage(extraDamage, attacker);
            attacker.Stam -= 10;
        }
    }
}
