using Server.Mobiles;
using Server.Spells;
using Server.Gumps;
using Server.Network;
using System;

namespace Server.Talent
{
    public class MerchantPorter : BaseTalent, ITalent
    {
        private Mobile m_Mobile;

        public static readonly Type[] NpcTypes =
        {
            typeof(Alchemist),
            typeof(Architect),
            typeof(Blacksmith),
            typeof(Bowyer),
            typeof(Carpenter),
            typeof(Tailor),
            typeof(Tinker),
            typeof(Scribe),
            typeof(LeatherWorker),
            typeof(Banker)
        };

        public MerchantPorter() : base()
        {
            BlockedBy = new Type[] { typeof(SmoothTalker) };
            DisplayName = "Merchant porter";
            CanBeUsed = true;
            Description = "Teleport an npc vendor to player position for 1 minute. 10 minute cooldown, decreases by 1 minutes per level.";
            ImageID = 349;
            GumpHeight = 230;
            AddEndY = 105;
        }

        public override void OnUse(Mobile mobile)
        {
            if (!OnCooldown)
            {
                m_Mobile = mobile;
                OnCooldown = true;
                mobile.SendGump(new MerchantPorterGump(mobile));
                Timer.StartTimer(TimeSpan.FromMinutes(10-Level), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}
