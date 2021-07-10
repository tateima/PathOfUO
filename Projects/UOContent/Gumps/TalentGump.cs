using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Talent;
using System.Collections.Generic;

namespace Server.Gumps
{
    public class TalentGump : Gump
    {
        private int m_LastTalentIndex = 0;
        private int m_LastPage = 1;

        public TalentGump(Mobile from, int page) : base(0, 0)
        {
            if (from == null)
            {
                from.CloseGump<TalentGump>();
            }
            Closable = true;
            Disposable = true;
            Draggable = true;
            Resizable = false;
            AddPage(0);
            AddImageTiled(0, 0, 1100, 900, 0xA8E);
            AddImageTiled(0, 0, 20, 900, 0x27A7);
            AddImageTiled(0, 0, 1100, 20, 0x27A7);
            AddImageTiled(1100, 0, 20, 920, 0x27A7);
            AddImageTiled(0, 900, 1100, 20, 0x27A7);
            AddLabel(20, 20, 0, "Talents");
            AddLabel(20, 60, 0, "Talent Points: " + ((PlayerMobile)from).TalentPoints.ToString());
            Type[] talentTypes = BaseTalent.TalentTypes;
            HashSet<BaseTalent> playerTalents = ((PlayerMobile)from).Talents;
            int x = 40;
            int y = 40;
            if (page > 1)
            {
                AddButton(5, 5, 40017, 40016, 1001, GumpButtonType.Reply, 0);
            }
            
            for (int i = m_LastTalentIndex; i < talentTypes.Length; i++)
            {
                if (y + 40 > 860)
                {
                    y = 40;
                    x += 220;
                }
                if (x + 220 > 1080)
                {
                    // need a new page
                    m_LastTalentIndex = i;
                    AddButton(5, 760, 40017, 40017, 1000, GumpButtonType.Reply, 0);
                    break;
                }
                var talent = TalentSerializer.Construct(talentTypes[i]) as BaseTalent;
                var dependsOn = TalentSerializer.Construct(talent.TalentDependency) as BaseTalent;
                Type[] blockedBy = talent.BlockedBy;
                BaseTalent hasBlocker = null;
                string blockedByStr = "";
                if (blockedBy != null)
                {
                    foreach(Type blockerType in blockedBy)
                    {
                        var blockerTalent = TalentSerializer.Construct(blockerType) as BaseTalent;
                        if (blockerTalent != null)
                        {
                            if (playerTalents != null && playerTalents.TryGetValue(blockerTalent, out hasBlocker)) {
                                break;
                            }
                            blockedByStr += blockerTalent.DisplayName;
                            if (blockerType != blockedBy[^1])
                            {
                                blockedByStr += ", ";
                            }
                            else
                            {
                                blockedByStr += ".";
                            }
                        }
                    }
                }
                int talentLevel = 0;
                BaseTalent used;
                BaseTalent hasDependency = null;
                if (playerTalents != null && playerTalents.TryGetValue(talent, out used))
                {
                    talentLevel = used.Level;
                    if (dependsOn != null)
                    {
                        playerTalents.TryGetValue(dependsOn, out hasDependency);
                    }
                };

                AddHtml(x, y, 200, 40, $"<BASEFONT COLOR=#FFFFFF>{talent.DisplayName}: ({talentLevel}/{talent.MaxLevel})</FONT>");
                AddImage(x + 160, y, talent.ImageID);
                if (talent.HasSkillRequirement(from) && (talentLevel < talent.MaxLevel && ((PlayerMobile)from).TalentPoints > 0) && ((dependsOn != null && hasDependency != null) || (dependsOn == null)) && (blockedBy != null && hasBlocker == null))
                {
                    AddButton(x, y + 10, 2223, 2223, 0 + i, GumpButtonType.Reply, 0);
                }
                y += 40;
                string requirements = (dependsOn != null) ? $"<BR>Requires {dependsOn.DisplayName}" : "";
                blockedByStr = (blockedByStr.Length > 1) ? $"<BR>Blocks: {blockedByStr}" : "";
                AddHtml(x, y, 200, 100, $"<BASEFONT COLOR=#FFFFFF>{talent.Description}{requirements}{blockedByStr}</FONT>");
                y += 110;
            }
        }
        public override void OnResponse(NetState state, RelayInfo info)
        {
            PlayerMobile player = (PlayerMobile)state.Mobile;

            if (player != null)
            {
                if (info.ButtonID == 1000)
                {
                    // next / previous page
                    m_LastPage++;
                }
                else if (info.ButtonID == 1001 && m_LastPage > 1)
                {
                    m_LastPage--;
                }
                else if (info.ButtonID > 0)
                {
                    var talent = TalentSerializer.Construct(BaseTalent.TalentTypes[info.ButtonID]) as BaseTalent;
                    HashSet<BaseTalent> playerTalents = (player.Talents != null) ? player.Talents : new HashSet<BaseTalent>();
                    BaseTalent used;
                    if (playerTalents.TryGetValue(talent, out used))
                    {
                        talent.Level = used.Level;
                        playerTalents.Remove(used);
                    }
                    talent.Level++;
                    playerTalents.Add(talent);
                    player.TalentPoints--;
                }
                   
                player.SendGump(new CharacterSheetGump(player, m_LastPage));
            }
        }
    }
}
