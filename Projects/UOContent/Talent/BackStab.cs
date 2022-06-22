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
            StamRequired = 10;
            CanBeUsed = true;
            Description =
                "When activated, the next miss from opponent causes you to go behind them and hit for extra damage.";
            CooldownSeconds = 30;
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
            if (Activated && attacker.Direction != Direction.Running && target.Weapon is BaseMeleeWeapon backstabWeapon && target.Stam >= StamRequired + 1)
            {
                Activated = false;
                OnCooldown = true;
                var attackerDirection = target.GetDirectionTo(attackerPosition.X, attackerPosition.Y);
                var newDefendingLocation = CalculatePushbackFromAnchor(attackerPosition, -2, target);
                var newDefendingDirection = attackerDirection switch
                {
                    Direction.West  => Direction.East,
                    Direction.East  => Direction.West,
                    Direction.North => Direction.South,
                    _               => Direction.North
                };
                if (target.InLOS(newDefendingLocation))
                {
                    ApplyStaminaCost(target);
                    target.MoveToWorld(newDefendingLocation, target.Map);
                    target.Direction = newDefendingDirection;
                    target.PlaySound(0x525);
                    if (backstabWeapon.CheckHit(target, attacker))
                    {
                        attacker.Damage(backstabWeapon.MaxDamage + Level, target);
                    }
                }
                else
                {
                    target.SendMessage("Your back stab execution failed due to nearby objects");
                }

                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}
