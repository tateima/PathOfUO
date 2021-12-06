using System;
using Server.Spells.Mysticism;
using Server.Spells.Necromancy;
using Server.Spells.Spellweaving;

namespace Server.Talent
{
    public class DarkAffinity : BaseTalent
    {
        private Mobile m_Mobile;
        private ResistanceMod m_ResistanceMod;

        public DarkAffinity()
        {
            CanBeUsed = true;
            BlockedBy = new[] { typeof(LightAffinity) };
            RequiredSpell = new[] { typeof(NecromancerSpell), typeof(SpellPlagueSpell), typeof(WordOfDeathSpell) };
            DisplayName = "Dark affinity";
            Description = "Enhances damage and strength of dark arts and spells. Requires 50+ Necromancy.";
            ImageID = 129;
            GumpHeight = 95;
            AddEndY = 80;
        }


        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills[SkillName.Necromancy].Base >= 50;

        public override double ModifySpellScalar() => Level / 100.0;

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                m_ResistanceMod = new ResistanceMod(ResistanceType.Cold, Level * 5);
                m_Mobile = from;
                OnCooldown = true;
                m_Mobile.AddResistanceMod(m_ResistanceMod);
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

                Timer.StartTimer(TimeSpan.FromSeconds(60 + Utility.Random(20)), ExpireBuff, out _);
                Timer.StartTimer(TimeSpan.FromSeconds(180 - Level * 5), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public void ExpireBuff()
        {
            m_Mobile?.RemoveResistanceMod(m_ResistanceMod);
        }

        public override bool IgnoreTalentBlock(Mobile mobile) => mobile.Skills.Spellweaving.Value > 0.0;
    }
}
