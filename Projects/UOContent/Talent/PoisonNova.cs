using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Talent
{
    public class PoisonNova : BaseTalent
    {
        public PoisonNova()
        {
            TalentDependencies = new[] { typeof(WyvernAspect) };
            DisplayName = "Poison nova";
            CanBeUsed = true;
            ManaRequired = 40;
            CooldownSeconds = 120;
            ManaRequired = 30;
            Description = "Unleash poisonous gas in an area around you.";
            AdditionalDetail = "Each level increases the area effect by 1 yard and poisoning chance. This skill requires at least 85 poisoning and 60 magery.";
            MaxLevel = 2;
            ImageID = 434;
            MaxLevel = 1;
            GumpHeight = 230;
            AddEndY = 105;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Poisoning.Base >= 85 && mobile.Skills.Magery.Base >= 60;

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from) && from.Mana >= ManaRequired)
            {
                ApplyManaCost(from);
                from.RevealingAction();
                from.PublicOverheadMessage(
                    MessageType.Spell,
                    from.SpeechHue,
                    true,
                    "Nox Ara Set Dei",
                    false
                );
                from.Animate(269, 7, 1, true, false, 0);
                var mobiles = from.GetMobilesInRange(5 + Level);
                foreach (var mobile in mobiles)
                {
                    if (mobile == from || !mobile.CanBeHarmful(from, false) ||
                        Core.AOS && !mobile.InLOS(from))
                    {
                        continue;
                    }
                    int damage = mobile.Poisoned ? Utility.RandomMinMax(5, 7) : Utility.RandomMinMax(3, 5);
                    Effects.SendLocationEffect(mobile, 0x3924, 16, 10, 0);
                    mobile.PlaySound(0x205);
                    if (Core.AOS)
                    {
                        AOS.Damage(mobile, damage, 0, 0, 0, 100, 0);
                    }
                    else
                    {
                        mobile.Damage(damage, from);
                    }
                    if (Utility.Random(100) < Level * 10 && !mobile.Poisoned || mobile is BaseCreature creature && !BaseInstrument.IsPoisonImmune(creature))
                    {
                        Poison poison = Level > 1 ? Poison.Deadly : Poison.Greater;
                        poison = Level > 2 ? Poison.Lethal : poison;
                        mobile.ApplyPoison(from, poison);
                    }
                }
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }
    }
}
