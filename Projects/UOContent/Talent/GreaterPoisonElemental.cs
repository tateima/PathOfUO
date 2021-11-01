using Server.Mobiles;
using Server.Spells;
using Server.Network;
using System;

namespace Server.Talent
{
    public class GreaterPoisonElemental : BaseTalent, ITalent
    {
        public GreaterPoisonElemental() : base()
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
        public override void OnUse(Mobile summoner)
        {
            if (!OnCooldown)
            {
                bool canCast = true;
                if (summoner.Mana > 65)
                {
                    summoner.Mana -= 65;
                }
                else
                {
                    canCast = false;
                    summoner.SendMessage("You need 65 mana to summon this poison lord.");
                }
                if (canCast)
                {
                    summoner.RevealingAction();
                    summoner.PublicOverheadMessage(MessageType.Spell, summoner.SpeechHue, true, "Nox Vas Xen Apoch Gras", false);
                    // its a talent, no need for animation timer, just a single animation is fine
                    summoner.Animate(269, 7, 1, true, false, 0);
                    SpellHelper.Summon(new PoisonElemental(), summoner, 0x217, TimeSpan.FromMinutes(2), false, false); // dont scale because theyre already quite powerful
                    Timer.StartTimer(TimeSpan.FromMinutes(5), ExpireTalentCooldown, out _talentTimerToken);
                    OnCooldown = true;
                }
            }
        }
    }
}
