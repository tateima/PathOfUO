using Server.Mobiles;
using Server.Items;
using System;
using System.Collections.Generic;

namespace Server.Talent
{
    public class GroundSlam : BaseTalent, ITalent
    {
        private List<Mobile> m_SlowedMobiles = new List<Mobile>();
        public TimerExecutionToken _slowedMobileTimerToken;

        public GroundSlam() : base()
        {
            TalentDependency = typeof(TwoHandedMaceSpecialist);
            RequiredWeapon = new Type[] { typeof(WarHammer) };
            DisplayName = "Ground slam";
            CanBeUsed = true;
            Description = "Push back surrounding mobiles by 1-5 yards and slows them down, 30s cooldown.";
            ImageID = 369;
            GumpHeight = 75;
            AddEndY = 80;
        }
        public override void OnUse(Mobile mobile)
        {
            BaseWeapon weapon = mobile.Weapon as BaseWeapon;
            if (weapon is WarHammer)
            {
                base.OnUse(mobile);
            }
            else
            {
                mobile.SendMessage("You do not have a warhammer equipped.");
            }
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (Activated)
            {
                Activated = false;
                OnCooldown = true;
                List<Mobile> mobiles = (List<Mobile>)attacker.GetMobilesInRange(3);
                foreach (Mobile mobile in mobiles)
                {
                    if (mobile == attacker || !mobile.CanBeHarmful(attacker, false) ||
                            Core.AOS && !mobile.InLOS(attacker))
                    {
                        continue;
                    }
                    Point3D mobileLocation = mobile.Location;
                    Direction mobileDirection = attacker.GetDirectionTo(mobileLocation.X, mobileLocation.Y, false);
                    Point3D newMobileLocation;
                    newMobileLocation = mobileDirection switch
                    {
                        Direction.East => new Point3D(mobileLocation.X + Level, mobileLocation.Y, mobileLocation.Y),
                        Direction.West => new Point3D(mobileLocation.X - Level, mobileLocation.Y, mobileLocation.Y),
                        Direction.South => new Point3D(mobileLocation.X, mobileLocation.Y + Level, mobileLocation.Y),
                        _ => new Point3D(mobileLocation.X, mobileLocation.Y - Level, mobileLocation.Y)
                    };
                    int attempts = 10;
                    while (!mobile.InLOS(newMobileLocation))
                    {
                        if (attempts > 10) {
                            newMobileLocation = mobileLocation;
                        }
                        newMobileLocation = mobileDirection switch
                        {
                            Direction.East => new Point3D(mobileLocation.X - 1, mobileLocation.Y, mobileLocation.Y),
                            Direction.West => new Point3D(mobileLocation.X + 1, mobileLocation.Y, mobileLocation.Y),
                            Direction.South => new Point3D(mobileLocation.X, mobileLocation.Y - 1, mobileLocation.Y),
                            _ => new Point3D(mobileLocation.X, mobileLocation.Y + 1, mobileLocation.Y)
                        };
                        attempts++;
                    }
                    mobile.MoveToWorld(newMobileLocation, mobile.Map);
                    mobile.Damage(Level, attacker);
                    if (mobile is BaseCreature)
                    {
                        ((BaseCreature)mobile).ActiveSpeed = ((BaseCreature)mobile).ActiveSpeed / 2;
                        m_SlowedMobiles.Add(mobile);
                    } else if (mobile is PlayerMobile player) {
                        player.Slow(Utility.Random(10));
                    }
                }
                Timer.StartTimer(TimeSpan.FromSeconds(10), ExpireSlowedMobiles, out _slowedMobileTimerToken);
                Timer.StartTimer(TimeSpan.FromSeconds(30), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
        public void ExpireSlowedMobiles()
        {
            foreach(Mobile mobile in m_SlowedMobiles)
            {
                ((BaseCreature)mobile).ActiveSpeed = ((BaseCreature)mobile).ActiveSpeed * 2;
            }
        }
    }
}
