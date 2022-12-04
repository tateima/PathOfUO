using Server.ContextMenus;
using Server.Network;
using Server.Mobiles;
using Server.Talent;
using System.Collections.Generic;
using Server.Pantheon;

namespace Server.Gumps
{
    public class DeityRewardEntry : ContextMenuEntry
    {
        private readonly PlayerMobile _From;

        public DeityRewardEntry(PlayerMobile from)
            : base(1116794, -1) // Seek deity reward
        {
            _From = from;
        }

        public override void OnClick()
        {

            _From.PlaySound(0x24A);
            Deity.BestowReward(_From);
        }
    }
    public class DeityFavorEntry : ContextMenuEntry
    {
        private readonly PlayerMobile _From;

        public DeityFavorEntry(PlayerMobile from)
            : base(1116793, -1) // Seek deity favor
        {
            _From = from;
        }

        public override void OnClick()
        {
            _From.PlaySound(0x24A);
            Deity.BestowFavor(_From, _From.Alignment);
        }
    }
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
            _From.SendGump(new CharacterSheetGump(_From, 1, null, true));
        }
    }

    public class CharacterSheetGump : Gump
    {
        private readonly SkillsGumpGroup[] m_Groups;
        private SkillsGumpGroup m_SkillGroup;
        private int m_Page;
        private Mobile m_Mobile;
        public TimerExecutionToken _characterSheetExecutionToken;
        public CharacterSheetGump(Mobile from, int page, SkillsGumpGroup skillGroup = null, bool playSound = false) : base(0, 0)
        {
            m_Groups = SkillsGumpGroup.Groups;
            if (from == null)
            {
                from.CloseGump<CharacterSheetGump>();
            }
            if (from is PlayerMobile player)
            {
                if (skillGroup != null)
                {
                    m_SkillGroup = skillGroup;
                }
                Closable = true;
                Disposable = true;
                Draggable = true;
                Resizable = false;
                AddPage(0);
                AddImage(40, 36, 500);
                if (playSound)
                {
                    from.SendSound(0x55);
                }
                m_Mobile = from;
                m_Page = page;
                int y = 100;
                // this.AddButton(525, 50, 1058, 1058, 1001, GumpButtonType.Reply, 0);
                if (page > 1)
                {
                    AddButton(40, 35, 501, 501, 1000, GumpButtonType.Reply, 0);
                }

                int x = 90;
                if (page == 2 && m_SkillGroup != null)
                {
                    AddLabel(110, 60, 0, m_SkillGroup.Name);
                    AddLabel(90, 80, 0, "Skill Pts: " + player.CraftSkillPoints.ToString() + "C/" + player.NonCraftSkillPoints.ToString() + "NC/" + player.RangerSkillPoints.ToString() + "R");
                    y = 100;
                    double maxSkill = player.Level switch
                    {
                        <= 10 => 70.00,
                        <= 20 => 80.00,
                        <= 30 => 85.00,
                        <= 40 => 90.00,
                        <= 50 => 95.00,
                        <= 60 => 98.00,
                        _     => 100.00
                    };
                    for (int i = 0; i < m_SkillGroup.Skills.Length; ++i)
                    {
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
                            double currentSkillValue = player.Skills[m_SkillGroup.Skills[i]].Base;
                            if (skill.Lock != SkillLock.Locked && currentSkillValue < maxSkill)
                            {
                                AddLabel(x, y, 0, skill.Name + " " + currentSkillValue);
                                if (
                                    (BaseTalent.IsCraftingSkill(m_SkillGroup.Skills[i]) && player.CraftSkillPoints > 0)
                                    ||
                                    (!BaseTalent.IsCraftingSkill(m_SkillGroup.Skills[i]) && !BaseTalent.IsRangerSkill(m_SkillGroup.Skills[i]) && player.NonCraftSkillPoints > 0)
                                    ||
                                    (BaseTalent.IsRangerSkill(m_SkillGroup.Skills[i]) && player.RangerSkillPoints > 0)
                                )
                                {
                                    AddButton(buttonX, y + 2, 2223, 2223, 200 + i, GumpButtonType.Reply, 0);
                                }
                            }
                            else
                            {
                                AddLabel(x, y, 0, $"{skill.Name} {currentSkillValue}/{maxSkill}");
                            }
                            y += 20;
                        }
                    }
                }
                else
                {
                    if (player.HardCore)
                    {
                        AddLabel(80, 60, 0, "Character Sheet (Hardcore)");
                    }
                    else
                    {
                        AddLabel(80, 60, 0, "Character Sheet");
                    }
                    AddLabel(90, 80, 0, "Level: " + player.Level.ToString());
                    AddLabel(90, 100, 0, "Experience Points:");
                    int totalExperience = player.LevelExperience + player.CraftExperience + player.NonCraftExperience + player.RangerExperience;
                    AddLabel(90, 120, 0, totalExperience.ToString());
                    AddLabel(90, 140, 0, "Talents");
                    AddButton(190, 144, 2223, 2223, 300, GumpButtonType.Reply, 0);
                    AddLabel(90, 160, 0, "Stat Points: " + player.StatPoints.ToString());
                    AddLabel(90, 180, 0, "Strength: " + from.RawStr.ToString());
                    if (player.StatPoints > 0)
                    {
                        AddButton(190, 182, 2223, 2223, 1, GumpButtonType.Reply, 0);
                    }
                    AddLabel(90, 200, 0, "Dexterity: " + from.RawDex.ToString());
                    if (player.StatPoints > 0)
                    {
                        AddButton(190, 200, 2223, 2223, 2, GumpButtonType.Reply, 0);
                    }
                    AddLabel(90, 220, 0, "Intelligence: " + from.RawInt.ToString());
                    if (player.StatPoints > 0)
                    {
                        AddButton(190, 222, 2223, 2223, 3, GumpButtonType.Reply, 0);
                    }

                    AddLabel(270, 60, 0, "Skill Points: " + player.CraftSkillPoints.ToString() + "C/" + player.NonCraftSkillPoints.ToString() + "NC/" + player.RangerSkillPoints.ToString() + "R");
                    AddLabel(270, 80, 0, "Skill Groups");
                    for (int i = 0; i < m_Groups.Length; ++i)
                    {
                        AddLabel(270, y, 0, m_Groups[i].Name);
                        AddButton(390, y + 2, 2223, 2223, 100 + i, GumpButtonType.Reply, 0);
                        y += 20;
                    }
                }
            }
            else
            {
                from.CloseGump<CharacterSheetGump>();
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            PlayerMobile player = (PlayerMobile)state.Mobile;

            if (player != null)
            {
                if (info.ButtonID > 0)
                {
                    int page = 1;
                    if (info.ButtonID >= 100 && info.ButtonID < 300)
                    {
                        page = 2;
                    } else if (info.ButtonID == 300)
                    {
                        player.CloseGump<CharacterSheetGump>();
                        player.SendGump(new TalentGump(player, 1, 0, new List<TalentGump.TalentGumpPage>()));
                       return;
                    }

                    if (info.ButtonID == 1000)
                    {
                        page--;
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
                    if (page == 2) // Skills
                    {
                        if (info.ButtonID < 200)
                        {
                            m_SkillGroup = m_Groups[info.ButtonID - 100];
                        }
                        else if (info.ButtonID < 300 && m_SkillGroup != null)
                        {
                            Skill skill = player.Skills[m_SkillGroup.Skills[info.ButtonID - 200]];
                            if (skill != null)
                            {
                                double amount = 1.0;
                                if (BaseTalent.IsCraftingSkill(m_SkillGroup.Skills[info.ButtonID - 200]))
                                {
                                    player.CraftSkillPoints--;
                                    player.AllottedCraftSkillPoints++;
                                } else if (BaseTalent.IsRangerSkill(m_SkillGroup.Skills[info.ButtonID - 200]))
                                {
                                    player.RangerSkillPoints--;
                                }
                                else
                                {
                                    if (BaseTalent.IsLoreSkill(m_SkillGroup.Skills[info.ButtonID - 200]))
                                    {
                                        var loreSeeker = player.GetTalent(typeof(LoreSeeker)) as LoreSeeker;
                                        if (loreSeeker?.Level > 0)
                                        {
                                            amount += loreSeeker.Level;
                                        }
                                    }
                                    player.NonCraftSkillPoints--;
                                }
                                player.Skills[m_SkillGroup.Skills[info.ButtonID - 200]].Base += amount;
                            }
                        }
                        else
                        {
                            m_SkillGroup = null;
                        }
                    }

                    player.SendGump(new CharacterSheetGump(player, page, m_SkillGroup));
                }
            }
        }
    }
}
