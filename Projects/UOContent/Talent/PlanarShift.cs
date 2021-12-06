using System;
using Server.Items;

namespace Server.Talent
{
    public class PlanarShift : BaseTalent
    {
        public PlanarShift()
        {
            TalentDependency = typeof(MageCombatant);
            DisplayName = "Planar shift";
            CanBeUsed = true;
            Description = "Reduces damage by 15% per level for 15 seconds. 2 minute cooldown.";
            ImageID = 161;
            AddEndY = 95;
        }

        public override int ModifySpellMultiplier() => Level * 15;

        public override void OnUse(Mobile from)
        {
            if (!Activated && !OnCooldown)
            {
                Activated = true;
                OnCooldown = true;
                Effects.SendLocationParticles(
                    EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration),
                    0x3728,
                    10,
                    10,
                    5023
                );
                from.PlaySound(0x0F7);
                Timer.StartTimer(TimeSpan.FromSeconds(15), ExpireActivated, out _);
                Timer.StartTimer(TimeSpan.FromMinutes(2), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public void ExpireActivated()
        {
            Activated = false;
        }
    }
}
