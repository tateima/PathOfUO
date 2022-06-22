using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Talent
{
    public class MerchantPorter : BaseTalent
    {
        public static readonly Type[] NpcTypes =
        {
            typeof(Alchemist),
            typeof(Architect),
            typeof(Blacksmith),
            typeof(Provisioner),
            typeof(Cook),
            typeof(Herbalist),
            typeof(Bowyer),
            typeof(Carpenter),
            typeof(Tailor),
            typeof(Tinker),
            typeof(Scribe),
            typeof(Mage),
            typeof(LeatherWorker),
            typeof(Banker),
            typeof(Healer),
            typeof(Bard),
            typeof(InnKeeper),
            typeof(Veterinarian),
            typeof(Jeweler),
            typeof(GolemCrafter),
            typeof(Furtrader)
        };

        public MerchantPorter()
        {
            BlockedBy = new[] { typeof(SmoothTalker) };
            DisplayName = "Merchant porter";
            CanBeUsed = true;
            CooldownSeconds = 600;
            Description =
                "Teleport an npc vendor to player position for 1 minute. Cooldown decreases by 1 minute per level.";
            ImageID = 349;
            GumpHeight = 230;
            AddEndY = 105;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                OnCooldown = true;
                from.SendGump(new MerchantPorterGump(from));
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds - Level), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}
