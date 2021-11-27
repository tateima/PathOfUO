using Server.Targeting;

namespace Server.Spells
{
    public interface ISpellTargetingMobile : ISpell
    {
        void Target(Mobile from);
    }

    public class SpellTargetMobile : Target, ISpellTarget
    {
        private readonly ISpellTargetingMobile _spell;

        public SpellTargetMobile(ISpellTargetingMobile spell, TargetFlags flags, int range = 12) :
            base(range, false, flags) => _spell = spell;

        public ISpell Spell => _spell;

        protected override void OnTarget(Mobile from, object o)
        {
            if (Blindness.BlindMobile(from)) {
                Mobile newTarget = Blindness.GetBlindTarget(from, 8);
                if (newTarget != null) {
                      _spell.Target(newTarget);
                }
            }
             _spell.Target(o as Mobile);
        }

        protected override void OnTargetFinish(Mobile from)
        {
            _spell?.FinishSequence();
        }
    }
}
