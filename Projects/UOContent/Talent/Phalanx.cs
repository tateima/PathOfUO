using System;
using Server.Items;

namespace Server.Talent
{
    public class Phalanx : BaseTalent
    {
        public Phalanx()
        {
            RequiredWeapon = new[] { typeof(BaseShield) };
            CanBeUsed = true;
            TalentDependency = typeof(ShieldFocus);
            DisplayName = "Phalanx";
            CooldownSeconds = 120;
            Description = "Blocks 2-8 projectiles from hitting target.";
            ImageID = 375;
            GumpHeight = 75;
            AddEndY = 85;
        }

        public int RemainingBlocks { get; set; }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Parry.Value >= 70.0;

        public bool CheckBlock(Mobile defender, BaseWeapon attackingWeapon)
        {
            if (Activated)
            {
                if (defender.FindItemOnLayer(Layer.TwoHanded) is BaseShield && attackingWeapon is BaseRanged && RemainingBlocks > 0)
                {
                    RemainingBlocks--;
                    if (RemainingBlocks == 0)
                    {
                        Activated = false;
                        OnCooldown = true;
                        Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                    }
                    defender.SendSound(0x520);
                    return true;
                }
            }
            return false;
        }

        public override void OnUse(Mobile from)
        {
            if (from.FindItemOnLayer(Layer.TwoHanded) is BaseShield && HasSkillRequirement(from))
            {
                if (!Activated && !OnCooldown)
                {
                    Activated = true;
                    RemainingBlocks = Level + Utility.Random(1, 3);
                    from.SendSound(0x140);
                }
            }
            else
            {
                from.SendMessage("You cannot use this talent right now.");
            }
        }
    }
}
