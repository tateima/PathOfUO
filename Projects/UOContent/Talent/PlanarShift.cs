using System;
using Server.Items;

namespace Server.Talent
{
    public class PlanarShift : BaseTalent
    {
        public PlanarShift()
        {
            TalentDependencies = new[] { typeof(MageCombatant) };
            DisplayName = "Planar shift";
            CanBeUsed = true;
            CooldownSeconds = 120;
            ManaRequired = 20;
            Description = "Reduces damage by 15% per level for 15 seconds.";
            ImageID = 161;
            AddEndY = 95;
        }

        public override int ModifySpellMultiplier() => Level * 15;

        public override void OnUse(Mobile from)
        {
            if (Activated)
            {
                from.SendMessage($"{DisplayName} is already in use");
            }
            else
            {
                if (from.Mana < ManaRequired)
                {
                    from.SendMessage($"You require {ManaRequired.ToString()} mana to shift planes.");
                } else if (!Activated && !OnCooldown && HasSkillRequirement(from))
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
                    ApplyManaCost(from);
                    Timer.StartTimer(TimeSpan.FromSeconds(15), ExpireActivated, out _);
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                }
            }

        }

        public void ExpireActivated()
        {
            Activated = false;
        }
    }
}
