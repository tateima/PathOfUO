using System;
using Server;
using Server.Mobiles;
using Server.Talent;
using Server.Targeting;

namespace Server.Items
{
    public class BaseShard : Item
    {
        public override double DefaultWeight
        {
            get { return 0.1; }
        }

        public override int LabelNumber { get { return 1061195; } } // frozen shard
        [Constructible]
        public BaseShard() : base(0x023E)
        {
            Stackable = true;
            Light = LightType.Circle150;
            Hue = MonsterBuff.FrozenHue;
        }

        public BaseShard(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Light = LightType.Circle150;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
        }
        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                if (from is PlayerMobile player) {
                    BaseTalent meld = player.GetTalent(typeof(Meld));
                    if (meld != null) {
                        from.SendMessage("What would you like to meld this elemental shard with?");
                        from.Target = new ShardTarget(from, this);
                    } else {
                        from.SendMessage("You do not have the necessary talent to meld this elemental shard.");
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(1045158); //  You must have the item in your backpack to target it.
            }
        }

        public void CheckDelete(bool use)
        {
            if (use && Amount <= 1)
            {
                Delete();
            }
            else
            {
                Amount--;
            }
        }

        public virtual void AddElementalProperties(Mobile from, BaseWeapon weapon)
        {
            Delete();
        }
        public virtual void AddElementalProperties(Mobile from, BaseArmor armor)
        {
            Delete();
        }
        private class ShardTarget : Target
        {
            private BaseShard m_BaseShard;
            public ShardTarget(Mobile from, BaseShard shard) : base(
                1,
                false,
                TargetFlags.None
            )
            {
                m_BaseShard = shard;
            } 

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item && (targeted is BaseWeapon || targeted is BaseArmor))
                {
                    if (item.IsChildOf(from.Backpack))
                    {
                        if (item is BaseWeapon weapon)
                        {
                            if (weapon.ShardPower < 10)
                            {
                                m_BaseShard.AddElementalProperties(from, weapon);
                                weapon.InvalidateProperties();
                            } 
                            else
                            {
                                from.SendLocalizedMessage(1061201); //You cannot imbue the properties of this shard with this item
                            }
                            
                        } else if (item is BaseArmor armor)
                        {
                            if (armor.ShardPower < 15) {
                                m_BaseShard.AddElementalProperties(from, armor);
                                armor.InvalidateProperties();
                            } 
                            else 
                            {
                                from.SendLocalizedMessage(1061201); //You cannot imbue the properties of this shard with this item
                            }
                            
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(1045158); //  You must have the item in your backpack to target it.
                    }
                } else
                {
                    from.SendLocalizedMessage(1061201); //You cannot imbue the properties of this shard with this item
                }
            }
        }
    }
}
