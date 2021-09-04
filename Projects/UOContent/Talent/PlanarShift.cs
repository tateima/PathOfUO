using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class PlanarShift : BaseTalent, ITalent
    {
        public TimerExecutionToken _ActivatedTimerToken;

        public PlanarShift() : base()
        {
            TalentDependency = typeof(MageCombatant);
            DisplayName = "Planar shift";
            CanBeUsed = true;
            Description = "Reduces damage by 10% per level for 10 seconds. 3 minute cooldown.";
            ImageID = 30029;
        }

        public virtual void OnUse(Mobile mobile)
        {
            if (!Activated && !OnCooldown)
            {
                Activated = true;
                Effects.SendLocationParticles(
                       EffectItem.Create(mobile.Location, mobile.Map, EffectItem.DefaultDuration),
                       0x3728,
                       10,
                       10,
                       5023
                   );
                mobile.PlaySound(0x0F7);
            }
            Timer.StartTimer(TimeSpan.FromSeconds(10), ExpireActivated, out _ActivatedTimerToken);
            Timer.StartTimer(TimeSpan.FromMinutes(3), ExpireTalentCooldown, out _talentTimerToken);
        }

        public override int ModifySpellMultiplier()
        {
            return Level * 10;
        }

        public void ExpireActivated()
        {
            Activated = false;
        }
        public virtual void ExpireTalentCooldown()
        {
            OnCooldown = false;
        }
    }
}
