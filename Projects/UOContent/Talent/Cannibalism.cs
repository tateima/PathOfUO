using Server.Mobiles;
using Server.Targeting;

namespace Server.Talent
{
    public class Cannibalism : BaseTalent
    {
        public Cannibalism()
        {
            TalentDependency = typeof(BondingMaster);
            DisplayName = "Cannibalise pet";
            CanBeUsed = true;
            Description = "Sacrifice another tamed animal, transferring between 10-50% of their stats.";
            ImageID = 376;
        }

        public override void OnUse(Mobile from)
        {
            from.SendMessage("Which pet do you wish to improve?");
            from.Target = new InternalFirstTarget(Level);
        }

        private class InternalFirstTarget : Target
        {
            private readonly int m_level;

            public InternalFirstTarget(int level) : base(
                3,
                false,
                TargetFlags.None
            ) =>
                m_level = level;

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (targeted is BaseCreature { ControlMaster: { } } creature && creature.ControlMaster == @from && creature.Controlled && creature.CannibalPoints < 3)
                {
                    from.SendMessage("Wish pet do you wish to sacrifice?");
                    from.Target = new InternalSecondTarget(creature, m_level);
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
            private readonly int m_Level;

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
                if (targeted is BaseCreature { ControlMaster: { } } creature && creature.ControlMaster == @from && creature.Controlled && creature.GetType() == m_CannibalCreature.GetType() && creature.CannibalPoints == 0)
                {
                    m_CannibalCreature = TransferMobileStats(creature, m_CannibalCreature);
                    m_CannibalCreature.CannibalPoints += 1;
                    creature.Kill();
                }
                else
                {
                    from.SendMessage("You cannot cannibalise that target with your last.");
                }
            }

            public BaseCreature TransferMobileStats(Mobile target, BaseCreature destination)
            {
                destination.RawDex += AOS.Scale(target.RawDex, m_Level * 2);
                destination.RawInt += AOS.Scale(target.RawInt, m_Level * 2);
                destination.RawStr += AOS.Scale(target.RawStr, m_Level * 2);
                destination.SetHits(destination.HitsMax + AOS.Scale(target.HitsMax, m_Level * 2));
                destination.SetMana(destination.ManaMax + AOS.Scale(target.ManaMax, m_Level * 2));
                destination.SetStam(destination.StamMax + AOS.Scale(target.StamMax, m_Level * 2));
                return destination;
            }
        }
    }
}
