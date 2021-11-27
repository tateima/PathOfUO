using Server.Engines.ConPVP;
using Server.Targeting;
using Server.Talent;
using Server.Mobiles;

namespace Server.Spells.Third
{
    public class BlessSpell : MagerySpell, ISpellTargetingMobile
    {
        private static readonly SpellInfo _info = new(
            "Bless",
            "Rel Sanct",
            203,
            9061,
            Reagent.Garlic,
            Reagent.MandrakeRoot
        );

        public BlessSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Third;

        public void Target(Mobile m)
        {
            if (CheckBSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                SpellHelper.AddStatBonus(Caster, m, StatType.Str);
                SpellHelper.DisableSkillCheck = true;
                SpellHelper.AddStatBonus(Caster, m, StatType.Dex);
                SpellHelper.AddStatBonus(Caster, m, StatType.Int);
                SpellHelper.DisableSkillCheck = false;

                m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                m.PlaySound(0x1EA);

                var percentage = (int)(SpellHelper.GetOffsetScalar(Caster, m, false) * 100);
                if (Caster is PlayerMobile player)
                {
                    BaseTalent lightAffinity = player.GetTalent(typeof(LightAffinity));
                    if (lightAffinity != null)
                    {
                        percentage += lightAffinity.Level;
                    }
                }
                var length = SpellHelper.GetDuration(Caster, m);

                var args = $"{percentage}\t{percentage}\t{percentage}";

                BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Bless, 1075847, 1075848, length, m, args));
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
