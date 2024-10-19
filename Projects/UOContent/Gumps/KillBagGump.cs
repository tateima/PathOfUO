using Server.ContextMenus;
using Server.Network;
using Server.Mobiles;
using System.Linq;

namespace Server.Gumps
{
    public class KillBagMenuEntry : ContextMenuEntry
    {
        private readonly PlayerMobile _from;

        public KillBagMenuEntry(PlayerMobile from)
            : base(1116798, -1) // Creature kills
        {
            _from = from;
        }

        public override void OnClick(Mobile from, IEntity target)
        {
            _from.CloseGump<KillBagGump>();
            _from.SendGump(new KillBagGump(_from, 1, true));
        }
    }

    public class KillBagGump : Gump
    {
        public KillBagGump(Mobile from, int page = 1, bool playSound = false) : base(0, 0)
        {
            if (from is PlayerMobile player)
            {
                Closable = true;
                Disposable = true;
                Draggable = true;
                Resizable = false;
                AddImage(40, 36, 500);
                var kills = player.KillBag.ToList();
                if (kills.Count > 0)
                {
                    kills.Sort((x, y) => x.Value.CompareTo(y.Value));
                    int totalEntries = kills.Count;
                    int totalPages = totalEntries / 20;
                    if (totalPages <= 0)
                    {
                        totalPages = 1;
                    }
                    if (playSound)
                    {
                        from.SendSound(0x55);
                    }
                    var paginatedKills = kills.Skip((page-1)*20).Take(20);
                    int y = 50;
                    int x = 80;
                    int i = 0;
                    if (totalPages > 1)
                    {
                        if (page > 1)
                        {
                            AddButton(125, 14, 2205, 2205, 1 - page, GumpButtonType.Page);
                        }
                        AddButton(393, 14, 2206, 2206, 1 + page, GumpButtonType.Page);
                    }
                    foreach (var kill in paginatedKills)
                    {
                        AddLabel(x, y, 0, $"{kill.Key.Name}: {kill.Value}");
                        if (i == 9 && page == 1)
                        {
                            page = 2;
                            // go to next page
                            i = 0;
                            x = 270;
                            y = 40;
                        }
                        y += 20;
                        i++;
                    }
                }
                else
                {
                    player.CloseGump<KillBagGump>();
                }
            }
        }

        public override void OnResponse(NetState state, in RelayInfo info)
        {
            PlayerMobile player = (PlayerMobile)state.Mobile;
            int page = 1;
            if (player != null)
            {
                if (info.ButtonID > 0)
                {
                    page = info.ButtonID;
                    player.SendGump(new KillBagGump(player, page, true));
                }
            }
        }
    }
}
