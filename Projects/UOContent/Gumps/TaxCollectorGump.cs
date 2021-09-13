using System;
using System.Collections.Generic;
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
            if (from == null)
            {
                from.CloseGump<TaxCollectorGump>();
            }
            PlayerMobile player = (PlayerMobile)from;
            BaseTalent taxCollector = player.GetTalent(typeof(TaxCollector));

            Closable = true;
            Disposable = true;
            Draggable = true;
            Resizable = false;
            
            if (taxCollector != null)
            {
                AddPage(0);
                AddImageTiled(0, 0, 940, 900, 0xA8E);
                AddImageTiled(0, 0, 20, 900, 0x27A7);
                AddImageTiled(0, 0, 940, 20, 0x27A7);
                AddImageTiled(940, 0, 20, 920, 0x27A7);
                AddImageTiled(0, 900, 940, 20, 0x27A7);

                List<Mobile> debtees = player.AllDebtees;
                AddLabel(80, 20, 2049, "Add or remove a tenant to receive tax from them: ");
                int x = 40;
                int y = 40;
                if (taxCollector.Level < debtees.Count)
                {
                    AddButton(x + 100, y + 4, 5534, 5533, 1000, GumpButtonType.Reply, 0); // add
                    y += 40;
                }
                DateTime now = DateTime.Now;
                for (int i = 0; i < debtees.Count; i++)
                {
                    BaseVendor vendor = (BaseVendor)debtees[i];
                    AddHtml(x, y, 40, 40, $"<BASEFONT COLOR=#FFFFE5>{vendor.Name} {vendor.Title}</FONT>");
                    if (now >= vendor.NextCollectionTime) // only allow them to be removed if they can be collected from to prevent exploitation
                    {
                        AddButton(x + 100, y + 4, 40015, 40015, 0 + i, GumpButtonType.Reply, 0); // remove
                    }
                    y += 40;
                }
            } else
            {
                from.CloseGump<TaxCollectorGump>();
            }
            
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            PlayerMobile player = (PlayerMobile)state.Mobile;

            if (player != null)
            {
                if (info.ButtonID < 1000)
                {
                    var npc = (BaseVendor)player.AllDebtees[info.ButtonID];
                    npc.TaxCollectorSerial = 0;
                } else if (info.ButtonID == 1000) // add a new one
                {
                    state.Mobile.SendMessage("Whom do you wish to invest with?");
                    player.Target = new InternalTarget(state.Mobile);
                }
                player.SendGump(new TaxCollectorGump(player));
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
                if (targeted is Mobile target && target is BaseVendor vendor && vendor.TaxCollectorSerial == 0)
                {
                    vendor.TaxCollectorSerial = from.Serial.ToInt32();
                    vendor.NextCollectionTime = DateTime.Now;
                    ((PlayerMobile)m_TaxCollector).AllDebtees.Add((Mobile)targeted);
                } else
                {
                    if (targeted is BaseVendor)
                    {
                        from.SendMessage("This vendor already has an assigned investor.");
                    } else
                    {
                        from.SendMessage("You cannot collect tax off that target.");
                    }
                    
                }
            }
        }
    }
}
