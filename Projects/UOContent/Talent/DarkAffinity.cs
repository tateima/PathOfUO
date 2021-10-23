using System;
using Server.Spells.Necromancy;
using Server.Spells.Mysticism;
using Server.Spells.Spellweaving;
using Server.Mobiles;

namespace Server.Talent
{
    public class DarkAffinity : BaseTalent, ITalent
    {
        private Mobile m_Mobile;
        private ResistanceMod m_ResistanceMod;
        private TimerExecutionToken _buffTimerToken;
        public DarkAffinity() : base()
        {
            CanBeUsed = true;
            BlockedBy = new Type[] { typeof(LightAffinity) };
            RequiredSpell = new Type[] { typeof(NecromancerSpell), typeof(SpellPlagueSpell), typeof(WordOfDeathSpell) };
            DisplayName = "Dark affinity";
            Description = "Enhances damage and strength of dark arts and spells.";
            ImageID = 129;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override double ModifySpellScalar()
        {
            return (Level / 100);
        }
        public override void OnUse(Mobile mobile)
        {
            if (!OnCooldown)
            {
                m_ResistanceMod = new ResistanceMod(ResistanceType.Cold, Level * 5);
                m_Mobile = mobile;
                OnCooldown = true;
                m_Mobile.AddResistanceMod(m_ResistanceMod);
                if (Core.AOS)
                {
                    mobile.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
                    mobile.PlaySound(0x0FC);
                }
                else
                {
                    mobile.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
                    mobile.PlaySound(0x1F1);
                }
                Timer.StartTimer(TimeSpan.FromSeconds(60 + Utility.Random(20)), ExpireBuff, out _buffTimerToken);
                Timer.StartTimer(TimeSpan.FromSeconds(180 - Level * 5), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
        public void ExpireBuff()
        {
            if (m_Mobile != null)
            {
                m_Mobile.RemoveResistanceMod(m_ResistanceMod);
            }
        }
        public override bool IgnoreTalentBlock(Mobile mobile)
        {
            return mobile.Skills.Spellweaving.Value > 0.0;
        }
    }
}
