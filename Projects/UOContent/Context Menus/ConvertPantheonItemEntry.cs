using Server.Mobiles;
using Server.Pantheon;
using Server.Targeting;

namespace Server.ContextMenus
{
    public class ConvertPantheonItemEntry : ContextMenuEntry
    {
        private readonly PlayerMobile _from;
        public ConvertPantheonItemEntry(PlayerMobile from) : base(1116796, 1) => _from = from;

        public override void OnClick(Mobile from, IEntity target)
        {
            foreach (var mobile in _from.GetMobilesInRange(3))
            {
                if (Core.AOS && !mobile.InLOS(_from))
                {
                    continue;
                }

                if (mobile is Oracle oracle)
                {
                    _from.Target = new InternalTarget(_from, oracle);
                    return;
                }
            }
            _from.SendMessage("You cannot convert an item here, you need to be near an Oracle.");
        }
        private class InternalTarget : Target
        {
            private readonly PlayerMobile _player;
            private readonly Oracle _oracle;

            public InternalTarget(PlayerMobile player, Oracle oracle) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                _player = player;
                _oracle = oracle;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {

                if (targeted is IPantheonItem pantheonItem)
                {
                    Deity.Alignment itemAlignment = Deity.AlignmentFromString(pantheonItem.AlignmentRaw);
                    if (!Equals(itemAlignment, Deity.Alignment.None) && !Equals(itemAlignment, _player.Alignment) && _player.DeityPoints >= 100)
                    {
                        _player.DeityPoints -= 100;
                        pantheonItem.AlignmentRaw = _player.Alignment.ToString();
                        _player.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                        _player.PlaySound(0x1E3);
                        _player.SendMessage("The alignment of this item has now been converted to your own.");
                    }
                    else
                    {
                        _oracle.SayTo(_player, "Thou cannot convert this item's alignment.");
                    }
                }
            }
        }
    }
}
