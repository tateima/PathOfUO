using Server.Mobiles;
using Server.Spells.First;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Spellweaving;
using System;

namespace Server.Talent
{
    public class FireAffinity : BaseTalent, ITalent
    {
        private Mobile m_Mobile;
        public FireAffinity() : base()
        {
            ResMod = new ResistanceMod(ResistanceType.Fire, Level * 5);
            RequiredSpell = new Type[] { typeof(FireballSpell), typeof(FireFieldSpell), typeof(FlameStrikeSpell), typeof(MeteorSwarmSpell), typeof(MagicArrowSpell), typeof(ExplosionSpell), typeof(ImmolatingWeaponSpell) };
            DisplayName = "Fire affinity";
            CanBeUsed = true;
            Description = "Increases damage done by fire abilities/spells.";
            ImageID = 30212;
        }
        public override int ModifySpellMultiplier()
        {
            // 2% per level
            return Level*2;
        }
        public override double ModifySpellScalar()
        {
            return (double)(Level / 100)*2;
        }

        public override void OnUse(Mobile mobile)
        {            
            if (!OnCooldown)
            {
                m_Mobile = mobile;
                OnCooldown = true;
                if (Core.AOS)
                {
                    m_Mobile.AddResistanceMod(ResMod);
                    m_Mobile.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    m_Mobile.PlaySound(0x208);
                }
                Timer.StartTimer(TimeSpan.FromSeconds(60), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
        public override void ExpireTalentCooldown()
        {
            base.ExpireTalentCooldown();
            if (m_Mobile != null)
            {
                if (Core.AOS)
                {
                    m_Mobile.RemoveResistanceMod(ResMod);
                }
            }
        }
    }
}
