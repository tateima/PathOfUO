using System;
using Server.Targeting;
using Server.Talent;
using Server.Mobiles;

namespace Server.Spells.First
{
    public class MagicArrowSpell : MagerySpell, ISpellTargetingMobile
    {
        private static readonly SpellInfo _info = new(
            "Magic Arrow",
            "In Por Ylem",
            212,
            9041,
            Reagent.SulfurousAsh
        );

        public MagicArrowSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;

        public override Type[] DelayedDamageSpellFamilyStacking => AOSNoDelayedDamageStackingSelf;

        public override bool DelayedDamage => true;

        public void Target(Mobile m)
        {
            if (CheckHSequence(m))
            {
                var source = Caster;

                SpellHelper.Turn(source, m);

                if (Core.SA && HasDelayedDamageContext(m))
                {
                    DoHurtFizzle();
                    return;
                }

                SpellHelper.CheckReflect((int)Circle, ref source, ref m);

                double damage;

                if (Core.AOS)
                {
                    damage = GetNewAosDamage(10, 1, 4, m);
                }
                else
                {
                    damage = Utility.Random(4, 4);

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
                if (Caster is PlayerMobile player)
                {
                    BaseTalent fireAffinity = player.GetTalent(typeof(FireAffinity));
                    if (fireAffinity != null)
                    {
                        damage += fireAffinity.ModifySpellMultiplier();
                    }
                    BaseTalent frostFire = player.GetTalent(typeof(FrostFire));
                    if (frostFire != null && fire > 0) {
                        ((FrostFire)frostFire).ModifyFireSpell(ref fire, ref cold, m, hue: ref hue);
                    }
                }
                source.MovingParticles(m, 0x36D4, 5, 0, false, false, hue, 0, 3006, 0, 0, 0);
                source.PlaySound(0x1E5);
                SpellHelper.Damage(this, m, damage, 0, fire, cold, 0, 0);
            }

            FinishSequence();
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTargetMobile(this, TargetFlags.Harmful, Core.ML ? 10 : 12);
        }
    }
}
