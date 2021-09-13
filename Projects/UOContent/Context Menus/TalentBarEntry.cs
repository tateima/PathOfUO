using Server.Multis;
using Server.Gumps;
using System.Collections.Generic;
using System;
using Server.Mobiles;

namespace Server.ContextMenus
{
    public class TalentBarEntry : ContextMenuEntry
    {
        private readonly PlayerMobile m_From;

        public TalentBarEntry(PlayerMobile from) : base(6163, 1)
        {
            m_From = from;
        }

        public override void OnClick()
        {
            if (m_From.Talents.Count == 0)
            {
                return;
            }
            m_From.SendGump(new TalentBarGump(m_From, 1, 0, new List<TalentGump.TalentGumpPage>()));
        }
    }
}
