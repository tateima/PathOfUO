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
        public DarkAffinity() : base()
        {
            m_ResistanceMod = new ResistanceMod(ResistanceType.Poison, +Level * 3);
            CanBeUsed = true;
            BlockedBy = new Type[] { typeof(LightAffinity) };
            RequiredSpell = new Type[] { typeof(NecromancerSpell), typeof(SpellPlagueSpell), typeof(WordOfDeathSpell) };
            DisplayName = "Dark affinity";
            Description = "Enhances damage and strength of dark arts and spells.";
            ImageID = 30042;
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
                Timer.StartTimer(TimeSpan.FromSeconds(60), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
        public override void ExpireTalentCooldown()
        {
            base.ExpireTalentCooldown();
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
