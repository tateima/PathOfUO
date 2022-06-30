using System.Collections.Generic;
using Server.Network;
using Server.Mobiles;

namespace Server.Gumps
{
    public class SkillResetGump : Gump
    {
        public SkillResetGump(Mobile from) : base(0, 0)
        {
            if (from == null)
            {
                from.CloseGump<ChoosePathGump>();
            }
            Closable = false;
            Disposable = true;
            Draggable = true;
            Resizable = false;
            AddPage(0);
            const int width = 500;
            const int height = 325;
            const int descriptionWidth = 450;
            const string description = "Which skill reset option do you wish to take?";
            List<ChoosePathGump.Option> options = new List<ChoosePathGump.Option>
            {
                new(1, 40, 80, 80, 80, "14 combat points per level"),
                new(2, 40, 120, 80, 120, "14 crafting points per level"),
                new(3, 40, 160, 80,160, "7 combat and 7 ranger points per level"),
                new(4, 40, 200, 80,200, "7 combat and 7 crafting points per level"),
                new(5, 40, 240, 80,240, "7 ranger and 7 crafting points per level"),
                new(6, 40, 280, 80,280, "4 ranger, 5 crafting and 5 combat points per level"),
            };
            AddImageTiled(0, 0, width, height, 0xA8E);
            AddImageTiled(0, 0, 20, height, 0x27A7);
            AddImageTiled(0, 0, width, 20, 0x27A7);
            AddImageTiled(width, 0, 20, height + 20, 0x27A7);
            AddImageTiled(0, height, width, 20, 0x27A7);

            AddHtml(40, 40, descriptionWidth, 150, $"<BASEFONT COLOR=#FFFFE5>{description}</FONT>");

            foreach (var option in options)
            {
                AddButton(option.ButtonX, option.ButtonY, 0xFA5, 0xFA7, option.ButtonId);
                AddHtml(option.X, option.Y, 450, 50, $"<BASEFONT COLOR=#FFFFE5>{option.Text}</FONT>");
            }
        }
        public override void OnResponse(NetState state, RelayInfo info)
        {
            PlayerMobile player = (PlayerMobile)state.Mobile;

            if (player != null)
            {
                if (info.ButtonID is >= 1 and <= 6)
                {
                    int combatRatio = 14;
                    int rangerRatio = 7;
                    int craftingRatio = 14;

                    switch (info.ButtonID)
                    {
                        case 1: // full combat
                            craftingRatio = 0;
                            rangerRatio = 0;
                            break;
                        case 2: // full crafting
                            combatRatio = 0;
                            rangerRatio = 0;
                            break;
                        case 3: // ranger
                            combatRatio = 7;
                            craftingRatio = 0;
                            break;
                        case 4: // combat crafter
                            combatRatio = 7;
                            craftingRatio = 7;
                            rangerRatio = 0;
                            break;
                        case 5: // ranger crafter
                            combatRatio = 0;
                            craftingRatio = 7;
                            break;
                        case 6: // hybrid
                            rangerRatio = 4;
                            craftingRatio = 5;
                            combatRatio = 5;
                            break;
                    }
                    player.ResetSkills(combatRatio, rangerRatio, craftingRatio);
                }
                player.CloseGump<SkillResetGump>();
            }
        }
    }
}
