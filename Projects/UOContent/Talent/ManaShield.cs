using System;
using Server.Spells;
using Server.Spells.Necromancy;

namespace Server.Talent
{
    public class ManaShield : BaseTalent
    {
        public ManaShield()
        {
            BlockedBy = new[] { typeof(MageCombatant) };
            TalentDependency = typeof(FastLearner);
            CanBeUsed = true;
            HasDefenseEffect = true;
            DisplayName = "Mana shield";
            MaxLevel = 1;
            Description = "Absorbs damage with mana. Does not work in Wraith Form. Requires 75+ magery.";
            ImageID = 155;
            AddEndY = 95;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills[SkillName.Magery].Base >= 75;

        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (Activated)
            {
                var context = TransformationSpellHelper.GetContext(target);
                // dont apply the effect if Wraith Form
                if (context?.Type != typeof(WraithFormSpell) && defender.Mana > 10)
                {
                    // restore hits first
                    defender.Hits += damage;
                    // get mana damage differential
                    var manaDmgDiff = damage;
                    if (defender.Mana < damage)
                    {
                        manaDmgDiff = defender.Mana;
                        Activated = false;
                        var hitReDamage = damage - manaDmgDiff;
                        defender.Hits -= hitReDamage;
                    }

                    defender.Mana -= manaDmgDiff;
                    if (defender.Mana < 10)
                    {
                        Activated = false;
                    }
                }
            }
        }

        public override void OnUse(Mobile from)
        {
            if (from.Mana < 10)
            {
                from.SendMessage("You cannot use a mana shield at this time.");
            }
            else if (!OnCooldown)
            {
                Activated = true;
                OnCooldown = true;
                from.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                from.PlaySound(0x1E8);
                Timer.StartTimer(TimeSpan.FromSeconds(180 - Level * 5), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}
