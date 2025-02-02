using Server.Targeting;
using Server.Talent;
using Server.Mobiles;

namespace Server.Spells.Seventh
{
    public class FlameStrikeSpell : MagerySpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Flame Strike",
            "Kal Vas Flam",
            245,
            9042,
            Reagent.SpidersSilk,
            Reagent.SulfurousAsh
        );

        public FlameStrikeSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Seventh;

        public override bool DelayedDamage => true;

        public void Target(Mobile m)
        {
            if (CheckHSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                SpellHelper.CheckReflect((int)Circle, Caster, ref m);

                double damage;

                if (Core.AOS)
                {
                    damage = GetNewAosDamage(48, 1, 5, m);
                }
                else
                {
                    damage = Utility.Random(27, 22);

                    if (CheckResisted(m))
                    {
                        damage *= 0.6;

                        m.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                    }

                    damage *= GetDamageScalar(m);
                }

                int fire = 100;
                int cold = 0;
                int hue = 0;

                if (Caster is PlayerMobile playerCaster) {
                    BaseTalent.ApplyFrostFireEffect(playerCaster, ref fire, ref cold, ref hue, m);
                }

                m.FixedParticles(0x3709, 10, 3, 5052, hue, 0, EffectLayer.LeftFoot, 0);
                m.PlaySound(0x208);

                SpellHelper.Damage(this, m, damage, 0, 100, 0, 0, 0);
            }
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }
    }
}
