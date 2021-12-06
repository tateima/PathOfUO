using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Talent
{
    public class GreaterFireElemental : BaseTalent
    {
        public GreaterFireElemental()
        {
            BlockedBy = new[] { typeof(MasterOfDeath), typeof(HolyAvenger) };
            TalentDependency = typeof(DragonAspect);
            DisplayName = "Greater fire lord";
            MobilePercentagePerPoint = 15;
            CanBeUsed = true;
            Description =
                "Summon a fire lord to assist you for 2 minutes. 5 minute cooldown. Hits will be used if no mana is available.";
            ImageID = 347;
            GumpHeight = 230;
            AddEndY = 145;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                var canCast = true;
                if (from.Mana < 50 && from.Hits >= 26)
                {
                    from.Hits -= 25;
                }
                else if (from.Mana > 50)
                {
                    from.Mana -= 50;
                }
                else
                {
                    canCast = false;
                    from.SendMessage("You need either 50 mana or 26 hitpoints to summon this fire lord.");
                }

                if (canCast)
                {
                    from.RevealingAction();
                    from.PublicOverheadMessage(
                        MessageType.Spell,
                        from.SpeechHue,
                        true,
                        "Kal Vas Xen Flam Apoch Gras",
                        false
                    );
                    // its a talent, no need for animation timer, just a single animation is fine
                    from.Animate(269, 7, 1, true, false, 0);

                    if (Core.AOS)
                    {
                        var creature = (BaseCreature)ScaleMobile(new SummonedFireElemental());
                        creature.Name = "a greater fire lord";
                        SpellHelper.Summon(creature, from, 0x217, TimeSpan.FromMinutes(2), false, false);
                    }
                    else
                    {
                        var creature = (BaseCreature)ScaleMobile(new FireElemental());
                        creature.Name = "a greater fire lord";
                        SpellHelper.Summon(creature, from, 0x217, TimeSpan.FromMinutes(2), false, false);
                    }

                    Timer.StartTimer(TimeSpan.FromMinutes(5), ExpireTalentCooldown, out _talentTimerToken);
                    OnCooldown = true;
                }
            }
        }
    }
}
