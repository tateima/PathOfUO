using System;
using Server.Items;
using Server.Spells;

namespace Server.Talent
{
    public class SpellWard : BaseTalent
    {
        public SpellWard()
        {
            RequiredWeapon = new[] { typeof(BaseShield) };
            CanBeUsed = true;
            TalentDependency = typeof(ShieldFocus);
            DisplayName = "Spell ward";
            Description = "Reflects damage from 4-8 spells while activated with shield. 2min second cooldown.";
            ImageID = 372;
            GumpHeight = 75;
            AddEndY = 75;
        }

        public int RemainingReflections { get; set; }

        public void ProcessDamage(
            Spell spell, TimeSpan delay, Mobile target, Mobile from, ref double damage, ref int phys, ref int fire,
            ref int cold, ref int pois, ref int nrgy, ref int chaos
        )
        {
            if (target.FindItemOnLayer(Layer.TwoHanded) is BaseShield && Activated)
            {
                if (RemainingReflections > 0)
                {
                    RemainingReflections--;
                    target.PlaySound(0x1E9);
                    target.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                    if (Core.AOS)
                    {
                        from.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
                        from.PlaySound(0x0FC);
                    }
                    else
                    {
                        from.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
                        from.PlaySound(0x1F1);
                    }

                    SpellHelper.Damage(spell, from, damage, phys, fire, cold, pois, nrgy, chaos);
                    damage = 0;
                    phys = 0;
                    fire = 0;
                    cold = 0;
                    pois = 0;
                    nrgy = 0;
                    chaos = 0;
                    if (RemainingReflections == 0)
                    {
                        Activated = false;
                        OnCooldown = true;
                        Timer.StartTimer(TimeSpan.FromSeconds(120), ExpireTalentCooldown, out _talentTimerToken);
                    }
                }
            }
        }

        public override void OnUse(Mobile from)
        {
            if (from.FindItemOnLayer(Layer.TwoHanded) is BaseShield)
            {
                if (from.Mana < 20)
                {
                    from.SendMessage("You require 20 mana to conjure a storm.");
                }
                else if (!Activated && !OnCooldown)
                {
                    Activated = true;
                    from.Mana -= 20;
                    RemainingReflections = Level + Utility.Random(1, 3);
                }
            }
            else
            {
                from.SendMessage("You require a shield equipped to use this talent.");
            }
        }
    }
}
