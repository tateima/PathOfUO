using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Talent
{
    public class Runebinding : BaseTalent
    {
        public Runebinding()
        {
            DisplayName = "Runebinding";
            TalentDependency = typeof(OrnateCrafter);
            Description =
                "Enables placing rare runes into sockets to unlock powerful runewords. Requires 40+ smithing/tailoring and tinkering.";
            ImageID = 410;
            CanBeUsed = true;
            MaxLevel = 1;
            GumpHeight = 105;
            AddEndY = 125;
        }

        public override void OnUse(Mobile from)
        {
            if (from.Backpack != null)
            {
                from.SendMessage("What rune do you wish to work with?");
                from.Target = new InternalTarget();
            }
        }

        public override void UpdateMobile(Mobile mobile)
        {
            var manual = mobile.Backpack?.FindItemByType<RunebindingGuide>() ?? mobile.BankBox?.FindItemByType<RunebindingGuide>();
            if (manual is null)
            {
                mobile.Backpack?.AddItem(new RunebindingGuide());
                mobile.SendMessage("A runebinding guide has been placed in your backpack.");
            }
        }

        private class InternalTarget : Target
        {
            public InternalTarget() : base(
                2,
                false,
                TargetFlags.None
            )
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item && item.IsChildOf(from.Backpack))
                {
                    var ornateCrafter = ((PlayerMobile)from).GetTalent(typeof(OrnateCrafter)) as OrnateCrafter;
                    if (targeted is RuneWord word && ornateCrafter?.HasSkillRequirement(from) == true)
                    {
                        from.Target = new InternalSecondTarget(word);
                        from.SendMessage("Which socketed item do you wish to use this rune with?");
                    }
                    else
                    {
                        from.SendMessage("You cannot work with this item");
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1045158); //  You must have the item in your backpack to target it.
                }
            }
        }

        private class InternalSecondTarget : Target
        {
            private readonly Item _item;

            public InternalSecondTarget(Item item) : base(
                2,
                false,
                TargetFlags.None
            ) =>
                _item = item;

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item && item.IsChildOf(from.Backpack))
                {
                    SocketBonus.AddItem(from, item, _item);
                    SocketBonus.AddSocketProperties(_item, item);
                }
                else
                {
                    from.SendLocalizedMessage(1045158); //  You must have the item in your backpack to target it.
                }
            }
        }
    }
}
