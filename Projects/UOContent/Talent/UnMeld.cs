using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Talent
{
    public class UnMeld : BaseTalent
    {
        public UnMeld()
        {
            TalentDependency = typeof(Enchant);
            DisplayName = "Unmeld";
            Description =
                "Attempt to remove elemental shards from item. Can fail and destroy item completely. Each level decreases chance of failure.";
            ImageID = 392;
            CanBeUsed = true;
            MaxLevel = 5;
            GumpHeight = 105;
            AddEndY = 125;
        }

        public override bool HasSkillRequirement(Mobile mobile) => Disenchant.CanDisenchant(mobile, 90);

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                OnCooldown = true;
                from.SendMessage("What item do you wish to attempt the unmeld?");
                from.Target = new InternalTarget(this);
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        private class InternalTarget : Target
        {
            private readonly BaseTalent _talent;

            public InternalTarget(BaseTalent talent) : base(
                2,
                false,
                TargetFlags.None
            ) =>
                _talent = talent;

            public bool Success(Mobile from, Item item)
            {
                if (Utility.Random(100) < 5 + _talent.Level)
                {
                    return true;
                }

                from.SendSound(0x03E);
                item.Delete();
                from.SendMessage("The item shatters.");
                return false;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item && item.IsChildOf(from.Backpack))
                {
                    var shards = 0;
                    var hue = 0;
                    if (targeted is BaseWeapon { ShardPower: > 0 } weapon)
                    {
                        if (Success(from, weapon))
                        {
                            shards += weapon.ShardPower;
                            hue = weapon.Hue;
                            weapon.Hue = 0;
                        }
                    }
                    else if (targeted is BaseArmor { ShardPower: > 0 } armor)
                    {
                        if (Success(from, armor))
                        {
                            shards += armor.ShardPower;
                            hue = armor.Hue;
                            armor.Hue = 0;
                        }
                    }

                    if (shards > 0)
                    {
                        BaseShard shard = null;
                        if (hue == MonsterBuff.ToxicHue)
                        {
                            shard = new ToxicShard(shards);
                        }
                        else if (hue == MonsterBuff.FrozenHue)
                        {
                            shard = new FrozenShard(shards);
                        }
                        else if (hue == MonsterBuff.BurningHue)
                        {
                            shard = new BurningShard(shards);
                        }
                        else if (hue == MonsterBuff.ElectrifiedHue)
                        {
                            shard = new ElectrifiedShard(shards);
                        }

                        if (shard != null && from.Backpack != null)
                        {
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
