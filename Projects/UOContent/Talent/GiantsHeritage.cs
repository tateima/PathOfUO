using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class GiantsHeritage : BaseTalent, ITalent
    {
        private Mobile m_Mobile;
        private TimerExecutionToken _buffTimerToken;
        public GiantsHeritage() : base()
        {
            TalentDependency = typeof(DivineStrength);
            DisplayName = "Giant's Heritage";
            CanBeUsed = true;
            Description = "Increases strength per level while active. The more stamina you have the more damage you do.";
            ImageID = 144;
        }
        public override void OnUse(Mobile mobile)
        {
            if (!OnCooldown)
            {
                if (mobile.Stam < 1)
                {
                    mobile.SendMessage("You cannot use giant's heritage at this time.");
                }
                else 
                {
                    m_Mobile = mobile;
                    Activated = true;
                    OnCooldown = true;
                    m_Mobile.RemoveStatMod("GiantsHeritage");
                    m_Mobile.AddStatMod(new StatMod(StatType.Str, "GiantsHeritage", Level * 2, TimeSpan.Zero));
                    m_Mobile.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                    m_Mobile.PlaySound(0x1AB);
                    Timer.StartTimer(TimeSpan.FromSeconds(60 + Utility.Random(20)), ExpireBuff, out _buffTimerToken);
                    Timer.StartTimer(TimeSpan.FromSeconds(180 - Level * 5), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }

        public void ExpireBuff()
        {
            if (m_Mobile != null)
            {
                m_Mobile.RemoveStatMod("GiantsHeritage");
            }
            Activated = false;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (Activated)
            {
                int extraDamage = (int)(attacker.Stam * SpecialDamageScalar);
                target.Damage(extraDamage, attacker);
                attacker.Stam -= 10 + Utility.Random(10);
                if (attacker.Stam < 10)
                {
                    m_Mobile = attacker;
                    ExpireBuff();
                }
            }  
        }
    }
}
