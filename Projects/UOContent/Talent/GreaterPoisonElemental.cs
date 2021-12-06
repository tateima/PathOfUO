using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Talent
{
    public class GreaterPoisonElemental : BaseTalent
    {
        public GreaterPoisonElemental()
        {
            TalentDependency = typeof(WyvernAspect);
            DisplayName = "Poison Elemental";
            MobilePercentagePerPoint = 15;
            CanBeUsed = true;
            Description = "Summon a poison elemental to assist you for 2 minutes. 5 minute cooldown. Mana is required.";
            ImageID = 390;
            MaxLevel = 1;
            GumpHeight = 230;
            AddEndY = 125;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                var canCast = true;
                if (from.Mana > 65)
                {
                    from.Mana -= 65;
                }
                else
                {
                    canCast = false;
                    from.SendMessage("You need 65 mana to summon this poison lord.");
                }

                if (canCast)
                {
                    from.RevealingAction();
                    from.PublicOverheadMessage(
                        MessageType.Spell,
                        from.SpeechHue,
                        true,
                        "Nox Vas Xen Apoch Gras",
                        false
                    );
                    // its a talent, no need for animation timer, just a single animation is fine
                    from.Animate(269, 7, 1, true, false, 0);
                    SpellHelper.Summon(
                        new PoisonElemental(),
                        from,
                        0x217,
                        TimeSpan.FromMinutes(2),
                        false,
                        false
                    ); // dont scale because they're already quite powerful
                    Timer.StartTimer(TimeSpan.FromMinutes(5), ExpireTalentCooldown, out _talentTimerToken);
                    OnCooldown = true;
                }
            }
        }
    }
}
