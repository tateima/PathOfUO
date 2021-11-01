using Server.Targeting;

namespace Server.Spells
{
    public interface ISpellTargetingMobile : ISpell
    {
        void Target(Mobile from);
    }

    public class SpellTargetMobile : Target, ISpellTarget
    {
        private readonly ISpellTargetingMobile m_Spell;

        public SpellTargetMobile(ISpellTargetingMobile spell, TargetFlags flags, int range = 12) :
            base(range, false, flags) => m_Spell = spell;

        public ISpell Spell => m_Spell;

        protected override void OnTarget(Mobile from, object o)
        {
            if (Blindness.BlindMobile(from)) {
                Mobile newTarget = Blindness.GetBlindTarget(from, 8);
                if (newTarget != null) {
                      m_Spell.Target(newTarget);
                }
            }
            m_Spell.Target(o as Mobile);
        }

        protected override void OnTargetFinish(Mobile from)
        {
            m_Spell?.FinishSequence();
        }
    }
}
