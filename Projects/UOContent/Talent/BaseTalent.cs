using System;

namespace Server.Talent
{
    public class BaseTalent : ITalent
    {
        private int m_MaxLevel;
        public int MaxLevel
        {
            get { return m_MaxLevel; }
            set { m_MaxLevel = value; }
        }

        private int m_Level;
        private ResistanceMod m_ResistanceMod;

        public ResistanceMod ReistanceMod
        {
            get { return m_ResistanceMod; }
            set { m_ResistanceMod = value; }
        }
        public static readonly Type[] TalentTypes =
{
            typeof(ViperAspect)
        };

        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        public BaseTalent()
        {
            m_Level = 0;
            m_MaxLevel = 5;
        }

        public void CheckEffect(Mobile m, Mobile t)
        {  
        }

        public void UpdateMobile(Mobile m)
        {
        }
    }
}
