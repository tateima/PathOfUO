using Server.Mobiles;
using Server.Spells.Bushido;
using System;

namespace Server.Talent
{
    public class DragonAspect : BaseTalent, ITalent
    {
        public DragonAspect() : base()
        {
            BlockedBy = new Type[] { typeof(ViperAspect) };
            TalentDependency = typeof(FireAffinity);
            DisplayName = "Dragon aspect";
            Description = "Has a chance on spell cast or hit to conjure a fire breath.";
            ImageID = 198;
            GumpHeight = 75;
            AddEndY = 105;
        }
        public void CheckDragonEffect(Mobile attacker, Mobile target)
        {
            if (Utility.Random(100) < Level)
            {
                Timer.StartTimer(TimeSpan.FromSeconds(1.3), () => BreathEffect_Callback(attacker, target));
            }
        }

        public override void CheckSpellEffect(Mobile attacker, Mobile target)
        {
            CheckDragonEffect(attacker, target);
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            CheckDragonEffect(attacker, target);
        }
        public virtual void BreathEffect_Callback(Mobile attacker, Mobile target)
        {
            if (!target.Alive)
            {
                return;
            }

            attacker.PlaySound(0x227);

            Effects.SendMovingEffect(
                 attacker,
                 target,
                 0x36D4,
                 5,
                 0,
                 false,
                 false,
                 0,
                 0
             );

            Timer.StartTimer(TimeSpan.FromSeconds(1.0), () => BreathDamage_Callback(attacker, target));
        }

        public virtual void BreathDamage_Callback(Mobile attacker, Mobile target)
        {
            if (target is BaseCreature creature && creature.BreathImmune)
            {
                return;
            }

            if (attacker.CanBeHarmful(target))
            {
                attacker.DoHarmful(target);
                BreathDealDamage(attacker, target);
            }
        }
        public virtual void BreathDealDamage(Mobile attacker, Mobile target)
        {
            if (!Evasion.CheckSpellEvasion(target))
            {
                int damage = (int)(attacker.Hits * SpecialDamageScalar);
                if (attacker is PlayerMobile player)
                {
                    BaseTalent fireAffinity = player.GetTalent(typeof(FireAffinity));
                    if (fireAffinity != null)
                    {
                        damage = AOS.Scale(damage, 100 + fireAffinity.ModifySpellMultiplier());
                    }
                }
                AOS.Damage(
                        target,
                        attacker,
                        damage,
                        0,
                        100,
                        0,
                        0,
                        0
                    );
            }
            return;
        }

            public void UpdateMobile(Mobile m)
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
    }
}
