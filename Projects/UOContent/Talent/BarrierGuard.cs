using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class BarrierGuard : BaseTalent, ITalent
    {
        private int m_RemainingParry;
        public int RemainingParry
        {
            get
            {
                return m_RemainingParry;
            }
            set
            {
                m_RemainingParry = value;
            }
        }
        public BarrierGuard() : base()
        {
            RequiredWeaponSkill = SkillName.Swords;
            RequiredWeapon = new Type[] { typeof(BaseSword) };
            TalentDependency = typeof(SwordSpecialist);
            DisplayName = "Barrier guard";
            CanBeUsed = true;
            Description = "Parry the next 1-5 attacks, depending on level. 45s min cooldown.";
            ImageID = 367;
            GumpHeight = 75;
            AddEndY = 85;
        }
        public bool CheckParry()
        {
            bool canParry = (RemainingParry > 0);
            if (canParry)
            {
                RemainingParry--;
            } else
            {
                OnCooldown = true;
                Timer.StartTimer(TimeSpan.FromSeconds(45), ExpireTalentCooldown, out _talentTimerToken);
            }
            return canParry;
        }
        public override void OnUse(Mobile mobile)
        {
            BaseWeapon weapon = mobile.Weapon as BaseWeapon;
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
                mobile.SendMessage("You do not have a one handed sword equipped.");
            }

        }
    }
}
