using System;
using System.Linq;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Talent;
using System.Collections.Generic;
using Server.Pantheon;

namespace Server.Gumps
{
    public class TalentBarGump : Gump
    {
        private readonly int m_LastTalentIndex = 0;
        private readonly int m_LastPage = 1;
        private readonly List<BaseTalent> m_UseableTalents;
        private readonly List<TalentGump.TalentGumpPage> m_TalentGumpPages;
        public TimerExecutionToken _talentBarExecutionToken;
        private readonly Mobile m_Mobile;

        public TalentBarGump(Mobile from, int page, int lastTalentIndex, List<TalentGump.TalentGumpPage> talentGumpPages) : base(0, 0)
        {
            Closable = false;
            Disposable = true;
            Draggable = true;
            Resizable = true;
            m_LastPage = page;
            m_TalentGumpPages = talentGumpPages;
            m_Mobile = from;
            PlayerMobile player = (PlayerMobile)from;
            m_UseableTalents = new List<BaseTalent>();

            foreach (var (_, value) in player.Talents)
            {
                BaseTalent[] dependencyMatrix = BaseTalent.GetTalentDependency(player, value);
                BaseTalent dependsOn = dependencyMatrix.Length > 0 ? dependencyMatrix[0] : null;
                BaseTalent hasDependency = dependencyMatrix.Length > 1 ? dependencyMatrix[1] : null;
                if (
                    value.CanBeUsed && ((dependsOn is not null && hasDependency is not null && hasDependency.Level >= value.TalentDependencyPoints) || (dependsOn is null)) &&
                        (
                            !value.RequiresDeityFavor || (value.RequiresDeityFavor && player.HasDeityFavor && value.DeityAlignment != Deity.Alignment.None && value.DeityAlignment == player.CombatAlignment)
                        )
                )
                {
                    m_UseableTalents.Add(value);
                }
            }
            m_UseableTalents.Sort((x, y) => string.Compare(x.DisplayName, y.DisplayName));
            int barWidth = (m_UseableTalents.Count * 70);
            barWidth = barWidth switch
            {
                > 1100 => 1100,
                < 200  => 220,
                _      => barWidth
            };
            AddPage(0);
            AddImageTiled(0, 0, barWidth, 200, 0xA8E);
            AddImageTiled(0, 0, 5, 200, 0x27A7);
            AddImageTiled(0, 0, barWidth, 5, 0x27A7);
            AddImageTiled(barWidth, 0, 5, 200, 0x27A7);
            AddImageTiled(0, 200, barWidth, 5, 0x27A7);
            // close button
            AddButton(barWidth, 0, 40015, 40015, 1002);
            int x = 10;
            int y = 10;
            if (page > 1)
            {
                AddButton(0, 0, 40016, 40016, 1001);
            }
            TalentGump.TalentGumpPage existingTalentGumpPage = m_TalentGumpPages.FirstOrDefault(g => g.TalentPage == page);
            if (existingTalentGumpPage == null)
            {
                TalentGump.TalentGumpPage talentGumpPage = new TalentGump.TalentGumpPage(lastTalentIndex, page);
                m_TalentGumpPages.Add(talentGumpPage);
            }

            for (int i = lastTalentIndex; i < m_UseableTalents.Count; i++)
            {
                var talent = m_UseableTalents.ElementAt(i);
                if (y + 60 > 200)
                {
                    y = 10;
                    x += 110;
                }
                if (x + 110 > 1100)
                {
                    // need a new page
                    m_LastTalentIndex = i;
                    AddButton(1069, 0, 40017, 40017, 1000);
                    break;
                }

                int hue = 0;
                string cooldownLeft = "";
                if (talent.Activated)
                {
                    hue = 0x170;
                } else if (talent.OnCooldown)
                {
                    hue = 0x26;
                    cooldownLeft = $"{WaitTeleporter.FormatTime(talent._talentTimerToken.Next - Core.Now)}";
                }
                AddImage(x, y, talent.ImageID, hue);
                string display = talent.DisplayName;
                if (cooldownLeft.Length > 0)
                {
                    display = $"{display}: {cooldownLeft}";
                }
                AddHtml(x, y + 40, 100, 100, $"<BASEFONT COLOR=#FFFFE5>{display}</FONT>");

                if (talent.HasSkillRequirement(from) && !talent.OnCooldown)
                {
                    AddButton(x + 30, y + 10, 2223, 2223, 0 + i);
                }
                y += 100;
            }
            Timer.StartTimer(TimeSpan.FromSeconds(5), UpdateGump, out _talentBarExecutionToken);
        }

        public void UpdateGump()
        {
            m_Mobile.CloseGump<TalentBarGump>();
            m_Mobile.SendGump(new TalentBarGump(m_Mobile, m_LastPage, m_LastTalentIndex, m_TalentGumpPages));
        }
        public override void OnResponse(NetState state, RelayInfo info)
        {
            PlayerMobile player = (PlayerMobile)state.Mobile;

            if (player != null)
            {
                int page = m_LastPage;
                int lastTalentIndex = m_LastTalentIndex;
                if (info.ButtonID == 1000)
                {
                    lastTalentIndex += 1;
                    // next page
                    page = (m_LastPage + 1);
                    player.SendGump(new TalentBarGump(player, page, lastTalentIndex, m_TalentGumpPages));
                    return;
                }
                else if (info.ButtonID == 1001)
                {
                    page = (m_LastPage - 1);
                }
                else if (info.ButtonID == 1002)
                {
                    player.CloseGump<TalentBarGump>();
                    _talentBarExecutionToken.Cancel();
                    return;
                }
                else if (info.ButtonID >= 0)
                {
                    var talent = m_UseableTalents.ElementAt(info.ButtonID);
                    talent.OnUse(state.Mobile);
                }

                if (page < 0)
                {
                    page = 1;
                }

                TalentGump.TalentGumpPage talentGumpPage = m_TalentGumpPages.First(g => g.TalentPage == page);
                lastTalentIndex = talentGumpPage.TalentIndex;

                if (lastTalentIndex < 0)
                {
                    lastTalentIndex = 0;
                }

                player.SendGump(new TalentBarGump(player, page, lastTalentIndex, m_TalentGumpPages));
            }
        }
    }
}
