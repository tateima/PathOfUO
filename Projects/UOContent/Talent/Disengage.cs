using System;

namespace Server.Talent
{
    public class Disengage : BaseTalent
    {
        public Disengage()
        {
            TalentDependency = typeof(ArcherFocus);
            CanBeUsed = true;
            DisplayName = "Disengage";
            Description = "Leaps backwards from enemies between 2-8 yards. 2 minute cooldown.";
            ImageID = 380;
            GumpHeight = 85;
            AddEndY = 85;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                var attackerPosition = from.Location;
                if (from.Direction != Direction.Running)
                {
                    var distance = Level + Utility.Random(1, 3);
                    var newLocation = from.Direction switch
                    {
                        Direction.East => new Point3D(attackerPosition.X + distance, attackerPosition.Y, attackerPosition.Y),
                        Direction.West => new Point3D(attackerPosition.X - distance, attackerPosition.Y, attackerPosition.Y),
                        Direction.South => new Point3D(
                            attackerPosition.X,
                            attackerPosition.Y + distance,
                            attackerPosition.Y
                        ),
                        _ => new Point3D(attackerPosition.X, attackerPosition.Y - distance, attackerPosition.Y)
                    };

                    while (!from.InLOS(newLocation))
                    {
                        newLocation = from.Direction switch
                        {
                            Direction.East  => new Point3D(attackerPosition.X - 1, attackerPosition.Y, attackerPosition.Y),
                            Direction.West  => new Point3D(attackerPosition.X + 1, attackerPosition.Y, attackerPosition.Y),
                            Direction.South => new Point3D(attackerPosition.X, attackerPosition.Y - 1, attackerPosition.Y),
                            _               => new Point3D(attackerPosition.X, attackerPosition.Y + 1, attackerPosition.Y)
                        };
                    }

                    from.MoveToWorld(newLocation, from.Map);
                    OnCooldown = true;
                    from.PlaySound(0x525);
                    Timer.StartTimer(TimeSpan.FromSeconds(120), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }
    }
}
