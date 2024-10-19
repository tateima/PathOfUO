using System.Collections.Generic;
using Server.ContextMenus;
using Server.Dungeon;
using Server.Network;
using Server.Mobiles;

namespace Server.Gumps
{
    public class DungeonDifficultyGump : Gump
    {
        public class DungeonDifficultyEntry : ContextMenuEntry
        {
            private readonly PlayerMobile _from;

            public DungeonDifficultyEntry(PlayerMobile from)
                : base(1116799, -1) // Set Dungeon Difficulty
            {
                _from = from;
            }

            public override void OnClick(Mobile from, IEntity target)
            {
                _from.CloseGump<DungeonDifficultyGump>();
                _from.SendGump(new DungeonDifficultyGump());
            }
        }
        public DungeonDifficultyGump() : base(0, 0)
        {
            Closable = true;
            Disposable = true;
            Draggable = true;
            Resizable = false;
            AddPage(0);
            int width = 350;
            int height = 320;
            int descriptionWidth = 300;
            string description = "Choose a dungeon difficulty to set.";
            AddButton(325, 25, 40015, 40015, 4);
            List<Option> options = new List<Option>
            {
                new Option(
                    1,
                    40,
                    80,
                    80,
                    80,
                    "Beginner: Reduces monster level range by 10, can only be done on level one."
                ),
                new Option(
                    2,
                    40,
                    140,
                    80,
                    140,
                    "Hard: Increases monster level range by 10. Unique monster chance increased by 10%. Experience gain increased by 3%"
                ),
                new Option(3, 40, 220, 80,220,
                    "Epic: Increases monster level range by 20. Unique monster chance increased by 20%. Hunger and thirst increases more quickly. Experience gain increased by 7%")
            };

            AddImageTiled(0, 0, width, height, 0xA8E);
            AddImageTiled(0, 0, 20, height, 0x27A7);
            AddImageTiled(0, 0, width, 20, 0x27A7);
            AddImageTiled(width, 0, 20, height + 20, 0x27A7);
            AddImageTiled(0, height, width, 20, 0x27A7);

            AddHtml(40, 40, descriptionWidth, 75, $"<BASEFONT COLOR=#FFFFE5>{description}</FONT>");

            foreach (var option in options)
            {
                AddButton(option.ButtonX, option.ButtonY, 0xFA5, 0xFA7, option.ButtonId);
                AddHtml(option.X, option.Y, 250, 150, $"<BASEFONT COLOR=#FFFFE5>{option.Text}</FONT>");
            }
        }
        public override void OnResponse(NetState state, in RelayInfo info)
        {
            PlayerMobile player = (PlayerMobile)state.Mobile;

            if (player != null)
            {
                if (info.ButtonID == 4)
                {
                    player.CloseGump<DungeonDifficultyGump>();
                }
                else
                {
                    DungeonLevelMod.DungeonDifficulty difficulty = info.ButtonID switch
                    {
                        3 => DungeonLevelMod.DungeonDifficulty.Epic,
                        2 => DungeonLevelMod.DungeonDifficulty.Hard,
                        _ => DungeonLevelMod.DungeonDifficulty.Beginner
                    };

                    Point3D location = player.Location;
                    Map map = player.Map;
                    int level = 0;
                    if (DungeonLevelModHandler.IsInLevelOneDungeon(location, map))
                    {
                        level = 1;
                    } else if (DungeonLevelModHandler.IsInLevelTwoDungeon(location, map))
                    {
                        level = 2;
                    } else if (DungeonLevelModHandler.IsInLevelThreeDungeon(location, map))
                    {
                        level = 3;
                    } else if (DungeonLevelModHandler.IsInLevelFourDungeon(location, map))
                    {
                        level = 4;
                    } else if (DungeonLevelModHandler.IsInLevelFiveDungeon(location, map))
                    {
                        level = 4;
                    }

                    if (level > 0)
                    {
                        DungeonLevelModHandler.SetDungeonDifficultyParameters(
                            location,
                            map,
                            level,
                            difficulty,
                            out string modName,
                            out int x1,
                            out int x2,
                            out int y1,
                            out int y2
                        );
                        if (!(x1 == 0 && x2 == 0 && y1 == 0 && y2 == 0) && modName.Length > 0)
                        {
                            DungeonLevelModHandler.SetDungeonDifficulty(
                                player,
                                difficulty,
                                modName,
                                x1,
                                x2,
                                y1,
                                y2
                            );
                        }
                    }
                    else
                    {
                        player.PrivateOverheadMessage(MessageType.Regular, player.SpeechHue, true, "You do not seem to be in a dungeon.", player.NetState);
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
