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
            Description = "Blocks 2-8 projectiles from hitting target. 2min second cooldown.";
            ImageID = 375;
            GumpHeight = 75;
            AddEndY = 85;
        }

        public int RemainingBlocks { get; set; }

        public bool CheckBlock(BaseWeapon weapon)
        {
            if (((Mobile)weapon.Parent).FindItemOnLayer(Layer.TwoHanded) is BaseShield && weapon is BaseRanged &&
                RemainingBlocks > 0 && Activated)
            {
                RemainingBlocks--;
                if (RemainingBlocks == 0)
                {
                    Activated = false;
                    OnCooldown = true;
                    Timer.StartTimer(TimeSpan.FromSeconds(120), ExpireTalentCooldown, out _talentTimerToken);
                }

                return true;
            }

            return false;
        }

        public override void OnUse(Mobile from)
        {
            if (from.FindItemOnLayer(Layer.TwoHanded) is BaseShield)
            {
                if (!Activated && !OnCooldown)
                {
                    Activated = true;
                    RemainingBlocks = Level + Utility.Random(1, 3);
                }
            }
            else
            {
                from.SendMessage("You require a shield equipped to use this talent.");
            }
        }
    }
}
