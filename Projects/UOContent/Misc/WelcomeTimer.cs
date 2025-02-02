using System;
using Server.Gumps;
using Server.Pantheon;

namespace Server.Misc
{
    /// <summary>
    ///     This timer spouts some welcome messages to a user at a set interval. It is used on character creation and login.
    /// </summary>
    public class WelcomeTimer : Timer
    {
        private static string[] m_Messages;

        private bool m_ShowedPathChoice;

        private readonly int m_Count;

        private readonly Mobile m_Mobile;
        private int m_State;

        public static void Initialize()
        {
            m_Messages = TestCenter.Enabled
                ? new[]
                {
                    $"Welcome to {ServerList.ServerName}.",
                    "You are able to customize your character's stats and skills at anytime to anything you wish.",
                    "To see the commands to do this just say 'help'.",
                    "You will find a bank check worth 1,000,000 gold in your bank!",
                    "A spellbook and a bag of reagents has been placed into your bank box.",
                    "Various tools have been placed into your bank.",
                    "Various raw materials like ingots, logs, feathers, hides, bottles, etc, have been placed into your bank.",
                    "5 unmarked recall runes, 5 Felucca moonstones and 5 Trammel moonstones have been placed into your bank box.",
                    "One of each level of treasure map has been placed in your bank box.",
                    "You will find 9000 silver pieces deposited into your bank box. Spend it as you see fit and enjoy yourself!",
                    "You will find 9000 gold pieces deposited into your bank box. Spend it as you see fit and enjoy yourself!",
                    "A bag of PowerScrolls has been placed in your bank box."
                }
                : new[]
                {
                    $"Welcome to {ServerList.ServerName}.",
                    "Please enjoy your stay."
                };
        }

        public WelcomeTimer(Mobile m) : this(m, m_Messages.Length)
        {
        }

        public WelcomeTimer(Mobile m, int count) : base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(10.0))
        {
            m_Mobile = m;
            m_Count = count;
            m_ShowedPathChoice = false;
        }

        protected override void OnTick()
        {
            if (m_State < m_Count)
            {
                if (!m_ShowedPathChoice)
                {
                    m_Mobile.SendGump(new ChoosePathGump(m_Mobile, 1, true, false, Deity.Alignment.None));
                    m_ShowedPathChoice = true;
                }
                m_Mobile.SendMessage(0x35, m_Messages[m_State++]);
            }

            Stop();
        }
    }
}
