using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Pantheon;

namespace Server.Gumps
{
    public class ChoosePathGump : Gump
    {
        private readonly bool _createStaterSkills;
        private readonly bool _alignmentChange;
        private readonly Deity.Alignment _currentAlignment;

        public ChoosePathGump(Mobile from, int page, bool createStaterSkills, bool alignmentChange, Deity.Alignment currentAlignment) : base(0, 0)
        {
            _createStaterSkills = createStaterSkills;
            _currentAlignment = currentAlignment;
            _alignmentChange = alignmentChange;
            if (from == null)
            {
                from.CloseGump<ChoosePathGump>();
            }
            Closable = false;
            Disposable = true;
            Draggable = true;
            Resizable = false;
            AddPage(0);
            int width = 500;
            int height = 500;
            int gumpImage = 0xAF8;
            int imageX = 220;
            int descriptionWidth = 150;
            string description = "Choose a childhood from your past.";
            List<Option> options = new List<Option>();
            if (page == 1)
            {
                options.Add(new Option(1, 40, 120, 80, 120, "Bookworm"));
                options.Add(new Option(2, 40, 160, 80,160, "Athletic"));
                options.Add(new Option(3, 40, 200, 80,200, "Bully"));
                options.Add(new Option(4, 40, 240, 80,240, "Street kid"));
                options.Add(new Option(5, 40, 280, 80,280, "Farmer kid"));
                options.Add(new Option(6, 40, 320, 80,320, "Soldiers son"));
                options.Add(new Option(7, 40, 360, 80,360, "Outcast"));
                options.Add(new Option(8, 40, 400, 80,400, "Wanderer"));
            }
            else if (page == 2)
            {
                var bookExist = from.Backpack?.FindItemByType(typeof(BookOfAlignments));
                if (bookExist is null)
                {
                    var book = new BookOfAlignments();
                    from.AddToBackpack(book);
                }
                descriptionWidth = 210;
                imageX = 260;
                width = 400;
                height = 400;
                gumpImage = 0xAD8;
                var deityButtonId = 9;
                var deityButtonX = 70;
                var deityButtonY = 200;
                var deityTextX = 110;
                var deityTextY = 200;
                foreach (var deity in Enum.GetValues(typeof(Deity.Alignment)))
                {
                    if (deityButtonY + 40 == 400)
                    {
                        deityButtonX = 250;
                        deityButtonY = 200;
                        deityTextX = 290;
                        deityTextY = deityButtonY;
                    }
                    options.Add(new Option(deityButtonId, deityButtonX, deityButtonY, deityTextX, deityTextY, deity.ToString()));
                    deityButtonId++;
                    deityButtonY += 40;
                    deityTextY = deityButtonY;
                }
                description = "Choose a deity alignment wish to worship. If you perform their alignment wishes well you will be granted unique and rare rewards. A book has been placed in your backpack with details.";
            }
            else if (page == 3)
            {
                imageX = 200;
                width = 400;
                height = 160;
                gumpImage = 0xAAE;
                description = "Do thou want to live a murderous path? If you say no you cannot turn back.";
                options.Add(new Option(16, 40, 120, 80, 120, "Yes"));
                options.Add(new Option(17, 110, 120, 150,120, "No"));
            }
            AddImageTiled(0, 0, width, height, 0xA8E);
            AddImageTiled(0, 0, 20, height, 0x27A7);
            AddImageTiled(0, 0, width, 20, 0x27A7);
            AddImageTiled(width, 0, 20, height + 20, 0x27A7);
            AddImageTiled(0, height, width, 20, 0x27A7);

            AddImage(imageX, 40, gumpImage);
            AddHtml(40, 40, descriptionWidth, 150, $"<BASEFONT COLOR=#FFFFE5>{description}</FONT>");

            foreach (var option in options)
            {
                AddButton(option.ButtonX, option.ButtonY, 0xFA5, 0xFA7, option.ButtonId);
                AddHtml(option.X, option.Y, 100, 50, $"<BASEFONT COLOR=#FFFFE5>{option.Text}</FONT>");
            }
        }

        public void CurrentlyAligned(PlayerMobile player, ref int page)
        {
            player.SendMessage("You are already aligned to this deity choice.");
            page = 2;
        }
        public override void OnResponse(NetState state, RelayInfo info)
        {
            PlayerMobile player = (PlayerMobile)state.Mobile;

            if (player != null)
            {
                int page = 2;
                if (info.ButtonID > 8)
                {
                    page = 3;
                }
                switch (info.ButtonID)
                {
                    case 1: // bookworm
                        player.RawStr -= Utility.RandomMinMax(2,5);
                        player.RawInt += Utility.RandomMinMax(2,5);
                        switch (Utility.Random(3))
                        {
                            case 1:
                                player.Skills[SkillName.EvalInt].Base += Utility.RandomMinMax(1, 5);
                                break;
                            case 2:
                                player.Skills[SkillName.AnimalLore].Base += Utility.RandomMinMax(1, 5);
                                break;
                            default:
                                player.Skills[SkillName.Anatomy].Base += Utility.RandomMinMax(1, 5);
                                break;
                        }
                        break;
                    case 2: // athletic
                        player.RawDex += Utility.RandomMinMax(2,5);
                        switch (Utility.Random(3))
                        {
                            case 1:
                                player.Skills[SkillName.Focus].Base += Utility.RandomMinMax(1, 5);
                                break;
                            case 2:
                                player.Skills[SkillName.Wrestling].Base += Utility.RandomMinMax(1, 5);
                                break;
                            default:
                                player.Skills[SkillName.Archery].Base += Utility.RandomMinMax(1, 5);
                                break;
                        }
                        break;
                    case 3: // bully
                        player.RawStr += Utility.RandomMinMax(1,3);
                        player.RawInt -= Utility.RandomMinMax(1,3);
                        player.RawDex += Utility.RandomMinMax(1,2);
                        switch (Utility.Random(3))
                        {
                            case 1:
                                player.Skills[SkillName.Wrestling].Base += Utility.RandomMinMax(1, 5);
                                break;
                            case 2:
                                player.Skills[SkillName.Provocation].Base += Utility.RandomMinMax(1, 5);
                                break;
                            default:
                                player.Skills[SkillName.Anatomy].Base += Utility.RandomMinMax(1, 5);
                                break;
                        }
                        break;
                    case 4: // street kid
                        player.RawDex += Utility.RandomMinMax(1,3);
                        player.RawInt += Utility.RandomMinMax(1,2);
                        player.RawInt -= Utility.RandomMinMax(1,2);
                        switch (Utility.Random(3))
                        {
                            case 1:
                                player.Skills[SkillName.Hiding].Base += Utility.RandomMinMax(1, 5);
                                break;
                            case 2:
                                player.Skills[SkillName.Stealing].Base += Utility.RandomMinMax(1, 5);
                                break;
                            default:
                                player.Skills[SkillName.Stealth].Base += Utility.RandomMinMax(1, 5);
                                break;
                        }
                        break;
                    case 5: // farmer kid
                        player.RawStr += Utility.RandomMinMax(2,4);
                        player.RawDex += 1;
                        player.RawInt -= 1;
                        switch (Utility.Random(3))
                        {
                            case 1:
                                player.Skills[SkillName.AnimalLore].Base += Utility.RandomMinMax(1, 5);
                                break;
                            case 2:
                                player.Skills[SkillName.AnimalTaming].Base += Utility.RandomMinMax(1, 5);
                                break;
                            default:
                                player.Skills[SkillName.Herding].Base += Utility.RandomMinMax(1, 5);
                                break;
                        }
                        break;
                    case 6: // Soldiers son
                        player.RawStr += Utility.RandomMinMax(2,3);
                        player.RawDex += Utility.RandomMinMax(2,3);
                        player.RawInt -= Utility.RandomMinMax(1,2);
                        switch (Utility.Random(3))
                        {
                            case 1:
                                player.Skills[SkillName.Tactics].Base += Utility.RandomMinMax(1, 5);
                                break;
                            case 2:
                                player.Skills[SkillName.Parry].Base += Utility.RandomMinMax(1, 5);
                                break;
                            default:
                                player.Skills[SkillName.Blacksmith].Base += Utility.RandomMinMax(1, 5);
                                break;
                        }
                        break;
                    case 7: // outcast
                        switch (Utility.Random(3))
                        {
                            case 1:
                                player.Skills[SkillName.Camping].Base += Utility.RandomMinMax(1, 5);
                                break;
                            case 2:
                                player.Skills[SkillName.Alchemy].Base += Utility.RandomMinMax(1, 5);
                                break;
                            default:
                                player.Skills[SkillName.Hiding].Base += Utility.RandomMinMax(1, 5);
                                break;
                        }
                        switch (Utility.Random(3))
                        {
                            case 1:
                                player.Skills[SkillName.Fletching].Base += Utility.RandomMinMax(1, 5);
                                break;
                            case 2:
                                player.Skills[SkillName.Fishing].Base += Utility.RandomMinMax(1, 5);
                                break;
                            default:
                                player.Skills[SkillName.Cooking].Base += Utility.RandomMinMax(1, 5);
                                break;
                        }
                        break;
                    case 8: // wanderer
                        switch (Utility.Random(3))
                        {
                            case 1:
                                player.Skills[SkillName.AnimalTaming].Base += Utility.RandomMinMax(1, 5);
                                break;
                            case 2:
                                player.Skills[SkillName.AnimalLore].Base += Utility.RandomMinMax(1, 5);
                                break;
                            default:
                                player.Skills[SkillName.Archery].Base += Utility.RandomMinMax(1, 5);
                                break;
                        }
                        switch (Utility.Random(3))
                        {
                            case 1:
                                player.Skills[SkillName.Cartography].Base += Utility.RandomMinMax(1, 5);
                                break;
                            case 2:
                                player.Skills[SkillName.Musicianship].Base += Utility.RandomMinMax(1, 5);
                                break;
                            default:
                                player.Skills[SkillName.Discordance].Base += Utility.RandomMinMax(1, 5);
                                break;
                        }
                        break;
                    case 9: // deity
                        if (_currentAlignment is Deity.Alignment.Charity)
                        {
                            CurrentlyAligned(player, ref page);
                        }
                        else
                        {
                            player.Alignment = Deity.Alignment.Charity;
                        }
                        break;
                    case 10:
                        if (_currentAlignment is Deity.Alignment.Greed)
                        {
                            CurrentlyAligned(player, ref page);
                        }
                        else
                        {
                            player.Alignment = Deity.Alignment.Greed;
                        }
                        break;
                    case 11:
                        if (_currentAlignment is Deity.Alignment.Order)
                        {
                            CurrentlyAligned(player, ref page);
                        }
                        else
                        {
                            player.Alignment = Deity.Alignment.Order;
                        }

                        break;
                    case 12:
                        if (_currentAlignment is Deity.Alignment.Chaos)
                        {
                            CurrentlyAligned(player, ref page);
                        }
                        else
                        {
                            player.Alignment = Deity.Alignment.Chaos;
                        }

                        break;
                    case 13:
                        if (_currentAlignment is Deity.Alignment.Light)
                        {
                            CurrentlyAligned(player, ref page);
                        }
                        else
                        {
                            player.Alignment = Deity.Alignment.Light;
                        }

                        break;
                    case 14:
                        if (_currentAlignment is Deity.Alignment.Darkness)
                        {
                            CurrentlyAligned(player, ref page);
                        }
                        else
                        {
                            player.Alignment = Deity.Alignment.Darkness;
                        }

                        break;
                    case 15:
                        if (_currentAlignment is Deity.Alignment.None)
                        {
                            CurrentlyAligned(player, ref page);
                        }
                        else
                        {
                            player.Alignment = Deity.Alignment.None;
                        }
                        break;
                    case 16:
                        page = 0;
                        Point3D point = new Point3D(2681, 2223, 0);
                        player.MoveToWorld(point, Map.Trammel); // bucs den
                        player.Kills = 5;                       // these dont decay
                        player.ShortTermMurders = 5;            // these dont decay, perma red
                        break;
                    case 17:
                        page = 0;
                        break;

                }

                if (page > 0)
                {
                    player.SendGump(new ChoosePathGump(player, page, _createStaterSkills, _alignmentChange, _currentAlignment));
                }
                else
                {
                    player.CloseGump<ChoosePathGump>();
                    if (_alignmentChange)
                    {
                        if (
                            player.Level >= 5
                            && (_currentAlignment is Deity.Alignment.None && player.Alignment is not Deity.Alignment.None
                                || _currentAlignment is not Deity.Alignment.None && player.Alignment is Deity.Alignment.None)
                        )
                        {
                            player.SendMessage("Your talents have been reset as a result of your alignment change.");
                            player.ResetTalents();
                            int talentAmount = (int)Math.Floor((double)player.Level / 5);
                            // if the player was not aligned and is over level 5, remove a talent point for every 5 levels
                            // otherwise add new talent points as that is the perk for non aligned
                            player.TalentPoints +=
                                _currentAlignment is Deity.Alignment.None && player.Alignment is not Deity.Alignment.None
                                    ? talentAmount * -1
                                    : talentAmount;
                        }
                    }
                    if (_createStaterSkills)
                    {
                        player.StarterSkills = new Skills(player);
                        foreach (var skill in player.Skills)
                        {
                            player.StarterSkills[skill.SkillName].Base = skill.Base;
                        }
                    }
                    if (player.Alignment is not Deity.Alignment.None)
                    {
                        player.DeityDecay();
                    }
                }
            }
        }

        public class Option
        {
            public int ButtonId { get; set; }
            public int ButtonX { get; set; }
            public int ButtonY { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public string Text { get; set; }

            public Option(int buttonId, int buttonX, int buttonY, int x, int y, string text)
            {
                ButtonId = buttonId;
                ButtonX = buttonX;
                ButtonY = buttonY;
                X = x;
                Y = y;
                Text = text;
            }
        }
    }
}
