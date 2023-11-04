using ModernUO.Serialization;
using Server.Talent;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items;

[Flippable(0x104F, 0x1050)]
[SerializationGenerator(0, false)]
public partial class ClockParts : Item
{
    [Constructible]
    public ClockParts(int amount = 1) : base(0x104F)
    {
        Stackable = true;
        Amount = amount;
        Weight = 1.0;
    }
    public override void OnDoubleClick(Mobile from)
    {
        base.OnDoubleClick(from);
        if (from is PlayerMobile player)
        {
            BaseTalent inventive = player.GetTalent(typeof(Inventive));
            if (inventive != null)
            {
                player.SendMessage("What do you wish to use these spare clock parts on?");
                from.Target = new InternalTarget(inventive);
            }
        }
    }
    private class InternalTarget : Target
    {
        private readonly BaseTalent m_Inventive;

        public InternalTarget(BaseTalent inventive) : base(
            2,
            false,
            TargetFlags.None
        ) =>
            m_Inventive = inventive;

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is AutomatonConstruct automaton)
            {
                if (automaton.Hits < automaton.HitsMax)
                {
                    automaton.Heal(Utility.Random(m_Inventive.ModifySpellMultiplier()));
                    from.PlaySound(0x241);
                }
                else
                {
                    from.SendMessage("The automaton is not damaged");
                }
            }
            else
            {
                from.SendMessage("You cannot use these clock parts on this");
            }
        }
    }
}
