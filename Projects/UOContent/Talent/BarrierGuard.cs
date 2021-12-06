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
            Description = "Parry the next 1-5 attacks, depending on level. 45s min cooldown.";
            ImageID = 367;
            GumpHeight = 75;
            AddEndY = 85;
        }

        public int RemainingParry { get; set; }

        public bool CheckParry()
        {
            var canParry = RemainingParry > 0;
            if (canParry)
            {
                RemainingParry--;
            }
            else
            {
                OnCooldown = true;
                Timer.StartTimer(TimeSpan.FromSeconds(45), ExpireTalentCooldown, out _talentTimerToken);
            }

            return canParry;
        }

        public override void OnUse(Mobile from)
        {
            var weapon = from.Weapon as BaseWeapon;
            if (weapon is not BaseSword)
            {
                if (!Activated && !OnCooldown)
                {
                    Activated = true;
                    RemainingParry = Level;
                }
            }
            else
            {
                from.SendMessage("You do not have a one handed sword equipped.");
            }
        }
    }
}
