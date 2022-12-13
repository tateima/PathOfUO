using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class BlindingShot : BaseTalent
    {
        public BlindingShot()
        {
            TalentDependencies = new[] { typeof(ArcherFocus) };
            RequiredWeapon = new[] { typeof(BaseRanged) };
            RequiredWeaponSkill = SkillName.Archery;
            DisplayName = "Blinding shot";
            ManaRequired = 15;
            CooldownSeconds = 120;
            CanBeUsed = true;
            Description = "Next hit blinds target for 3s per level. Level also reduces cooldown by 10s. 2m cooldown";
            ImageID = 383;
            GumpHeight = 75;
            AddEndY = 95;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (Activated && attacker.Mana >= ManaRequired)
            {
                ApplyManaCost(attacker);
                Activated = false;
                OnCooldown = true;
                var cooldownSeconds = CooldownSeconds - Level * 10;
                var duration = Level * 3;
                if (target is PlayerMobile targetPlayer)
                {
                    targetPlayer.Blind(duration);
                }
                else if (target is BaseCreature targetCreature)
                {
                    targetCreature.Blind(duration);
                }
                else
                {
                    cooldownSeconds = 0;
                }

                if (cooldownSeconds > 0)
                {
                    Timer.StartTimer(TimeSpan.FromSeconds(cooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }
    }
}
