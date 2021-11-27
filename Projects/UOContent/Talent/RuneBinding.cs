using System;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;

namespace Server.Talent
{
    public class RuneBinding : BaseTalent, ITalent
    {
        public RuneBinding() : base()
        {
            DisplayName = "Runebinding";
            TalentDependency = typeof(OrnateCrafter);
            Description = "Enables placing rare runes into sockets to unlock powerful runewords. Requires 40+ smithing/tailoring and tinkering.";
            ImageID = 410;
            CanBeUsed = true;
            MaxLevel = 1;
            GumpHeight = 105;
            AddEndY = 125;
        }

        public override void OnUse(Mobile from)
        {
            if (from.Backpack != null) {
                from.SendMessage("What rune do you wish to work with?");
                from.Target = new InternalTarget(from, this);
            }
        }

        private class InternalTarget : Target
        {
            private RuneBinding m_Talent;
            public InternalTarget(Mobile from, RuneBinding talent) : base(
                2,
                false,
                TargetFlags.None
            )
            {
                m_Talent = talent;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item && item.IsChildOf(from.Backpack)) {
                    OrnateCrafter ornateCrafter = ((PlayerMobile)from).GetTalent(typeof(OrnateCrafter)) as OrnateCrafter;
                    if (targeted is RuneWord && ornateCrafter != null && ornateCrafter.HasSkillRequirement(from)) {
                         from.Target = new InternalSecondTarget(from, (Item)targeted);
                         from.SendMessage("Which socketed item do you wish to use this rune with?");
                    } else {
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
            private Item m_Item;
            public InternalSecondTarget(Mobile from, Item item) : base(
                2,
                false,
                TargetFlags.None
            )
            {
                m_Item = item;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item && item.IsChildOf(from.Backpack)) {
                    SocketBonus.AddItem(from, item, m_Item);
                }
                else 
                {
                    from.SendLocalizedMessage(1045158); //  You must have the item in your backpack to target it.
                }
            }
        }
    }
}

