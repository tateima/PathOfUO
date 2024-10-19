using Server.Network;

namespace Server.Gumps
{
    public class MiscScrollGump : Gump
    {
        public MiscScrollGump(string title, string[] hooks, int talentImageId) : base(0, 0)
        {
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
            AddImage(25, 700, talentImageId);
            // close button
            AddButton(0, 0, 40015, 40015, 1002);
            int y = 65;
            AddHtml(25, y, 395, 50, $"<BASEFONT COLOR=#000000>{title}</FONT>");
            y += 45;
            for(int i = 0; i < hooks.Length; i++) {
                AddHtml(25, y, 365, 65, $"<BASEFONT COLOR=#000000>{hooks[i]}</FONT>");
                y += 65;
            }
        }
        public override void OnResponse(NetState state, in RelayInfo info)
        {
            if (state.Mobile != null)
            {
                if (info.ButtonID == 1002)
                {
                    state.Mobile.CloseGump<MiscScrollGump>();
                }
            }
        }
    }
}
