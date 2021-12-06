using System;
using System.Linq;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Talent;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Server.Gumps
{
    public class TalentGump : Gump
    {
        private readonly int m_LastTalentIndex;
        private readonly int m_LastPage;
        private readonly List<TalentGumpPage> m_TalentGumpPages;

        public TalentGump(Mobile from, int page, int lastTalentIndex, List<TalentGumpPage> talentGumpPages) : base(0, 0)
        {
            Closable = false;
            Disposable = true;
            Draggable = true;
            Resizable = false;
            m_LastPage = page;
            m_TalentGumpPages = talentGumpPages;
            AddPage(0);
            AddImageTiled(0, 0, 940, 900, 0xA8E);
            AddImageTiled(0, 0, 20, 900, 0x27A7);
            AddImageTiled(0, 0, 940, 20, 0x27A7);
            AddImageTiled(940, 0, 20, 920, 0x27A7);
            AddImageTiled(0, 900, 940, 20, 0x27A7);
            // close button
            AddButton(935, 0, 40015, 40015, 1002);
            Type[] talentTypes = BaseTalent.TalentTypes;
            PlayerMobile player = (PlayerMobile)from;
            AddLabel(80, 20, 2049, "Talent Points: " + player.TalentPoints);
            int x = 40;
            int y = 45;
            if (page > 1)
            {
                AddButton(20, 20, 40016, 40016, 1001);
            }
            TalentGumpPage existingTalentGumpPage = m_TalentGumpPages.FirstOrDefault(g => g.TalentPage == page);
            if (existingTalentGumpPage == null)
            {
                TalentGumpPage talentGumpPage = new TalentGumpPage(lastTalentIndex, page);
                m_TalentGumpPages.Add(talentGumpPage);
            }

            for (int i = lastTalentIndex; i < talentTypes.Length; i++)
            {
                var talent = TalentConstructor.Construct(talentTypes[i]) as BaseTalent;
                if (y + 45 > 860)
                {
                    y = 45;
                    x += 220;
                }
                if (x + 220 > 920)
                {
                    // need a new page
                    m_LastTalentIndex = i;
                    AddButton(909, 20, 40017, 40017, 1000);
                    break;
                }

                if (talent != null)
                {
                    Type[] blockedBy = talent.BlockedBy;
                    BaseTalent hasBlocker = null;
                    string blockedByStr = "";
                    if (blockedBy != null && !talent.IgnoreTalentBlock(@from))
                    {
                        foreach (Type blockerType in blockedBy)
                        {
                            if (TalentConstructor.Construct(blockerType) is BaseTalent blockerTalent)
                            {
                                blockedByStr += blockerTalent.DisplayName;
                                if (blockerType != blockedBy[^1])
                                {
                                    blockedByStr += ", ";
                                }
                                else
                                {
                                    blockedByStr += ".";
                                }
                                hasBlocker = player.GetTalent(blockerType);
                                if (hasBlocker != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    BaseTalent dependsOn = null;
                    BaseTalent hasDependency = null;
                    BaseTalent used = player.GetTalent(talent.GetType());
                    if (talent.TalentDependency != null)
                    {
                        dependsOn = TalentConstructor.Construct(talent.TalentDependency) as BaseTalent;
                        if (dependsOn != null)
                        {
                            hasDependency = player.GetTalent(dependsOn.GetType());
                        }
                    }
                    int talentLevel = (used != null && used.Level > 0) ? used.Level : 0;
                    AddHtml(x, y, 200, 45, $"<BASEFONT COLOR=#FFFFE5>{talent.DisplayName}: ({talentLevel}/{talent.MaxLevel})</FONT>");
                    int hue = 0;
                    if (talentLevel == 0 && (dependsOn != null && hasDependency == null) || (blockedBy != null && hasBlocker != null) || !talent.HasSkillRequirement(@from)
                        || (hasDependency != null && hasDependency.Level < talent.TalentDependencyPoints && hasDependency.Level != hasDependency.MaxLevel))
                    {
                        hue = 0x3E8;
                    }
                    if (talentLevel == talent.MaxLevel)
                    {
                        hue = 0x26;
                    }
                    talent.Level = talentLevel;
                    AddImage(x + 160, y - 10, talent.ImageID, hue);
                    if (talent.HasSkillRequirement(@from)
                        && (talentLevel < talent.MaxLevel && ((PlayerMobile)@from).TalentPoints > 0)
                        && ((dependsOn != null && hasDependency != null
                                               && (hasDependency.Level >= talent.TalentDependencyPoints || hasDependency.Level == hasDependency.MaxLevel))
                            || (dependsOn == null))
                        && (blockedBy != null && hasBlocker == null))
                    {
                        AddButton(x + 190, y, 2223, 2223, 0 + i, GumpButtonType.Reply, 0);
                    }
                    y += 40;
                    string requirements = (dependsOn != null) ? $"<BR>Requires {dependsOn.DisplayName}" : "";
                    blockedByStr = (blockedByStr.Length > 1) ? $"<BR>Blocks: {blockedByStr}" : "";
                    AddHtml(x, y, 200, talent.GumpHeight, $"<BASEFONT COLOR=#FFFFE5>{talent.Description}{requirements}{blockedByStr}</FONT>");
                }

                if (talent != null)
                {
                    y += talent.AddEndY;
                }
            }
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
                    player.SendGump(new TalentGump(player, page, lastTalentIndex, m_TalentGumpPages));
                    return;
                }

                if (info.ButtonID == 1001)
                {
                    page = (m_LastPage - 1);
                }
                else if (info.ButtonID == 1002)
                {
                    player.CloseGump<TalentGump>();
                    player.SendGump(new CharacterSheetGump(player, 1, null));
                    return;
                }
                else if (info.ButtonID >= 0)
                {
                    var talent = TalentConstructor.Construct(BaseTalent.TalentTypes[info.ButtonID]) as BaseTalent;
                    ConcurrentDictionary<Type,BaseTalent> playerTalents = player.Talents ?? new ConcurrentDictionary<Type,BaseTalent>();
                    BaseTalent used = player.GetTalent(BaseTalent.TalentTypes[info.ButtonID]);
                    talent = used ?? talent;
                    if (talent != null)
                    {
                        if (talent.UpgradeCost && talent.HasUpgradeRequirement(player) || !talent.UpgradeCost)
                        {
                            talent.Level++;
                            playerTalents.AddOrUpdate(BaseTalent.TalentTypes[info.ButtonID], talent, (t, bt) => talent);
                            player.TalentPoints--;
                            player.Talents = playerTalents;
                            talent.UpdateMobile(player);
                        }
                        else
                        {
                            player.SendMessage("You do not have the required upgrade resources to increase this talent");
                        }
                    }
                }

                if (page < 0)
                {
                    page = 1;
                }

                TalentGumpPage talentGumpPage = m_TalentGumpPages.First(g => g.TalentPage == page);
                lastTalentIndex = talentGumpPage.TalentIndex;

                if (lastTalentIndex < 0)
                {
                    lastTalentIndex = 0;
                }

                player.SendGump(new TalentGump(player, page, lastTalentIndex, m_TalentGumpPages));
            }
        }
        public class TalentGumpPage
        {
            public int TalentIndex { get; set; }

            public int TalentPage { get; set; }

            public TalentGumpPage(int talentIndex, int talentPage)
            {
                TalentIndex = talentIndex;
                TalentPage = talentPage;
            }


        }
    }
}
