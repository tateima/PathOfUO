using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class Cleave : BaseTalent
    {
        public Cleave()
        {
            TalentDependency = typeof(SwordsmanshipFocus);
            RequiredWeapon = new[] { typeof(BasePoleArm) };
            RequiredWeaponSkill = SkillName.Swords;
            DisplayName = "Cleave";
            CanBeUsed = true;
            Description =
                "Attack one other nearby enemy for 50% + level * 2 damage. If no enemies, apply 25% damage to same target (30s cooldown).";
            ImageID = 368;
            GumpHeight = 75;
            AddEndY = 105;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (Activated)
            {
                Activated = false;
                OnCooldown = true;
                var mobiles = attacker.GetMobilesInRange(3);
                var hitAnotherMobile = false;
                foreach (var mobile in mobiles)
                {
                    if (mobile == attacker || mobile is PlayerMobile && mobile.Karma > 0 ||
                        !mobile.CanBeHarmful(attacker, false) ||
                        Core.AOS && !mobile.InLOS(attacker))
                    {
                        continue;
                    }

                    hitAnotherMobile = true;
                    mobile.Damage(AOS.Scale(damage, 50 + Level * 2), attacker);
                    break;
                }

                if (!hitAnotherMobile)
                {
                    target.Damage(AOS.Scale(damage, 25), attacker);
                }

                Timer.StartTimer(TimeSpan.FromSeconds(30), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public override void OnUse(Mobile from)
        {
            var weapon = from.Weapon as BaseWeapon;
            if (weapon is BasePoleArm)
            {
                base.OnUse(from);
            }
            else
            {
                from.SendMessage("You do not have a pole arm equipped.");
            }
        }
    }
}
