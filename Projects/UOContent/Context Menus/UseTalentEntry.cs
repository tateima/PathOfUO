using Server.Multis;
using Server.Talent;
using System.Collections.Generic;
using System;
using Server.Mobiles;

namespace Server.ContextMenus
{
    public class UseTalentEntry : ContextMenuEntry
    {
        private readonly PlayerMobile m_From;
        private readonly BaseTalent m_BaseTalent;

        public UseTalentEntry(PlayerMobile from, BaseTalent baseTalent) : base(6211, 1)
        {
            m_From = from;
            m_BaseTalent = baseTalent;
        }

        public override void OnClick()
        {
            if (m_From.Talents == null)
            {
                return;
            }
            HashSet<BaseTalent> playerTalents = m_From.Talents;
            BaseTalent used;
            if (playerTalents.TryGetValue(m_BaseTalent, out used))
            {
                playerTalents.Remove(used);
                m_BaseTalent.OnUse(m_From);
                playerTalents.Add(m_BaseTalent);
                m_From.Talents = playerTalents;
            }
        }
    }
}
