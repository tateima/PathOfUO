using Server.Mobiles;
using Server.Spells;
using System;

namespace Server.Talent
{
    public class ViperAspect : BaseTalent, ITalent
    {
        private Mobile m_Mobile;
        private TimerExecutionToken _buffTimerToken;
        public ViperAspect() : base()
        {
            BlockedBy = new Type[] { typeof(DragonAspect) };
            DisplayName = "Viper aspect";
            CanBeUsed = true;
            Description = "Increased poison resistance and adds a chance to poison your target on weapon or spell hit.";
            ImageID = 195;
            GumpHeight = 75;
            AddEndY = 85;
        }

        public override void OnUse(Mobile mobile)
        {
            if (!OnCooldown)
            {
                ResMod = new ResistanceMod(ResistanceType.Poison, Level * 5);
                m_Mobile = mobile;
                OnCooldown = true;
                if (Core.AOS)
                {
                    m_Mobile.AddResistanceMod(ResMod);
                    m_Mobile.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                    m_Mobile.PlaySound(0x205);

                }
                Timer.StartTimer(TimeSpan.FromSeconds(60 + Utility.Random(20)), ExpireBuff, out _buffTimerToken);
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

        public void CheckViperEffect(Mobile attacker, Mobile target)
        {
            if (Utility.Random(100) < Level)
            {
                target.ApplyPoison(attacker, Poison.GetPoison(Level));
            }
        }

        public override void CheckSpellEffect(Mobile attacker, Mobile target)
        {
            CheckViperEffect(attacker, target);
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            CheckViperEffect(attacker, target);
        }

    }
}
