using System;
using Server.Items;

namespace Server.Talent
{
    public class BarrierGuard : BaseTalent
    {
        public BarrierGuard()
        {
            RequiredWeaponSkill = SkillName.Swords;
            RequiredWeapon = new[] { typeof(BaseSword) };
            TalentDependency = typeof(SwordSpecialist);
            DisplayName = "Barrier guard";
            CanBeUsed = true;
            CooldownSeconds = 45;
            Description = "Parry the next 1-5 attacks, depending on level.";
            ImageID = 367;
            GumpHeight = 75;
            AddEndY = 85;
        }

        public int RemainingParry { get; set; }

        public bool CheckParry(Mobile defender)
        {
            var canParry = false;
            if (Activated && defender.FindItemOnLayer(Layer.OneHanded) is BaseSword)
            {
                canParry = RemainingParry > 0;
                if (canParry)
                {
                    RemainingParry--;
                }
                else
                {
                    Activated = false;
                    OnCooldown = true;
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
            return canParry;
        }

        public override void OnUse(Mobile from)
        {
            var weapon = from.Weapon as BaseWeapon;
            if (weapon?.Skill == RequiredWeaponSkill && weapon is BaseSword)
            {
                base.OnUse(from);
                RemainingParry = Level;
            }
            else
            {
                from.SendMessage("You do not have a one handed sword equipped.");
            }
        }
    }
}
