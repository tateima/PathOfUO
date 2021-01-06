using System;
using Server;
using Server.ContextMenus;
using Server.Network;
using Server.Mobiles;
using Server.Targets;

namespace Server.Gumps
{
    public class CharacterSheetMenuEntry : ContextMenuEntry
    {
        private readonly PlayerMobile _From;

        public CharacterSheetMenuEntry(PlayerMobile from)
            : base(1116792, -1) // Character sheet
        {
            _From = from;
        }

        public override void OnClick()
        {
            _From.CloseGump<CharacterSheetGump>();
            _From.SendGump(new CharacterSheetGump(_From, 1));
        }
    }

    public class CharacterSheetGump : Gump
    {
        private readonly SkillsGumpGroup[] m_Groups;
        private SkillsGumpGroup m_SkillGroup;
        public CharacterSheetGump(Mobile from, int page, SkillsGumpGroup skillGroup = null) : base(0, 0)
        {
            m_Groups = SkillsGumpGroup.Groups;
            if (from != null)
            {
                from.CloseGump<CharacterSheetGump>();
            }
            if (skillGroup != null)
            {
                m_SkillGroup = skillGroup;
            }
            this.Closable = true;
            this.Disposable = true;
            this.Draggable = true;
            this.Resizable = false;

            this.AddPage(0);
            this.AddImage(40, 36, 500);
            from.SendSound(0x55);
            int y = 100;
            // this.AddButton(525, 50, 1058, 1058, 1001, GumpButtonType.Reply, 0);
            if (page == 2 && m_SkillGroup != null)
            {
                this.AddButton(40, 35, 501, 501, 1000, GumpButtonType.Reply, 0);
                this.AddLabel(110, 60, 0, m_SkillGroup.Name);
                this.AddLabel(90, 80, 0, "Skill Points: " + ((PlayerMobile)from).SkillPoints.ToString());
                y = 100;
                for (int i = 0; i < m_SkillGroup.Skills.Length; ++i)
                {
                    int x = 90;
                    int buttonX = 220;
                    if (i > 6)
                    {
                        // go to next page
                        x = 270;
                        buttonX = 390;
                    }
                    if (i == 7)
                    {
                        y = 60;
                    }
                    Skill skill = from.Skills[m_SkillGroup.Skills[i]];
                    if (skill != null)
                    {
                        string currentSkillValue = ((PlayerMobile)from).Skills[m_SkillGroup.Skills[i]].Base.ToString();
                        if (skill.Lock != SkillLock.Locked)
                        {
                            this.AddLabel(x, y, 0, skill.Name + " " + currentSkillValue);
                            if (((PlayerMobile)from).SkillPoints > 0)
                            {
                                this.AddButton(buttonX, y + 2, 2223, 2223, 60 + i, GumpButtonType.Reply, 0);
                            }
                        }
                        else
                        {
                            this.AddLabel(x, y, 0, skill.Name + " " + currentSkillValue + " - Locked");
                        }
                        y += 20;
                    }
                }
            }
            else
            {
                if (((PlayerMobile)from).HardCore)
                {
                    this.AddLabel(90, 60, 0, "Character Sheet - (Hardcore)");
                }
                else
                {
                    this.AddLabel(90, 60, 0, "Character Sheet");
                }
                this.AddLabel(90, 80, 0, "Level: " + (Array.IndexOf(Enum.GetNames(typeof(Level)), ((PlayerMobile)from).Level) + 1).ToString());
                this.AddLabel(90, 100, 0, "Experience Points:");
                this.AddLabel(90, 120, 0, ((PlayerMobile)from).Experience.ToString());
                this.AddLabel(90, 140, 0, "Stat Points: " + ((PlayerMobile)from).StatPoints.ToString());
                this.AddLabel(90, 160, 0, "Strength: " + from.RawStr.ToString());
                if (((PlayerMobile)from).StatPoints > 0)
                {
                    this.AddButton(190, 155, 2151, 2153, 1, GumpButtonType.Reply, 0);
                }
                this.AddLabel(90, 180, 0, "Dexterity: " + from.RawDex.ToString());
                if (((PlayerMobile)from).StatPoints > 0)
                {
                    this.AddButton(190, 175, 2151, 2153, 2, GumpButtonType.Reply, 0);
                }
                this.AddLabel(90, 200, 0, "Intelligence: " + from.RawInt.ToString());
                if (((PlayerMobile)from).StatPoints > 0)
                {
                    this.AddButton(190, 195, 2151, 2153, 3, GumpButtonType.Reply, 0);
                }
                this.AddLabel(270, 60, 0, "Skill Points: " + ((PlayerMobile)from).SkillPoints.ToString());
                this.AddLabel(270, 80, 0, "Skill Groups");
                for (int i = 0; i < m_Groups.Length; ++i)
                {
                    this.AddLabel(270, y, 0, m_Groups[i].Name);
                    this.AddButton(390, y + 2, 2223, 2223, 40 + i, GumpButtonType.Reply, 0);
                    y += 20;
                }
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            PlayerMobile player = (PlayerMobile)state.Mobile;

            if (player != null)
            {
                if (info.ButtonID > 0)
                {
                    int page = (info.ButtonID >= 60) ? 2 : 1;
                    if (info.ButtonID == 1000)
                    {
                        page = 1;
                    }
                    switch (info.ButtonID)
                    {
                        case 1:
                            player.RawStr++;
                            player.StatPoints--;
                            break;
                        case 2:
                            player.RawDex++;
                            player.StatPoints--;
                            break;
                        case 3:
                            player.RawInt++;
                            player.StatPoints--;
                            break;
                    }
                    if (info.ButtonID >= 40 && info.ButtonID < 1000 && page == 1)
                    {
                        page = 2;
                        m_SkillGroup = m_Groups[info.ButtonID - 40];
                    }
                    else if (info.ButtonID >= 60 && page == 2 && m_SkillGroup != null)
                    {
                        Skill skill = player.Skills[m_SkillGroup.Skills[info.ButtonID - 60]];
                        if (skill != null)
                        {
                            player.Skills[m_SkillGroup.Skills[info.ButtonID - 60]].Base++;
                            player.SkillPoints--;
                        }
                    }
                    else
                    {
                        m_SkillGroup = null;
                    }
                    player.SendGump(new CharacterSheetGump(player, page, m_SkillGroup));
                }
            }
        }
    }
}
