using System;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class UnMeld : BaseTalent, ITalent
    {
        public UnMeld() : base()
        {
            TalentDependency = typeof(Enchant);
            DisplayName = "Unmeld";
            Description = "Attempt to remove elemental shards from item. Can fail and destroy item completely. Each level decreases chance of failure.";
            ImageID = 392;
            CanBeUsed = true;
            MaxLevel = 5;
            GumpHeight = 105;
            AddEndY = 125;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            return Disenchant.CanDisenchant(mobile, 90);
        }
        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                OnCooldown = true;
                from.SendMessage("What item do you wish to attempt the unmeld?");
                from.Target = new InternalTarget(from, this);
            }
        }
         private class InternalTarget : Target
        {
            private BaseTalent m_Talent;
            public InternalTarget(Mobile from, BaseTalent talent) : base(
                2,
                false,
                TargetFlags.None
            )
            {
                m_Talent = talent;
            }
            public bool Success(Mobile from, Item item) {
                if (Utility.Random(100) < (5 + m_Talent.Level)) {
                    return true;
                } else {
                    from.SendSound(0x03E);
                    item.Delete();
                    from.SendMessage("The item shatters.");
                }
                return false;
            }
            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item && item.IsChildOf(from.Backpack)) 
                {
                    int shards = 0;
                    int hue = 0;
                    if (targeted is BaseWeapon weapon && weapon.ShardPower > 0) {
                        if (Success(from, weapon)) {
                            shards += weapon.ShardPower;
                            hue = weapon.Hue;
                            weapon.Hue = 0;
                        }
                    } else if (targeted is BaseArmor armor && armor.ShardPower > 0) {
                        if (Success(from, armor)) {
                            shards += armor.ShardPower;
                            hue = armor.Hue;
                            armor.Hue = 0;
                        }   
                    }
                    if (shards > 0) {
                        BaseShard shard = null;
                        if (hue == MonsterBuff.ToxicHue) {
                            shard = new ToxicShard(shards);
                        } else if (hue == MonsterBuff.FrozenHue) {
                            shard = new FrozenShard(shards);
                        } else if (hue == MonsterBuff.BurningHue) {
                            shard = new BurningShard(shards);
                        } else if (hue == MonsterBuff.ElectrifiedHue) {
                            shard = new ElectrifiedShard(shards);
                        }
                        if (shard != null && from.Backpack != null) {
                            Effects.SendLocationParticles(
                                EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration),
                                0x376A,
                                9,
                                32,
                                5024
                            );
                            from.SendSound(0x1F4);
                            from.Backpack.AddItem(shard);
                            from.SendMessage("You successfully extract the melded elemental shards.");
                        }
                    }
                }
                else 
                {
                    from.SendLocalizedMessage(1045158); //  You must have the item in your backpack to target it.
                }
            }

        }
    }
}

