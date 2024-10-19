using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Talent;
using Server.Network;
using Server.Targeting;

namespace Server.Gumps
{
    class TaxCollectorGump : Gump
    {
        public TaxCollectorGump(Mobile from) : base(0, 0)
        {
            PlayerMobile player = (PlayerMobile)from;
            BaseTalent taxCollector = player.GetTalent(typeof(TaxCollector));

            Closable = false;
            Disposable = true;
            Draggable = true;
            Resizable = false;

            if (taxCollector != null)
            {
                AddPage(0);
                AddImageTiled(0, 0, 400, 680, 0xA8E);
                AddImageTiled(0, 0, 20, 680, 0x27A7);
                AddImageTiled(0, 0, 400, 20, 0x27A7);
                AddImageTiled(400, 0, 20, 680, 0x27A7);
                AddImageTiled(0, 680, 420, 20, 0x27A7);
                AddButton(395, 0, 40015, 40015, 1002);
                AddButton(355, 0, 4029, 4030, 1003);
                List<Mobile> debtees = player.AllDebtees;
                AddLabel(40, 50, 2049, "Add or remove a tenant to receive tax from them: ");
                int x = 40;
                int y = 50;
                if (taxCollector.Level > debtees.Count)
                {
                    AddButton(x, y + 30, 5534, 5533, 1000); // add
                    y += 60;
                }
                DateTime now = DateTime.Now;
                for (int i = 0; i < debtees.Count; i++)
                {
                    BaseVendor vendor = (BaseVendor)debtees[i];
                    AddHtml(x, y, 150, 60, $"<BASEFONT COLOR=#FFFFE5>{vendor.Name} {vendor.Title}</FONT>");
                    if (now >= vendor.NextCollectionTime) // only allow them to be removed if they can be collected from to prevent exploitation
                    {
                        AddButton(x + 170, y + 4, 40015, 40015, 0 + i); // remove
                    }
                    else
                    {
                        AddHtml(x + 170, y, 120, 60, $"<BASEFONT COLOR=#FFFFE5>Cooldown {WaitTeleporter.FormatTime(vendor.NextCollectionTime - now)}</FONT>");
                    }
                    y += 60;
                }
            } else
            {
                from.CloseGump<TaxCollectorGump>();
            }

        }

        public override void OnResponse(NetState state, in RelayInfo info)
        {
            PlayerMobile player = (PlayerMobile)state.Mobile;

            if (player != null)
            {
                if (info.ButtonID < 1000)
                {
                    var npc = (BaseVendor)player.AllDebtees[info.ButtonID];
                    npc.TaxCollectorSerial = 0;
                    player.AllDebtees.Remove(npc);
                    player.SendGump(new TaxCollectorGump(player));
                } else if (info.ButtonID == 1000) // add a new one
                {
                    state.Mobile.SendMessage("Whom do you wish to invest with?");
                    player.Target = new InternalTarget(state.Mobile);
                }
                else
                {
                    player.CloseGump<TaxCollectorGump>();
                    if (info.ButtonID == 1003)
                    {
                        player.SendGump(new TaxCollectorGump(player));
                    }
                }
            }
        }
        private class InternalTarget : Target
        {
            private readonly Mobile m_TaxCollector;
            public InternalTarget(Mobile taxCollector) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                m_TaxCollector = taxCollector;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is not Oracle)
                {
                    if (targeted is BaseVendor { TaxCollectorSerial: 0 } vendor)
                    {
                        vendor.TaxCollectorSerial = from.Serial.ToInt32();
                        vendor.NextCollectionTime = DateTime.Now;
                        ((PlayerMobile)m_TaxCollector).AllDebtees.Add(vendor);
                    } else
                    {
                        from.SendMessage(
                            targeted is BaseVendor
                                ? "This vendor already has an assigned investor."
                                : "You cannot collect tax off that target."
                        );
                    }
                }
                else
                {
                    from.SendMessage("You cannot collect tax off this");
                }

                from.CloseGump<TaxCollectorGump>();
                from.SendGump(new TaxCollectorGump(from));
            }
        }
    }
}
