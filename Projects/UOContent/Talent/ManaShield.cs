using Server.Mobiles;
using Server.Spells;
using Server.Spells.Necromancy;
using System;

namespace Server.Talent
{
    public class ManaShield : BaseTalent, ITalent
    {
        public ManaShield() : base()
        {
            BlockedBy = new Type[] { typeof(MageCombatant) };
            TalentDependency = typeof(FastLearner);
            CanBeUsed = true;
            DisplayName = "Mana shield";
            MaxLevel = 1;
            Description = "Absorbs damage with mana. Does not work in Wraith Form.";
            ImageID = 155;
            AddEndY = 95;
        }

        public override void CheckDefenseEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (Activated)
            {
                TransformContext context = TransformationSpellHelper.GetContext(attacker);
                // dont apply the effect if Wraith Form 
                if (context?.Type != typeof(WraithFormSpell) && defender.Mana > 0)
                {
                    // restore hits first
                    defender.Hits += damage;
                    // get mana damage differential
                    int manaDmgDiff = damage;
                    if (defender.Mana < damage)
                    {
                        manaDmgDiff = defender.Mana;
                        Activated = false;
                        int hitReDamage = damage - manaDmgDiff;
                        defender.Hits -= hitReDamage;
                    }
                    defender.Mana -= manaDmgDiff;
                }
            }   
        }

        public override void OnUse(Mobile mobile)
        {
            if (mobile.Mana < 1) { 
               mobile.SendMessage("You cannot use a mana shield at this time.");
            }
             else if (!Activated)
            {
                Activated = true;
                mobile.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                mobile.PlaySound(0x1E8);
            } 
        }

    }
}
