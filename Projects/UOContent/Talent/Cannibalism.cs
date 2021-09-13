using Server.Mobiles;
using Server.Targeting;
using System.Linq;
using System;

namespace Server.Talent
{
    public class Cannibalism : BaseTalent, ITalent
    {

        public Cannibalism() : base()
        {
            TalentDependency = typeof(BondingMaster);
            DisplayName = "Cannibalise pet";
            CanBeUsed = true;
            Description = "Sacrifice another tamed animal, transferring between 10-50% of their stats.";
            ImageID = 376;
        }
        public override void OnUse(Mobile mobile)
        {
            mobile.Target = new InternalFirstTarget(mobile, Level);
        }

        private class InternalFirstTarget : Target
        {
            private int m_level;
            public InternalFirstTarget(Mobile from, int level) : base(
                3,
                false,
                TargetFlags.None
            )
            {
                m_level = level;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (targeted is BaseCreature creature && creature.ControlMaster != null && creature.ControlMaster == from && creature.Controlled && creature.CannibalPoints < 5)
                {
                   from.Target = new InternalSecondTarget((Mobile)targeted, m_level);
                }
                else
                {
                    from.SendMessage("You cannot cannibalise with that target.");
                }
            }
        }
        private class InternalSecondTarget : Target
        {
            private BaseCreature m_CannibalCreature;
            private int m_Level;

            public InternalSecondTarget(Mobile cannibalCreature, int level) : base(
                3,
                false,
                TargetFlags.None
            )
            {
                m_Level = level;
                m_CannibalCreature = (BaseCreature)cannibalCreature;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();
                if (targeted is BaseCreature creature && creature.ControlMaster != null && creature.ControlMaster == from && creature.Controlled && creature.GetType() == m_CannibalCreature.GetType())
                {
                    int modifier = (m_CannibalCreature.CannibalPoints > 0) ? m_Level - m_CannibalCreature.CannibalPoints : m_Level;
                    m_CannibalCreature.CannibalPoints = m_Level;
                    m_CannibalCreature = TransferMobileStats((Mobile)targeted, m_CannibalCreature, modifier);
                    m_CannibalCreature.CannibalPoints += modifier;
                    ((Mobile)targeted).Kill();
                }
                else
                {
                    from.SendMessage("You cannot cannibalise that target with your last.");
                }
            }

            public BaseCreature TransferMobileStats(Mobile target, BaseCreature destination, int modifier)
            { 
                destination.RawDex += AOS.Scale(target.RawDex, modifier * 10);
                destination.RawInt += AOS.Scale(target.RawInt, modifier * 10);
                destination.RawStr += AOS.Scale(target.RawStr, modifier * 10);
                return destination;
            }
        }
    }
}
