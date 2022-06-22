using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Talent
{
    public class SpectralScream : BaseTalent
    {
        public SpectralScream()
        {
            BlockedBy = new[] { typeof(GreaterFireElemental) };
            TalentDependency = typeof(SummonerCommand);
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
            if (!OnCooldown)
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

                        var location = other.Location;
                        var newLocation = new Point3D(
                            location.X + Utility.RandomMinMax(-15, 15),
                            location.Y + Utility.RandomMinMax(-15, 15),
                            location.Z
                        );
                        var attempts = 0;
                        while (!other.InLOS(newLocation))
                        {
                            if (attempts > 10)
                            {
                                break;
                            }

                            newLocation = new Point3D(
                                location.X + Utility.RandomMinMax(-15, 15),
                                location.Y + Utility.RandomMinMax(-15, 15),
                                location.Z
                            );
                            attempts++;
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
        }
    }
}
