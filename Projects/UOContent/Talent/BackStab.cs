using System;
using Server.Items;

namespace Server.Talent
{
    public class BackStab : BaseTalent
    {
        public BackStab()
        {
            TalentDependency = typeof(FencingSpecialist);
            RequiredWeaponSkill = SkillName.Fencing;
            DisplayName = "Back stab";
            CanBeUsed = true;
            Description =
                "When activated, the next miss from opponent causes you to go behind them and hit for extra damage.";
            ImageID = 118;
            GumpHeight = 75;
            AddEndY = 95;
        }

        public override void OnUse(Mobile from)
        {
            var weapon = from.Weapon as BaseWeapon;
            if (weapon?.Skill == RequiredWeaponSkill && weapon is not BaseSpear)
            {
                base.OnUse(from);
            }
            else
            {
                from.SendMessage("You do not have a one handed fencing weapon equipped.");
            }
        }

        public override void CheckDefenderMissEffect(Mobile attacker, Mobile target)
        {
            var attackerPosition = attacker.Location;
            if (Activated && attacker.Direction != Direction.Running && target.Weapon is BaseMeleeWeapon)
            {
                Activated = false;
                OnCooldown = true;
                var attackerDirection = target.GetDirectionTo(attackerPosition.X, attackerPosition.Y);
                var newDefendingDirection = attackerDirection switch
                {
                    Direction.West  => Direction.East,
                    Direction.East  => Direction.West,
                    Direction.North => Direction.South,
                    _               => Direction.North
                };
                var newDefendingLocation = newDefendingDirection switch
                {
                    Direction.East  => new Point3D(attackerPosition.X - 2, attackerPosition.Y, attackerPosition.Y),
                    Direction.West  => new Point3D(attackerPosition.X + 2, attackerPosition.Y, attackerPosition.Y),
                    Direction.South => new Point3D(attackerPosition.X, attackerPosition.Y - 2, attackerPosition.Y),
                    _               => new Point3D(attackerPosition.X, attackerPosition.Y + 2, attackerPosition.Y)
                };

                if (target.InLOS(newDefendingLocation))
                {
                    target.MoveToWorld(newDefendingLocation, target.Map);
                    target.Direction = newDefendingDirection;
                    target.PlaySound(0x525);
                    var weapon = (BaseMeleeWeapon)target.Weapon;
                    attacker.Damage(weapon.MaxDamage + Level, target);
                }
                else
                {
                    target.SendMessage("Your back stab execution failed due to nearby objects");
                }

                Timer.StartTimer(TimeSpan.FromSeconds(45), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}
