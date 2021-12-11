using System;
using Server.Engines.Craft;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Targeting;

namespace Server.Talent
{
    public class OrnateCrafter : BaseTalent
    {
        public OrnateCrafter()
        {
            DisplayName = "Ornate crafter";
            Description =
                "Add sockets to weapons and armour or pockets to clothing for runes and gems. Also enables placing items into sockets. Requires 40+ smithing/tailoring and tinkering.";
            ImageID = 409;
            CanBeUsed = true;
            MaxLevel = 6;
            GumpHeight = 115;
            AddEndY = 130;
        }

        public override bool HasSkillRequirement(Mobile mobile) => CanSocketOrPocket(mobile, Level);

        public static BaseTool GetSkillItem(Mobile mobile, string type)
        {
            BaseTool skillItem = null;
            if (mobile.Backpack != null)
            {
                if (type == "metal")
                {
                    skillItem = mobile.Backpack.FindItemByType<SmithHammer>();
                    skillItem ??= mobile.Backpack.FindItemByType<Tongs>();
                }
                else if (type == "cloth")
                {
                    skillItem = mobile.Backpack.FindItemByType<SewingKit>();
                }
                else
                {
                    skillItem = mobile.Backpack.FindItemByType<TinkersTools>();
                }
            }

            return skillItem;
        }

        public static bool CanSocket(Mobile mobile, int level)
        {
            return level switch
            {
                0 => mobile.Skills.Blacksmith.Base >= 40.0 && mobile.Skills.Tinkering.Base >= 40.0,
                1 => mobile.Skills.Blacksmith.Base >= 50.0 && mobile.Skills.Tinkering.Base >= 50.0,
                2 => mobile.Skills.Blacksmith.Base >= 60.0 && mobile.Skills.Tinkering.Base >= 60.0,
                3 => mobile.Skills.Blacksmith.Base >= 70.0 && mobile.Skills.Tinkering.Base >= 70.0,
                4 => mobile.Skills.Blacksmith.Base >= 70.0 && mobile.Skills.Tinkering.Base >= 80.0,
                5 => mobile.Skills.Blacksmith.Base >= 90.0 && mobile.Skills.Tinkering.Base >= 90.0,
                6 => mobile.Skills.Blacksmith.Base >= 98.0 && mobile.Skills.Tinkering.Base >= 98.0,
                _ => false
            };
        }

        public static bool CanPocket(Mobile mobile, int level)
        {
            return level switch
            {
                0 => mobile.Skills.Tailoring.Base >= 40.0 && mobile.Skills.Tinkering.Base >= 40.0,
                1 => mobile.Skills.Tailoring.Base >= 50.0 && mobile.Skills.Tinkering.Base >= 50.0,
                2 => mobile.Skills.Tailoring.Base >= 60.0 && mobile.Skills.Tinkering.Base >= 60.0,
                3 => mobile.Skills.Tailoring.Base >= 70.0 && mobile.Skills.Tinkering.Base >= 70.0,
                4 => mobile.Skills.Tailoring.Base >= 80.0 && mobile.Skills.Tinkering.Base >= 80.0,
                5 => mobile.Skills.Tailoring.Base >= 90.0 && mobile.Skills.Tinkering.Base >= 90.0,
                6 => mobile.Skills.Tailoring.Base >= 98.0 && mobile.Skills.Tinkering.Base >= 98.0,
                _ => false
            };
        }

        public static bool CanSocketOrPocket(Mobile mobile, int level)
        {
            var validPocketer = CanPocket(mobile, level);
            var validSocketer = CanSocket(mobile, level);
            return validSocketer || validPocketer;
        }

        public override void OnUse(Mobile from)
        {
            if (from.Backpack != null)
            {
                from.SendMessage("What item do you wish to work with?");
                from.Target = new InternalTarget(this);
            }
        }

        public static bool SocketSuccess(Mobile from, int number, Item item)
        {
            // 6 = 0.5%
            // 5 = 9.6 %
            // 4 = 15.7 %
            // 3 = 19.4 %
            // 2 = 21.3 %
            // 1 = 22 %
            for (var round = 1; round <= number; round++)
            {
                double diff = (long)Math.Pow(number, 3.0);
                int divider = (round >= 3) ? (8 - round) : 3;
                double chance = (221.0 - diff) / divider;;
                if (Utility.Random(1000) < chance)
                {
                    return true;
                }
            }
            from.SendSound(0x03E);
            from.SendMessage("The item was destroyed in your attempt");
            item.Delete();
            return false;
        }

        private class InternalTarget : Target
        {
            private readonly OrnateCrafter m_Talent;

            public InternalTarget(OrnateCrafter talent) : base(
                2,
                false,
                TargetFlags.None
            ) =>
                m_Talent = talent;

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item && item.IsChildOf(from.Backpack))
                {
                    var number = Utility.Random(m_Talent.Level);
                    var overheadMessage = "";
                    var smithItem = GetSkillItem(from, "metal");
                    var tailorItem = GetSkillItem(from, "cloth");
                    var tinkerItem = GetSkillItem(from, "jewels");

                    DefBlacksmithy.CheckAnvilAndForge(from, 2, out var anvil, out var forge);
                    var canSmith = smithItem != null && anvil && forge;
                    var socketItem = false;
                    var pocketedItem = false;
                    if (targeted is BaseWeapon weapon && canSmith && weapon.SocketAmount == 0 &&
                        SocketSuccess(from, number, weapon) && CanSocket(from, m_Talent.Level))
                    {
                        weapon.SocketAmount = number;
                        socketItem = true;
                        overheadMessage = "* You add sockets to the weapon *";
                    }
                    else if (targeted is BaseArmor armor && canSmith && armor.SocketAmount == 0 &&
                             SocketSuccess(from, number, armor) && CanSocket(from, m_Talent.Level))
                    {
                        armor.SocketAmount = number;
                        socketItem = true;
                        overheadMessage = "* You add sockets to the armor *";
                    }
                    else if (targeted is BaseWaist waist && tailorItem != null && waist.PocketAmount == 0 &&
                             SocketSuccess(from, number, waist) && CanPocket(from, m_Talent.Level))
                    {
                        waist.PocketAmount = number;
                        pocketedItem = true;
                        overheadMessage = "* You add pockets to the waist cloth *";
                    }
                    else if (targeted is BaseHat hat && tailorItem != null && hat.PocketAmount == 0 &&
                             SocketSuccess(from, number, hat) && CanPocket(from, m_Talent.Level))
                    {
                        hat.PocketAmount = number;
                        pocketedItem = true;
                        overheadMessage = "* You add pockets to the hat *";
                    }
                    else if (targeted is BaseJewel jewel && tinkerItem != null && jewel.SocketAmount == 0 &&
                             SocketSuccess(from, number, jewel) && CanSocketOrPocket(from, m_Talent.Level))
                    {
                        jewel.SocketAmount = number;
                        pocketedItem = true;
                        overheadMessage = "* You add sockets to the jewellery *";
                    }
                    else if (SocketBonus.IsGem(item))
                    {
                        from.Target = new InternalSecondTarget(item);
                        from.SendMessage("Which socketed item do you wish to use this item with?");
                    }
                    else
                    {
                        if (item is BaseWeapon || item is BaseArmor && !canSmith)
                        {
                            if (smithItem is null)
                            {
                                from.SendMessage("You do not have a smithing item!");
                            }
                            else
                            {
                                from.SendLocalizedMessage(1044267); // You must be near an anvil and a forge to smith items.
                            }
                        }
                        else
                        {
                            from.SendMessage("You cannot work with this item.");
                        }
                    }

                    if (!string.IsNullOrEmpty(overheadMessage))
                    {
                        if (socketItem)
                        {
                            smithItem.UsesRemaining--;
                            from.PlaySound(0x2A);
                        }
                        else if (pocketedItem)
                        {
                            tailorItem.UsesRemaining--;
                            from.PlaySound(0x248);
                        }

                        from.PublicOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            false,
                            overheadMessage
                        );
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
            private readonly Item m_Item;

            public InternalSecondTarget(Item item) : base(
                2,
                false,
                TargetFlags.None
            ) =>
                m_Item = item;

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item && item.IsChildOf(from.Backpack))
                {
                    SocketBonus.AddItem(from, item, m_Item);
                    SocketBonus.AddSocketProperties(m_Item, item);
                }
                else
                {
                    from.SendLocalizedMessage(1045158); //  You must have the item in your backpack to target it.
                }
            }
        }
    }
}
