using Server.Mobiles;
using System;
using Server.Items;

namespace Server.Talent
{
    public class BlindingShot : BaseTalent, ITalent
    {
        public BlindingShot() : base()
        {
            TalentDependency = typeof(ArcherFocus);
            RequiredWeapon = new Type[] { typeof(BaseRanged) };
            RequiredWeaponSkill = SkillName.Archery;
            DisplayName = "Blinding shot";
            CanBeUsed = true;
            Description = "Next hit blinds target for 3s per level. Level also reduces cooldown by 10s. 2m cooldown";
            ImageID = 383;
            GumpHeight = 75;
            AddEndY = 100;
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (Activated)
            {
                Activated = false;
                OnCooldown = true;
                int cooldownSeconds = 120-(Level*10);
                int duration = Level * 3;
                if (target is PlayerMobile targetPlayer) {
                    targetPlayer.Blind(duration);
                } else if (target is BaseCreature targetCreature) {
                    targetCreature.Blind(duration);
                } else {
                    cooldownSeconds = 0;
                }
                if (cooldownSeconds > 0) {
                    Timer.StartTimer(TimeSpan.FromSeconds(cooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }
    }
}
