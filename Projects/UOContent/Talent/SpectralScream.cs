using Server.Mobiles;
using Server.Spells;
using Server.Items;
using Server.Gumps;
using System;
using System.Linq;

namespace Server.Talent
{
    public class SpectralScream : BaseTalent, ITalent
    {
        public SpectralScream() : base()
        {
            BlockedBy = new Type[] { typeof(GreaterFireElemental) };
            TalentDependency = typeof(SummonerCommand);
            DisplayName = "Spectral scream";
            Description = "Fears surrounding enemies. Level decreases cooldown by 24s. 3 min cooldown.";
            CanBeUsed = true;
            GumpHeight = 230;
            AddEndY = 105;
            ImageID = 385;
        }
        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                if (from.Mana < 30) {
                    from.SendMessage("You require atleast 30 mana to use this talent");
                } else {
                    from.Mana -= 30;
                    from.RevealingAction();
                    from.PlaySound(0x380);
                    foreach (var other in from.GetMobilesInRange(6))
                    {
                        if (other == from || !other.CanBeHarmful(from, false) ||
                            Core.AOS && !other.InLOS(from))
                            {
                                continue;
                            }
                        Point3D location = other.Location;
                        Point3D newLocation = new Point3D(location.X + Utility.Random(5), location.Y + Utility.Random(5), location.Z);
                        int attempts = 0;
                        while (!other.InLOS(newLocation))
                        {
                            if (attempts > 10) {
                                newLocation = location;
                            }
                            newLocation = new Point3D(location.X + Utility.Random(5), location.Y + Utility.Random(5), location.Z);
                            attempts++;
                        }
                        if (other is BaseCreature creature) {
                            PathFollower path = new PathFollower(other, newLocation);
                            creature.AIObject.Path = path;
                        } else if (other is PlayerMobile player) {
                            player.Fear(Utility.Random(10));
                        }
                    }
                    OnCooldown = true;
                    Timer.StartTimer(TimeSpan.FromSeconds(180 - (Level * 24)), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }
    }
}
