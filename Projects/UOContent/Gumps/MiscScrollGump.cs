using Server.Network;

namespace Server.Gumps
{
    public class MiscScrollGump : Gump
    {
        public MiscScrollGump(Mobile from, string title, string[] hooks, int[] imageIds = null) : base(0, 0)
        {
            if (from == null)
            {
                from.CloseGump<MiscScrollGump>();
            }
            Closable = true;
            Disposable = true;
            Draggable = true;
            Resizable = false;
            AddPage(0);
            AddImage(0, 0, 0x63C);
            AddImage(0, 142, 0x63D);
            AddImage(0, 284, 0x63E);
            AddImage(0, 426, 0x63D);
            AddImage(0, 568, 0x63E);
            AddImage(0, 710, 0x63F);
            AddImage(25, 700, 0x197);
            // close button
            AddButton(0, 0, 40015, 40015, 1002, GumpButtonType.Reply, 0);
            int y = 60;
            AddHtml(25, y, 395, 50, $"<BASEFONT COLOR=#000000>{title}</FONT>");
            y += 35;
            for(int i = 0; i < hooks.Length; i++) {
                AddHtml(25, y, 365, 50, $"<BASEFONT COLOR=#000000>{hooks[i]}</FONT>");
                if (imageIds != null && i < imageIds.Length) {
                    if (imageIds[i] != 0) {
                        y += 35;
                        AddImage(25, y, 365, imageIds[i]);
                    }
                }
                y += 35;
            }
        }
        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (state.Mobile != null)
            {
                if (info.ButtonID == 1002)
                {
                    state.Mobile.CloseGump<MiscScrollGump>();
                    return;
                }
            }
        }
    }
}
