using Server.Gumps;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.ContextMenus
{
    public class TalentBarEntry : ContextMenuEntry
    {
        private readonly PlayerMobile _from;

        public TalentBarEntry(PlayerMobile from) : base(6163, 1)
        {
            _from = from;
        }

        public override void OnClick(Mobile from, IEntity target)
        {
            if (_from.Talents.Count == 0)
            {
                return;
            }
            _from.SendGump(new TalentBarGump(_from, 1, 0, new List<TalentGump.TalentGumpPage>()));
        }
    }
}
