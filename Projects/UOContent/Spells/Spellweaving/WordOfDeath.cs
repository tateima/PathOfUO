using System;
using Server.Targeting;

namespace Server.Spells.Spellweaving
{
    public class WordOfDeathSpell : ArcanistSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new("Word of Death", "Nyraxle", -1);

        public WordOfDeathSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(3.5);

        public override double RequiredSkill => 80.0;
        public override int RequiredMana => 50;

        public void Target(Mobile m)
        {
            if (CheckHSequence(m))
            {
                var loc = m.Location;
                loc.Z += 50;

                m.PlaySound(0x211);
                m.FixedParticles(0x3779, 1, 30, 0x26EC, 0x3, 0x3, EffectLayer.Waist);

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, loc, m.Map),
                    new Entity(Serial.Zero, m.Location, m.Map),
                    0xF5F,
                    1,
                    0,
                    true,
                    false,
                    0x21,
                    0x3F,
                    0x251D,
                    0,
                    0,
                    EffectLayer.Head,
                    0
                );

                var percentage = 0.05 * FocusLevel;

                int damage;

                if (!m.Player && m.Hits / (double)m.HitsMax < percentage)
                {
                    damage = 300;
                }
                else
                {
                    var minDamage = (int)Caster.Skills.Spellweaving.Value / 5;
                    var maxDamage = (int)Caster.Skills.Spellweaving.Value / 3;
                    damage = Utility.RandomMinMax(minDamage, maxDamage);
                    var damageBonus = AosAttributes.GetValue(Caster, AosAttribute.SpellDamage);
                    if (m.Player && damageBonus > 15)
                    {
                        damageBonus = 15;
                    }

                    damage *= damageBonus + 100;
                    damage /= 100;
                }

                DarkAffinityPower(ref damage);

                SpellHelper.Damage(this, m, damage, 0, 0, 0, 0, 0, 100);
            }
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }
    }
}
