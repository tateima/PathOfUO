using Server.Network;
using Server.Mobiles;

namespace Server.Gumps
{
    public class ChoosePathGump : Gump
    {
        public ChoosePathGump(Mobile from) : base(0, 0)
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
            AddImageTiled(0, 0, 320, 160, 0xA8E);
            AddImageTiled(0, 0, 20, 160, 0x27A7);
            AddImageTiled(0, 0, 320, 20, 0x27A7);
            AddImageTiled(320, 0, 20, 180, 0x27A7);
            AddImageTiled(0, 160, 320, 20, 0x27A7);
            AddImage(200, 40, 0xAAE);

            AddHtml(40, 40, 150, 150, $"<BASEFONT COLOR=#FFFFE5>Do thou want to live a murderous path? If you say no you cannot turn back.</FONT>");

            AddButton(40, 120, 0xFA5, 0xFA7, 1);
            AddHtml(80, 120, 100, 50, $"<BASEFONT COLOR=#FFFFE5>Yes</FONT>");

            AddButton(110, 120, 0xFA5, 0xFA7, 2);
            AddHtml(150, 120, 100, 50, $"<BASEFONT COLOR=#FFFFE5>No</FONT>");
        }
        public override void OnResponse(NetState state, RelayInfo info)
        {
            PlayerMobile player = (PlayerMobile)state.Mobile;

            if (player != null)
            {
                if (info.ButtonID == 1)
                {
                    Point3D point = new Point3D(2681, 2223, 0);
                    player.MoveToWorld(point, Map.Trammel); // bucs den
                    player.Kills = 5; // these dont decay
                    player.ShortTermMurders = 5; // these dont decay, perma red
                }
                player.CloseGump<ChoosePathGump>();
            }
        }
    }
}
