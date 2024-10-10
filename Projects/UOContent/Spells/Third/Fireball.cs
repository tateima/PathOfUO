using Server.Targeting;
using Server.Mobiles;
using Server.Talent;

namespace Server.Spells.Third
{
    public class FireballSpell : MagerySpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Fireball",
            "Vas Flam",
            203,
            9041,
            Reagent.BlackPearl
        );

        public FireballSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Third;

        public override bool DelayedDamage => true;

        public void Target(Mobile m)
        {
            if (CheckHSequence(m))
            {
                var source = Caster;

                SpellHelper.Turn(source, m);

                SpellHelper.CheckReflect((int)Circle, ref source, ref m);

                double damage;

                if (Core.AOS)
                {
                    damage = GetNewAosDamage(19, 1, 5, m);
                }
                else
                {
                    damage = Utility.Random(10, 7);

                    if (CheckResisted(m))
                    {
                        damage *= 0.75;

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


                source.MovingParticles(m, 0x36D4,7, 0, false, true, hue, 0, 9502, 4019, 0x160, 0);
                source.PlaySound(Core.AOS ? 0x15E : 0x44B);

                SpellHelper.Damage(this, m, damage, 0, fire, cold, 0, 0);
            }
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }
    }
}
