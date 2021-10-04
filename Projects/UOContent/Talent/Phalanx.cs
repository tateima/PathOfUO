using Server.Items;
using Server.Spells;
using Server.Misc;
using System;
namespace Server.Talent
{
    public class Phalanx : BaseTalent, ITalent
    {
        private int m_RemainingBlocks;
        public int RemainingBlocks
        {
            get
            {
                return m_RemainingBlocks;
            }
            set
            {
                m_RemainingBlocks = value;
            }
        }
        public Phalanx() : base()
        {
            RequiredWeapon = new Type[] { typeof(BaseShield) };
            CanBeUsed = true;
            TalentDependency = typeof(ShieldFocus);
            DisplayName = "Phalanx";
            Description = "Blocks 2-8 projectiles from hitting target. 2min second cooldown.";
            ImageID = 375;
            GumpHeight = 75;
            AddEndY = 85;
        }

        public bool CheckBlock(BaseWeapon weapon)
        {
            if (((Mobile)weapon.Parent).FindItemOnLayer(Layer.TwoHanded) is BaseShield shield && weapon is BaseRanged && RemainingBlocks > 0 && Activated)
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

        public override void OnUse(Mobile mobile)
        {
            if (mobile.FindItemOnLayer(Layer.TwoHanded) is BaseShield shield)
            {
                if (!Activated && !OnCooldown)
                {
                    Activated = true;
                    RemainingBlocks = Level + Utility.Random(1, 3);
                }
            } else
            {
                mobile.SendMessage("You require a shield equipped to use this talent.");
            }
        }
    }
}
