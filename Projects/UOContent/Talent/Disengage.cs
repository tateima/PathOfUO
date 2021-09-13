using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class Disengage : BaseTalent, ITalent
    {
        public Disengage() : base()
        {
            TalentDependency = typeof(ArcherFocus);
            CanBeUsed = true;
            DisplayName = "Disengage";
            Description = "Leaps backwards from enemies between 2-8 yards. 2 minute cooldown.";
            ImageID = 380;
            GumpHeight = 85;
            AddEndY = 65;
        }
        public override void OnUse(Mobile mobile)
        {
            if (!OnCooldown)
            {
                Point3D attackerPosition = mobile.Location;
                if (mobile.Direction != Direction.Running)
                {
                    int distance = Level + Utility.Random(1, 3);
                    Point3D newLocation;
                    newLocation = mobile.Direction switch
                    {
                        Direction.East => new Point3D(attackerPosition.X + distance, attackerPosition.Y, attackerPosition.Y),
                        Direction.West => new Point3D(attackerPosition.X - distance, attackerPosition.Y, attackerPosition.Y),
                        Direction.South => new Point3D(attackerPosition.X, attackerPosition.Y + distance, attackerPosition.Y),
                        _ => new Point3D(attackerPosition.X, attackerPosition.Y - distance, attackerPosition.Y)
                    };

                    while (!mobile.InLOS(newLocation))
                    {
                        newLocation = mobile.Direction switch
                        {
                            Direction.East => new Point3D(attackerPosition.X - 1, attackerPosition.Y, attackerPosition.Y),
                            Direction.West => new Point3D(attackerPosition.X + 1, attackerPosition.Y, attackerPosition.Y),
                            Direction.South => new Point3D(attackerPosition.X, attackerPosition.Y - 1, attackerPosition.Y),
                            _ => new Point3D(attackerPosition.X, attackerPosition.Y + 1, attackerPosition.Y)
                        };
                    }
                    mobile.MoveToWorld(newLocation, mobile.Map);
                    OnCooldown = true;
                    mobile.PlaySound(0x525);
                    Timer.StartTimer(TimeSpan.FromSeconds(120), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }
    }
}
