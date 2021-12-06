using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class GroundSlam : BaseTalent
    {
        private readonly List<Mobile> m_SlowedMobiles = new();

        public GroundSlam()
        {
            TalentDependency = typeof(TwoHandedMaceSpecialist);
            RequiredWeapon = new[] { typeof(WarHammer) };
            DisplayName = "Ground slam";
            CanBeUsed = true;
            Description = "Push back surrounding mobiles by 1-5 yards and slows them down, 30s cooldown.";
            ImageID = 369;
            GumpHeight = 75;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (Activated)
            {
                Activated = false;
                OnCooldown = true;
                var mobiles = attacker.GetMobilesInRange(3);
                foreach (var mobile in mobiles)
                {
                    if (mobile == attacker || !mobile.CanBeHarmful(attacker, false) ||
                        Core.AOS && !mobile.InLOS(attacker))
                    {
                        continue;
                    }

                    var mobileLocation = mobile.Location;
                    var mobileDirection = attacker.GetDirectionTo(mobileLocation.X, mobileLocation.Y);
                    var newMobileLocation = mobileDirection switch
                    {
                        Direction.East  => new Point3D(mobileLocation.X + Level, mobileLocation.Y, mobileLocation.Y),
                        Direction.West  => new Point3D(mobileLocation.X - Level, mobileLocation.Y, mobileLocation.Y),
                        Direction.South => new Point3D(mobileLocation.X, mobileLocation.Y + Level, mobileLocation.Y),
                        _               => new Point3D(mobileLocation.X, mobileLocation.Y - Level, mobileLocation.Y)
                    };
                    var attempts = 10;
                    while (!mobile.InLOS(newMobileLocation))
                    {
                        if (attempts > 10)
                        {
                            break;
                        }

                        newMobileLocation = mobileDirection switch
                        {
                            Direction.East  => new Point3D(mobileLocation.X - 1, mobileLocation.Y, mobileLocation.Y),
                            Direction.West  => new Point3D(mobileLocation.X + 1, mobileLocation.Y, mobileLocation.Y),
                            Direction.South => new Point3D(mobileLocation.X, mobileLocation.Y - 1, mobileLocation.Y),
                            _               => new Point3D(mobileLocation.X, mobileLocation.Y + 1, mobileLocation.Y)
                        };
                        attempts++;
                    }

                    mobile.MoveToWorld(newMobileLocation, mobile.Map);
                    mobile.Damage(Level, attacker);
                    if (mobile is BaseCreature creature)
                    {
                        creature.ActiveSpeed /= 2;
                        m_SlowedMobiles.Add(creature);
                    }
                    else if (mobile is PlayerMobile player)
                    {
                        player.Slow(Utility.Random(10));
                    }
                }

                Timer.StartTimer(TimeSpan.FromSeconds(10), ExpireSlowedMobiles, out _);
                Timer.StartTimer(TimeSpan.FromSeconds(30), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public override void OnUse(Mobile from)
        {
            var weapon = from.Weapon as BaseWeapon;
            if (weapon is WarHammer)
            {
                base.OnUse(from);
            }
            else
            {
                from.SendMessage("You do not have a war hammer equipped.");
            }
        }

        public void ExpireSlowedMobiles()
        {
            foreach (var mobile in m_SlowedMobiles)
            {
                ((BaseCreature)mobile).ActiveSpeed *= 2;
            }
        }
    }
}
