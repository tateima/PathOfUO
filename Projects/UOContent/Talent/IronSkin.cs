using System;

namespace Server.Talent
{
    public class IronSkin : BaseTalent
    {
        private Mobile m_Mobile;

        public IronSkin()
        {
            TalentDependency = typeof(GiantsHeritage);
            DisplayName = "Iron skin";
            CanBeUsed = true;
            Description = "Increases physical resistance on use.";
            ImageID = 119;
            GumpHeight = 70;
            AddEndY = 65;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                ResMod = new ResistanceMod(ResistanceType.Physical, Level * 5);
                m_Mobile = from;
                OnCooldown = true;
                if (Core.AOS)
                {
                    m_Mobile.AddResistanceMod(ResMod);
                    m_Mobile.FixedParticles(0x373A, 10, 15, 5021, EffectLayer.Waist);
                    m_Mobile.PlaySound(0x63B);
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
