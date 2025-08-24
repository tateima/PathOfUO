using System;
using Server.Targeting;
using Server.Mobiles;
using Server.Talent;

namespace Server.Spells.Sixth
{
    public class ExplosionSpell : MagerySpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Explosion",
            "Vas Ort Flam",
            230,
            9041,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot
        );

        public ExplosionSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Sixth;

        public override Type[] DelayedDamageSpellFamilyStacking => AOSNoDelayedDamageStackingSelf;

        public override bool DelayedDamage => false;

        public void Target(Mobile m)
        {
            if (Core.SA && HasDelayedDamageContext(m))
            {
                DoHurtFizzle();
                return;
            }

            if (Caster.CanBeHarmful(m) && CheckSequence())
            {
                Mobile defender = m;

                SpellHelper.Turn(Caster, m);
                SpellHelper.CheckReflect((int)Circle, Caster, ref m);

                new InternalTimer(this, Caster, defender, m).Start();
            }
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }

        private class InternalTimer : Timer
        {
            private readonly Mobile _attacker;
            private readonly Mobile _defender;
            private readonly MagerySpell _spell;
            private readonly Mobile _target;

            public InternalTimer(MagerySpell spell, Mobile attacker, Mobile defender, Mobile target)
                : base(TimeSpan.FromSeconds(Core.AOS ? 3.0 : 2.5))
            {
                _spell = spell;
                _attacker = attacker;
                _defender = defender;
                _target = target;

                _spell?.StartDelayedDamageContext(_attacker, this);
            }

            protected override void OnTick()
            {
                if (_defender.Deleted || !_defender.Alive)
                {
                    return;
                }

                if (_attacker.HarmfulCheck(_defender))
                {
                    double damage;

                    if (Core.AOS)
                    {
                        damage = _spell.GetNewAosDamage(30, 1, 5, _defender);
                    }
                    else
                    {
                        damage = Utility.Random(23, 22);

                        if (_spell.CheckResisted(_target))
                        {
                            damage *= 0.75;

                            _target.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                        }

                        damage *= _spell.GetDamageScalar(_target);
                    }

                    int fire = 100;
                    int cold = 0;
                    int hue = 0;

                    if (_attacker is PlayerMobile playerCaster) {
                        BaseTalent.ApplyFrostFireEffect(playerCaster, ref fire, ref cold, ref hue, _target);
                    }

                    _target.FixedParticles(0x36BD, 20, 10, 5044, hue, 0, EffectLayer.Head, 0);
                    _target.PlaySound(0x307);

                    SpellHelper.Damage(_spell, _target, damage, 0, fire, cold, 0, 0);
                }

                _spell?.RemoveDelayedDamageContext(_attacker);
            }
        }
    }
}
