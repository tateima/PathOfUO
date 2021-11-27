using System;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Engines.Craft;

namespace Server.Talent
{
    public class OrnateCrafter : BaseTalent, ITalent
    {
        public OrnateCrafter() : base()
        {
            DisplayName = "Ornate crafter";
            Description = "Add sockets to weapons and armour or pockets to clothing for runes and gems. Also enables placing items into sockets. Requires 40+ smithing/tailoring and tinkering.";
            ImageID = 409;
            CanBeUsed = true;
            MaxLevel = 6;
            GumpHeight = 115;
            AddEndY = 145;
        }

        public static BaseTool GetSkillItem(Mobile mobile, string type)
        {
            BaseTool skillItem = null;
            if (mobile.Backpack != null)
            {
                if (type == "metal")
                {
                    skillItem = mobile.Backpack.FindItemByType<SmithHammer>();
                    skillItem = (skillItem is null) ? mobile.Backpack.FindItemByType<Tongs>() : skillItem;
                } else if (type == "cloth")
                {
                    skillItem = mobile.Backpack.FindItemByType<SewingKit>();
                } else {
                    skillItem = mobile.Backpack.FindItemByType<TinkersTools>();
                }
            }
            return skillItem;
        }
        
        public static bool CanSocket(Mobile mobile, int level) 
        {
            switch (level)
            {
                case 0:
                    return mobile.Skills.Blacksmith.Base >= 40.0 && mobile.Skills.Tinkering.Base >= 40.0;
                    break;
                case 1:
                    return mobile.Skills.Blacksmith.Base >= 50.0 && mobile.Skills.Tinkering.Base >= 50.0;
                    break;
                case 2:
                    return mobile.Skills.Blacksmith.Base >= 60.0 && mobile.Skills.Tinkering.Base >= 60.0;
                    break;
                case 3:
                    return mobile.Skills.Blacksmith.Base >= 70.0 && mobile.Skills.Tinkering.Base >= 70.0;
                    break;
                case 4:
                    return mobile.Skills.Blacksmith.Base >= 70.0 && mobile.Skills.Tinkering.Base >= 80.0;
                    break;
                case 5:
                    return mobile.Skills.Blacksmith.Base >= 90.0 && mobile.Skills.Tinkering.Base >= 90.0;
                    break;
                case 6:
                    return mobile.Skills.Blacksmith.Base >= 98.0 && mobile.Skills.Tinkering.Base >= 98.0;
                    break;
            }
            return false;
        }
        public static bool CanPocket(Mobile mobile, int level)
        {
            switch (level)
            {
                case 0:
                    return mobile.Skills.Tailoring.Base >= 40.0 && mobile.Skills.Tinkering.Base >= 40.0;
                    break;
                case 1:
                    return mobile.Skills.Tailoring.Base >= 50.0 && mobile.Skills.Tinkering.Base >= 50.0;
                    break;
                case 2:
                    return mobile.Skills.Tailoring.Base >= 60.0 && mobile.Skills.Tinkering.Base >= 60.0;
                    break;
                case 3:
                    return mobile.Skills.Tailoring.Base >= 70.0 && mobile.Skills.Tinkering.Base >= 70.0;
                    break;
                case 4:
                    return mobile.Skills.Tailoring.Base >= 80.0 && mobile.Skills.Tinkering.Base >= 80.0;
                    break;
                case 5:
                    return mobile.Skills.Tailoring.Base >= 90.0 && mobile.Skills.Tinkering.Base >= 90.0;
                    break;
                case 6:
                    return mobile.Skills.Tailoring.Base >= 98.0 && mobile.Skills.Tinkering.Base >= 98.0;
                    break;
            }
            return false;
        }

        public static bool CanSocketOrPocket(Mobile mobile, int level)
        {
            bool validPocketer = CanPocket(mobile, level);
            bool validSocketer = CanSocket(mobile, level);
            return validSocketer || validPocketer;
        }

        public override bool HasSkillRequirement(Mobile mobile)
        {
            return CanSocketOrPocket(mobile, Level);
        }
        
        public override void OnUse(Mobile from)
        {
            if (from.Backpack != null) {
                from.SendMessage("What item do you wish to work with?");
                from.Target = new InternalTarget(from, this);
            }
        }

        public static bool SocketSuccess(Mobile from, int chance, Item item) {
            if (Utility.Random(100) < chance) {
                return true;
            } else {
                from.SendSound(0x03E);
                from.SendMessage("The item was destroyed in your attempt");
                item.Delete();
                return false;
            }
        }

        private class InternalTarget : Target
        {
            private OrnateCrafter m_Talent;
            public InternalTarget(Mobile from, OrnateCrafter talent) : base(
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
                    int number = Utility.Random(m_Talent.Level);
                    string overheadMessage = "";
                    
                    BaseTool smithItem = GetSkillItem(from, "metal");
                    BaseTool tailorItem = GetSkillItem(from, "cloth");
                    BaseTool tinkerItem = GetSkillItem(from, "jewels");

                    DefBlacksmithy.CheckAnvilAndForge(from, 2, out var anvil, out var forge);
                    bool canSmith = (smithItem != null && anvil && forge);
                    bool socketedItem = false;
                    bool pocketedItem = false;
                    if (targeted is BaseWeapon weapon && canSmith && weapon.SocketAmount == 0 && SocketSuccess(from, 10 - number, weapon) && CanSocket(from, m_Talent.Level)) {
                        weapon.SocketAmount = number;
                        socketedItem = true;
                        overheadMessage = "* You add sockets to the weapon *";
                    } else if (targeted is BaseArmor armor && canSmith && armor.SocketAmount == 0 && SocketSuccess(from, 10 - number, armor) && CanSocket(from, m_Talent.Level)) { 
                        armor.SocketAmount = number;
                        socketedItem = true;
                        overheadMessage = "* You add sockets to the armor *";
                    } else if (targeted is BaseWaist waist && tailorItem != null && waist.PocketAmount == 0 && SocketSuccess(from, 10 - number, waist) && CanPocket(from, m_Talent.Level)) {
                        waist.PocketAmount = number;
                        pocketedItem = true;
                        overheadMessage = "* You add pockets to the waist cloth *";
                    } else if (targeted is BaseHat hat && tailorItem != null && hat.PocketAmount == 0 && SocketSuccess(from, 10 - number, hat) && CanPocket(from, m_Talent.Level)) {
                        hat.PocketAmount = number;
                        pocketedItem = true;
                        overheadMessage = "* You add pockets to the hat *";
                    } else if (targeted is BaseJewel jewel && tinkerItem != null && jewel.SocketAmount == 0 && SocketSuccess(from, 10 - number, jewel) && CanSocketOrPocket(from, m_Talent.Level)) {
                        jewel.SocketAmount = number;
                        pocketedItem = true;
                        overheadMessage = "* You add sockets to the jewellery *";
                    } else if (SocketBonus.IsGem(item)) {
                         from.Target = new InternalSecondTarget(from, item);
                         from.SendMessage("Which socketed item do you wish to use this item with?");
                    } else {
                        if (item is BaseWeapon || item is BaseArmor && !canSmith) {
                            if (smithItem is null)
                            {
                                from.SendMessage("You do not have a smithing item!");
                            }
                            else
                            {
                                from.SendLocalizedMessage(1044267); // You must be near an anvil and a forge to smith items.
                            }
                        } else {
                            from.SendMessage("You cannot work with this item.");
                        }
                    }
                    if (!string.IsNullOrEmpty(overheadMessage)) {
                        if (socketedItem)
                        {
                            smithItem.UsesRemaining--;
                            from.PlaySound(0x2A);
                        } else if (pocketedItem)
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

