using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class PlanarShift : BaseTalent, ITalent
    {
        public TimerExecutionToken _activatedTimerToken;

        public PlanarShift() : base()
        {
            TalentDependency = typeof(MageCombatant);
            DisplayName = "Planar shift";
            CanBeUsed = true;
            Description = "Reduces damage by 15% per level for 15 seconds. 2 minute cooldown.";
            ImageID = 161;
            AddEndY = 95;
        }

        public override void OnUse(Mobile mobile)
        {
            if (!Activated && !OnCooldown)
            {
                Activated = true;
                OnCooldown = true;
                Effects.SendLocationParticles(
                       EffectItem.Create(mobile.Location, mobile.Map, EffectItem.DefaultDuration),
                       0x3728,
                       10,
                       10,
                       5023
                   );
                mobile.PlaySound(0x0F7);
                Timer.StartTimer(TimeSpan.FromSeconds(15), ExpireActivated, out _activatedTimerToken);
                Timer.StartTimer(TimeSpan.FromMinutes(2), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public override int ModifySpellMultiplier()
        {
            return Level * 15;
        }

        public void ExpireActivated()
        {
            Activated = false;
        }
    }
}
