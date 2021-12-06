using System;
using Server.Spells.First;
using Server.Spells.Fourth;
using Server.Spells.Seventh;
using Server.Spells.Sixth;
using Server.Spells.Spellweaving;
using Server.Spells.Third;

namespace Server.Talent
{
    public class FireAffinity : BaseTalent
    {
        private Mobile m_Mobile;

        public FireAffinity()
        {
            RequiredSpell = new[]
            {
                typeof(FireballSpell), typeof(FireFieldSpell), typeof(FlameStrikeSpell), typeof(MeteorSwarmSpell),
                typeof(MagicArrowSpell), typeof(ExplosionSpell), typeof(ImmolatingWeaponSpell)
            };
            DisplayName = "Fire affinity";
            CanBeUsed = true;
            Description = "Increases damage done by fire abilities/spells.";
            ImageID = 137;
            GumpHeight = 70;
            AddEndY = 65;
        }

        public override int ModifySpellMultiplier() =>
            // 2% per level
            Level * 2;

        public override double ModifySpellScalar() => Level / 100.0 * 2;

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                ResMod = new ResistanceMod(ResistanceType.Fire, Level * 5);
                m_Mobile = from;
                OnCooldown = true;
                if (Core.AOS)
                {
                    m_Mobile.AddResistanceMod(ResMod);
                    m_Mobile.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    m_Mobile.PlaySound(0x208);
                }

                Timer.StartTimer(TimeSpan.FromSeconds(60 + Utility.Random(20)), ExpireBuff, out _);
                Timer.StartTimer(TimeSpan.FromSeconds(180 - Level * 5), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public void ExpireBuff()
        {
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
