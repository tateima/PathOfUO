using Server.Mobiles;
using Server.Spells;
using Server.Spells.Eighth;
using Server.Network;
using System;

namespace Server.Talent
{
    public class GreaterFireElemental : BaseTalent, ITalent
    {
        public GreaterFireElemental() : base()
        {
            BlockedBy = new Type[] { typeof(MasterOfDeath), typeof(HolyAvenger) };
            TalentDependency = typeof(DragonAspect);
            DisplayName = "Greater fire lord";
            MobilePercentagePerPoint = 15;
            CanBeUsed = true;
            Description = "Summon a fire lord to assist you for 2 minutes. 5 minute cooldown. Hits will be used if no mana is available.";
            ImageID = 30225;
            AddY = 130;
        }
        public override void OnUse(Mobile summoner)
        {
            if (!OnCooldown)
            {
                bool canCast = true;
                if (summoner.Mana < 50 && summoner.Hits >= 26)
                {
                    summoner.Hits -= 25;
                }
                else if (summoner.Mana > 50)
                {
                    summoner.Mana -= 50;
                }
                else
                {
                    canCast = false;
                    summoner.SendMessage("You need either 50 mana or 26 hitpoints to summon this fire lord.");
                }
                if (canCast)
                {
                    summoner.RevealingAction();
                    summoner.PublicOverheadMessage(MessageType.Spell, summoner.SpeechHue, true, "Kal Vas Xen Flam Apoch Gras", false);
                    // its a talent, no need for animation timer, just a single animation is fine
                    summoner.Animate(269, 7, 1, true, false, 0);

                    if (Core.AOS)
                    {
                        SpellHelper.Summon((BaseCreature)ScaleMobile(new SummonedFireElemental()), summoner, 0x217, TimeSpan.FromMinutes(2), false, false);
                    }
                    else
                    {
                        SpellHelper.Summon((BaseCreature)ScaleMobile(new FireElemental()), summoner, 0x217, TimeSpan.FromMinutes(2), false, false);
                    }
                    Timer.StartTimer(TimeSpan.FromMinutes(5), ExpireTalentCooldown, out _talentTimerToken);
                    OnCooldown = true;
                }
            }
        }
    }
}
