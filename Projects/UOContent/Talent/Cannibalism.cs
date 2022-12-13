using Server.Mobiles;
using Server.Targeting;

namespace Server.Talent
{
    public class Cannibalism : BaseTalent
    {
        public Cannibalism()
        {
            TalentDependencies = new[] { typeof(BondingMaster) };
            DisplayName = "Cannibalise pet";
            CanBeUsed = true;
            Description = "Sacrifice another tamed animal, transferring their stats to a target creature.";
            AdditionalDetail = "You may only sacrifice a maximum of 3 tamed creatures to your chosen target. The transfer increases by 5% per level.";
            ImageID = 376;
            AddEndAdditionalDetailsY = 70;
        }

        public override void OnUse(Mobile from)
        {
            from.SendMessage("Which pet do you wish to improve?");
            from.Target = new InternalFirstTarget(Level);
        }

        private class InternalFirstTarget : Target
        {
            private readonly int _level;

            public InternalFirstTarget(int level) : base(
                3,
                false,
                TargetFlags.None
            ) =>
                _level = level;

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (targeted is BaseCreature { ControlMaster: { } } creature && creature.ControlMaster == @from && creature.Controlled && creature.CannibalPoints < 3)
                {
                    from.SendMessage("Wish pet do you wish to sacrifice?");
                    from.Target = new InternalSecondTarget(creature, _level);
                }
                else
                {
                    from.SendMessage("You cannot cannibalise with that target.");
                }
            }
        }

        private class InternalSecondTarget : Target
        {
            private BaseCreature _cannibalCreature;
            private readonly int _level;

            public InternalSecondTarget(Mobile cannibalCreature, int level) : base(
                3,
                false,
                TargetFlags.None
            )
            {
                _level = level;
                _cannibalCreature = (BaseCreature)cannibalCreature;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();
                if (targeted is BaseCreature { ControlMaster: { } } creature && creature.ControlMaster == @from && creature.Controlled && creature.GetType() == _cannibalCreature.GetType() && creature.CannibalPoints == 0)
                {
                    _cannibalCreature = TransferMobileStats(creature, _cannibalCreature);
                    _cannibalCreature.CannibalPoints += 1;
                    creature.Kill();
                }
                else
                {
                    from.SendMessage("You cannot cannibalise that target with your last.");
                }
            }

            public BaseCreature TransferMobileStats(Mobile target, BaseCreature destination)
            {
                destination.RawDex += AOS.Scale(target.RawDex, _level * 2);
                destination.RawInt += AOS.Scale(target.RawInt, _level * 2);
                destination.RawStr += AOS.Scale(target.RawStr, _level * 2);
                destination.SetHits(destination.HitsMax + AOS.Scale(target.HitsMax, _level * 2));
                destination.SetMana(destination.ManaMax + AOS.Scale(target.ManaMax, _level * 2));
                destination.SetStam(destination.StamMax + AOS.Scale(target.StamMax, _level * 2));
                return destination;
            }
        }
    }
}
