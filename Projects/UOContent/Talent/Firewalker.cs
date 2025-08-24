using System;
using Server.Ethics;
using Server.Mobiles;
using Server.Spells.Fourth;

namespace Server.Talent
{
    public class Firewalker : BaseTalent
    {
        public Firewalker()
        {
            TalentDependencies = new[] { typeof(GreaterFireElemental) };
            DisplayName = "Fire walker";
            CanBeUsed = true;
            HasMovementEffect = true;
            Description =
                "Leave fire in your wake as you move.";
            AdditionalDetail = "The duration of this effect lasts 7 seconds per level.";
            ImageID = 430;
            MaxLevel = 1;
            CooldownSeconds = 120;
            ManaRequired = 40;
            AddEndY = 105;
        }

        public override void CheckMovementEffect(Mobile from)
        {
            if (Activated)
            {
                var damage = 2;
                if (from is PlayerMobile playerMobile)
                {
                    var fireAffinity = playerMobile.GetTalent(typeof(FireAffinity));
                    if (fireAffinity != null)
                    {
                        damage += fireAffinity.Level;
                    }
                    var warmth = playerMobile.GetTalent(typeof(Warmth));
                    if (warmth != null)
                    {
                        damage += warmth.Level;
                    }
                }
                var itemID = Utility.RandomBool() ? 0x398C : 0x3996;
                Effects.PlaySound(from.Location, from.Map, 0x20C);
                new FireFieldItem(itemID, from.Location, from, from.Map, TimeSpan.FromSeconds(Level * 7), Utility.RandomMinMax(-1, 1), damage);
            }
        }

        public void ExpireActivation()
        {
            Activated = false;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && from.Mana >= ManaRequired)
            {
                Activated = true;
                OnCooldown = true;
                Timer.StartTimer(TimeSpan.FromSeconds(20), ExpireActivation, out _talentTimerToken);
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }
    }
}
