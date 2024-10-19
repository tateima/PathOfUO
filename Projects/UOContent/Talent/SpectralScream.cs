using System;
using Server.Mobiles;

namespace Server.Talent
{
    public class SpectralScream : BaseTalent
    {
        public SpectralScream()
        {
            BlockedBy = new[] { typeof(GreaterFireElemental) };
            TalentDependencies = new[] { typeof(SummonerCommand) };
            DisplayName = "Spectral scream";
            Description = "Fears surrounding enemies. Level decreases cooldown by 24s.";
            AdditionalDetail = "The fear effect lasts 10 seconds.";
            CooldownSeconds = 180;
            ManaRequired = 30;
            CanBeUsed = true;
            GumpHeight = 230;
            AddEndY = 105;
            ImageID = 385;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                if (from.Mana < ManaRequired)
                {
                    from.SendMessage($"You require at least {ManaRequired.ToString()} mana to use this talent.");
                }
                else
                {
                    ApplyManaCost(from);
                    from.RevealingAction();
                    from.PlaySound(0x380);
                    foreach (var other in from.GetMobilesInRange(6))
                    {
                        if (other == from || !other.CanBeHarmful(from, false) ||
                            Core.AOS && !other.InLOS(from))
                        {
                            continue;
                        }
                        if (other is BaseCreature creature)
                        {
                            creature.Fear(10);
                            creature.BeginFlee(TimeSpan.FromSeconds(10));
                        }
                        else if (other is PlayerMobile player)
                        {
                            player.Fear(Utility.Random(10));
                        }
                    }

                    OnCooldown = true;
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds - Level * 24), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }
    }
}
