using System;
using Server.Items;
using Server.Mobiles;
using Server.Spells.Bushido;

namespace Server.Talent
{
    public class DragonAspect : BaseTalent
    {
        public DragonAspect()
        {
            RequiredWeapon = new[] { typeof(BaseWeapon) };
            BlockedBy = new[] { typeof(ViperAspect) };
            TalentDependency = typeof(FireAffinity);
            DisplayName = "Dragon aspect";
            Description = "Has a chance on spell cast or hit to conjure a fire breath.";
            AdditionalDetail = $"Each level increases the chance by 2%. Damage is scaled by your hit points and your fire affinity level. This talent also increases your fire resistance by 5 points per level. {PassiveDetail}";
            ImageID = 198;
            GumpHeight = 75;
            AddEndY = 105;
            AddEndAdditionalDetailsY = 110;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            CheckDragonEffect(attacker, target);
        }

        public override void UpdateMobile(Mobile m)
        {
            if (Core.AOS)
            {
                if (ResMod != null)
                {
                    m.RemoveResistanceMod(ResMod);
                }

                ResMod = new ResistanceMod(ResistanceType.Fire, Level * 5);
                m.AddResistanceMod(ResMod);
            }
        }

        public void CheckDragonEffect(Mobile attacker, Mobile target)
        {
            if (Utility.Random(100) < Level * 200)
            {
                Timer.StartTimer(TimeSpan.FromSeconds(1.3), () => BreathEffectCallback(attacker, target));
            }
        }

        public override void CheckSpellEffect(Mobile attacker, Mobile target)
        {
            CheckDragonEffect(attacker, target);
        }

        public virtual void BreathEffectCallback(Mobile attacker, Mobile target)
        {
            if (!target.Alive)
            {
                return;
            }
            int fire = 100;
            int cold = 0;
            int hue = 0;
            ApplyFrostFireEffect((PlayerMobile)attacker, ref fire, ref cold, ref hue, target);
            attacker.PlaySound(0x227);
            attacker.MovingParticles(target, 0x36D4,7, 0, false, true, hue, 0, 9502, 4019, 0x160, 0);
            Timer.StartTimer(TimeSpan.FromSeconds(1.0), () => BreathDamageCallback(attacker, target, fire, cold));
        }

        public virtual void BreathDamageCallback(Mobile attacker, Mobile target, int fire, int cold)
        {
            if (target is BaseCreature { BreathImmune: true })
            {
                return;
            }

            if (attacker.CanBeHarmful(target))
            {
                attacker.DoHarmful(target);
                BreathDealDamage(attacker, target, fire, cold);
            }
        }

        public virtual void BreathDealDamage(Mobile attacker, Mobile target, int fire, int cold)
        {
            if (!Evasion.CheckSpellEvasion(target))
            {
                var damage = attacker.Hits * SpecialDamageScalar;
                if (damage < 1)
                {
                    damage = 1;
                }
                AOS.Damage(
                    target,
                    attacker,
                    (int)damage,
                    0,
                    fire,
                    cold,
                    0,
                    0
                );
            }
        }
    }
}
