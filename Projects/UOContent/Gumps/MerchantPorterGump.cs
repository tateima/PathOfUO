using System;
using Server.Mobiles;
using Server.Talent;
using Server.Network;

namespace Server.Gumps
{
    class MerchantPorterGump : Gump
    {
        private BaseVendor m_Merchant;
        public TimerExecutionToken _merchantExecutionToken;
        public MerchantPorterGump(Mobile from) : base(0, 0)
        {
            Closable = true;
            Disposable = true;
            Draggable = true;
            Resizable = false;
            AddPage(0);
            AddImageTiled(0, 0, 160, 900, 0xA8E);
            AddImageTiled(0, 0, 20, 900, 0x27A7);
            AddImageTiled(0, 0, 160, 20, 0x27A7);
            AddImageTiled(160, 0, 20, 920, 0x27A7);
            AddImageTiled(0, 900, 160, 20, 0x27A7);
            Type[] npcTypes = MerchantPorter.NpcTypes;
            PlayerMobile player = (PlayerMobile)from;
            AddLabel(80, 20, 2049, "Choose an NPC to summon: ");
            int x = 40;
            int y = 40;
            for (int i = 0; i < npcTypes.Length; i++)
            {
                Type type = npcTypes[i];
                var npc = TalentConstructor.Construct(type) as BaseVendor;
                AddHtml(x, y, 80, 40, $"<BASEFONT COLOR=#FFFFE5>{type.Name}</FONT>");
                AddButton(x + 100, y + 4, 2223, 2223, 0 + i, GumpButtonType.Reply, 0);
                y += 40;
            }
        }

        public override void OnResponse(NetState state, in RelayInfo info)
        {
            PlayerMobile player = (PlayerMobile)state.Mobile;

            if (player != null)
            {
                var npc = TalentConstructor.Construct(MerchantPorter.NpcTypes[info.ButtonID]) as BaseVendor;
                m_Merchant = npc;
                Point3D location = player.Location;
                location.X += 3;
                if (npc != null)
                {
                    npc.MoveToWorld(location, player.Map);
                    npc.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                    npc.PlaySound(0x1FE);
                }

                player.CloseGump<MerchantPorterGump>();
                Timer.StartTimer(TimeSpan.FromSeconds(60), DeleteMerchant, out _merchantExecutionToken);
            }
        }
        public void DeleteMerchant()
        {
            m_Merchant.PlaySound(0x1FE);
            m_Merchant.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
            m_Merchant.Delete();
        }
    }
}
