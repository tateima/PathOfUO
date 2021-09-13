using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class BackStab : BaseTalent, ITalent
    {
        public BackStab() : base()
        {
            TalentDependency = typeof(FencingSpecialist);
            RequiredWeaponSkill = SkillName.Fencing;
            DisplayName = "Backstab";
            CanBeUsed = true;
            Description = "When activated, the next miss from opponent causes you to go behind them and hit for extra damage.";
            ImageID = 118;
            GumpHeight = 75;
            AddEndY = 100;
        }

        public override void OnUse(Mobile mobile)
        {
            BaseWeapon weapon = mobile.Weapon as BaseWeapon;
            if (weapon.Skill == RequiredWeaponSkill && weapon is not BaseSpear)
            {
                base.OnUse(mobile);
            }
            else
            {
                mobile.SendMessage("You do not have a one handed fencing weapon equipped.");
            }
        }
        public override void CheckDefenderMissEffect(Mobile attacker, Mobile defender)
        {
            Point3D attackerPosition = attacker.Location;
            if (Activated && attacker.Direction != Direction.Running && defender.Weapon is BaseMeleeWeapon)
            {
                Activated = false;
                OnCooldown = true;
                Direction attackerDirection = defender.GetDirectionTo(attackerPosition.X, attackerPosition.Y, false);
                Direction newDefendingDirection;
                newDefendingDirection = attackerDirection switch
                {
                    Direction.West => Direction.East,
                    Direction.East => Direction.West,
                    Direction.North => Direction.South,
                    _ => Direction.North
                };
                Point3D newDefendingLocation;
                newDefendingLocation = newDefendingDirection switch
                {
                    Direction.East => new Point3D(attackerPosition.X - 2, attackerPosition.Y, attackerPosition.Y),
                    Direction.West => new Point3D(attackerPosition.X + 2, attackerPosition.Y, attackerPosition.Y),
                    Direction.South => new Point3D(attackerPosition.X, attackerPosition.Y - 2 , attackerPosition.Y),
                    _ => new Point3D(attackerPosition.X, attackerPosition.Y + 2, attackerPosition.Y)
                };

                if (defender.InLOS(newDefendingLocation))
                {
                    defender.MoveToWorld(newDefendingLocation, defender.Map);
                    defender.Direction = newDefendingDirection;
                    defender.PlaySound(0x525);
                    BaseMeleeWeapon weapon = (BaseMeleeWeapon)defender.Weapon;
                    attacker.Damage(weapon.MaxDamage + Level, defender);
                } else
                {
                    defender.SendMessage("Your back stab execution failed due to nearby objects");
                }
                Timer.StartTimer(TimeSpan.FromSeconds(45), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}
