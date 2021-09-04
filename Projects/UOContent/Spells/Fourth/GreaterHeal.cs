using Server.Engines.ConPVP;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Talent;

namespace Server.Spells.Fourth
{
    public class GreaterHealSpell : MagerySpell, ISpellTargetingMobile
    {
        private static readonly SpellInfo m_Info = new(
            "Greater Heal",
            "In Vas Mani",
            204,
            9061,
            Reagent.Garlic,
            Reagent.Ginseng,
            Reagent.MandrakeRoot,
            Reagent.SpidersSilk
        );

        public GreaterHealSpell(Mobile caster, Item scroll = null) : base(caster, scroll, m_Info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fourth;

        public void Target(Mobile m)
        {
            if (m == null)
            {
                return;
            }

            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (m is BaseCreature creature && creature.IsAnimatedDead)
            {
                Caster.SendLocalizedMessage(1061654); // You cannot heal that which is not alive.
            }
            else if (m.IsDeadBondedPet)
            {
                Caster.SendLocalizedMessage(1060177); // You cannot heal a creature that is already dead!
            }
            else if (m is Golem)
            {
                Caster.LocalOverheadMessage(MessageType.Regular, 0x3B2, 500951); // You cannot heal that.
            }
            else if (m.Poisoned || MortalStrike.IsWounded(m))
            {
                Caster.LocalOverheadMessage(MessageType.Regular, 0x22, Caster == m ? 1005000 : 1010398);
            }
            else if (CheckBSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                // Algorithm: (40% of magery) + (1-10)

                var toHeal = (int)(Caster.Skills.Magery.Value * 0.4);
                toHeal += Utility.Random(1, 10);
                toHeal = (int)(toHeal * ReagentsScale());

                if (Caster is PlayerMobile player)
                {
                    BaseTalent lightAffinity = player.GetTalent(typeof(LightAffinity));
                    if (lightAffinity != null)
                    {
                        toHeal += AOS.Scale(toHeal, lightAffinity.ModifySpellMultiplier());
                    }
                }
                // m.Heal( toHeal, Caster );
                SpellHelper.Heal(toHeal, m, Caster);

                m.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                m.PlaySound(0x202);
            }

            FinishSequence();
        }

        public override bool CheckCast()
        {
            if (DuelContext.CheckSuddenDeath(Caster))
            {
                Caster.SendMessage(0x22, "You cannot cast this spell when in sudden death.");
                return false;
            }

            return base.CheckCast();
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTargetMobile(this, TargetFlags.Beneficial, Core.ML ? 10 : 12);
        }
    }
}
