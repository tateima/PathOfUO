using Server.Mobiles;
using Server.Targeting;
using System;

namespace Server.Talent
{
    public class Gambler : BaseTalent, ITalent
    {
        public Gambler() : base()
        {
            TalentDependency = typeof(SmoothTalker);
            DisplayName = "Gambler";
            Description = "Gamble gold with target npc, can result in gold loss. 5m cooldown.";
            ImageID = 362;
            GumpHeight = 85;
            AddEndY = 75;
            MaxLevel = 10;
        }

        public override void OnUse(Mobile mobile)
        {
            if (!OnCooldown)
            {
                OnCooldown = true;
                mobile.SendMessage("Whom do you wish to gamble with?");
                mobile.Target = new InternalTarget(mobile, this);
                Timer.StartTimer(TimeSpan.FromMinutes(5), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public override double ModifySpellScalar()
        {
            return (Level / 100) * 2; // 2% per point
        }

        private class InternalTarget : Target
        {
            private Mobile m_Gambler;
            private Gambler m_GamblerTalent;
            public InternalTarget(Mobile gambler, Gambler gamblerTalent) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                m_Gambler = gambler;
                m_GamblerTalent = gamblerTalent;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                Mobile target = null;
                if (targeted is BaseVendor || targeted is BaseEscortable || targeted is Gypsy || targeted is Actor || targeted is Artist || targeted is Sculptor || targeted is Samurai || targeted is Ninja)
                {
                    target = (Mobile)targeted;
                    double goldAmount = Utility.Random(m_GamblerTalent.Level * 50);
                    double chanceOfLosing = 30.0;
                    double stakes = Utility.RandomDouble();
                    if (targeted is Gypsy)
                    {
                        chanceOfLosing += (double)(Utility.Random(1, 30));
                        stakes += Utility.RandomDouble();
                    }
                    goldAmount *= stakes;

                    if (m_GamblerTalent.CanAffordLoss((PlayerMobile)m_Gambler, (int)goldAmount)) {                      
                        if (Utility.Random(100) < (int)(chanceOfLosing))
                        {
                            m_GamblerTalent.ProcessGoldGain((PlayerMobile)m_Gambler, (int)goldAmount, (Utility.Random(100) < (int)(chanceOfLosing)));
                        }
                    } else
                    {
                        target.Say("Thou cannot afford to bet with me!");
                    }
                }
                else
                {
                    m_Gambler.SendMessage("You cannot gamble with that target.");
                }
            }
        }
    }
}
