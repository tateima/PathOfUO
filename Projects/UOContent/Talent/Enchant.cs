using System;
using Server.Items;
using Server.Targeting;

namespace Server.Talent
{
    public class Enchant : BaseTalent
    {
        public Enchant()
        {
            UpgradeCost = true;
            TalentDependencies = new[] { typeof(Disenchant) };
            DisplayName = "Enchant";
            Description =
                "Enchant crafted weaponry with random magical properties. Each level enables more affixes. Requires at least one magic and one crafting skill above 70+.";
            ImageID = 399;
            CanBeUsed = true;
            MaxLevel = 8;
            GumpHeight = 90;
            AddEndY = 105;
        }

        public override bool HasSkillRequirement(Mobile mobile) => Disenchant.CanDisenchant(mobile, 70 + Level * 3);

        public override void OnUse(Mobile from)
        {
            if (from.Backpack != null && HasSkillRequirement(from))
            {
                from.SendMessage("What item do you wish to enchant?");
                from.Target = new InternalTarget(this);
            }
            else
            {
                from.SendMessage("You don't have the necessary skills to enchant items.");
            }
        }

        public override void UpdateMobile(Mobile mobile)
        {
            var manual = mobile.Backpack?.FindItemByType<EnchantersGuide>() ?? mobile.BankBox?.FindItemByType<EnchantersGuide>();
            if (manual is null)
            {
                mobile.Backpack?.AddItem(new EnchantersGuide());
                mobile.SendMessage("An enchanters guide has been placed in your backpack.");
            }
        }

        public override bool HasUpgradeRequirement(Mobile mobile)
        {
            if (Level == 0)
            {
                return true;
            }
            var enchanterDustCost = Level * 100;
            var goldCost = Level * 250;
            return HasResourceQuantity(mobile, typeof(Gold), goldCost)
                   && HasResourceQuantity(mobile, typeof(EnchanterDust), enchanterDustCost);
        }

        public static AosElementAttribute RandomElementAttribute()
        {
            var attributes = Enum.GetValues(typeof(AosElementAttribute));
            var randomAttribute = (AosElementAttribute)attributes.GetValue(Utility.Random(attributes.Length));
            return randomAttribute;
        }

        public static AosAttribute RandomAttribute()
        {
            var attributes = Enum.GetValues(typeof(AosAttribute));
            var randomAttribute = (AosAttribute)attributes.GetValue(Utility.Random(attributes.Length));
            return randomAttribute;
        }

        public static AosArmorAttribute RandomArmorAttribute()
        {
            var attributes = Enum.GetValues(typeof(AosArmorAttribute));
            var randomAttribute = (AosArmorAttribute)attributes.GetValue(Utility.Random(attributes.Length));
            return randomAttribute;
        }

        public static AosWeaponAttribute RandomWeaponAttribute()
        {
            var attributes = Enum.GetValues(typeof(AosWeaponAttribute));
            var randomAttribute = (AosWeaponAttribute)attributes.GetValue(Utility.Random(attributes.Length));
            return randomAttribute;
        }

        public static SkillName RandomSkill()
        {
            var skills = Enum.GetValues(typeof(SkillName));
            var randomSkill = (SkillName)skills.GetValue(Utility.Random(skills.Length));
            return randomSkill;
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

            public static void AffixChances(int level, ref int tierOne, ref int tierTwo, ref int tierThree, ref int tierFour, ref int tierFive)
            {
                while (level > 0)
                {
                    tierOne += 15;

                    if (level >= 2)
                    {
                        tierTwo += 10;
                    }

                    if (level >= 3)
                    {
                        tierThree += 8;
                    }

                    if (level >= 4)
                    {
                        tierFour += 5;
                    }

                    if (level >= 6)
                    {
                        tierFive += 1;
                    }

                    level--;
                }
            }

            public static void CalculateAffixPower(
                ref int affixPower, ref int numberOfAffixes, ref int numberOfSkills, ref int numberOfElements, int tierOne,
                int tierTwo, int tierThree, int tierFour, int tierFive
            )
            {
                if (Utility.Random(100) < tierFive)
                {
                    numberOfAffixes = 5;
                    affixPower = 50;
                    numberOfElements = Utility.RandomBool() ? 2 : 1;
                    numberOfSkills = Utility.RandomMinMax(1, 5);
                } else if (Utility.Random(100) < tierFour)
                {
                    numberOfAffixes = 4;
                    affixPower = 30;
                    numberOfElements = Utility.RandomBool() ? 1 : 0;
                    numberOfSkills = Utility.Random(4);
                } else if (Utility.Random(100) < tierThree)
                {
                    numberOfAffixes = 3;
                    affixPower = 15;
                    numberOfElements = Utility.RandomBool() ? 1 : 0;
                    numberOfSkills = Utility.Random(2);
                } else if (Utility.Random(100) < tierTwo)
                {
                    numberOfAffixes = 2;
                    affixPower = 6;
                    numberOfSkills = 1;
                    numberOfElements = 0;
                } else if (Utility.Random(100) < tierOne)
                {
                    numberOfAffixes = 1;
                    affixPower = 3;
                    numberOfSkills = -1;
                    numberOfElements = 0;
                }
            }


            public static void GenerateArmorAttributes(
                BaseArmor armor, int numberOfAffixes, int affixPower, int numberOfSkills
            )
            {
                var affixes = Utility.Random(numberOfAffixes);
                while (affixes > 0)
                {
                    var attribute = RandomArmorAttribute();
                    if (armor.ArmorAttributes[attribute] >= 0)
                    {
                        affixPower = CheckRareStrike(affixPower);
                        armor.ArmorAttributes[attribute] += Utility.Random(affixPower);
                        affixes--;
                    }
                }

                affixes = Utility.Random(numberOfAffixes);
                while (affixes > 0)
                {
                    var attribute = RandomAttribute();
                    if (armor.Attributes[attribute] >= 0)
                    {
                        affixPower = CheckRareStrike(affixPower);
                        armor.Attributes[attribute] += Utility.Random(affixPower);
                        affixes--;
                    }
                }

                if (numberOfSkills > 0)
                {
                    numberOfSkills--;
                    var bonuses = new AosSkillBonuses(armor);
                    while (numberOfSkills >= 0)
                    {
                        bonuses.SetBonus(numberOfSkills, Utility.Random(25));
                        bonuses.SetSkill(numberOfSkills, RandomSkill());
                        numberOfSkills--;
                    }

                    armor.SkillBonuses = bonuses;
                }

                CheckSetAttributes(armor.Attributes);
                CheckSetArmorAttributes(armor.ArmorAttributes);
            }


            public static int CheckRareStrike(int affixPower)
            {
                if (Utility.Random(500) < 1)
                {
                    affixPower *= 2;
                }

                return affixPower;
            }

            public static void GenerateJewelAttributes(
                BaseJewel jewel, int numberOfAffixes, int affixPower, int numberOfSkills, int numberOfElements
            )
            {
                var affixes = Utility.Random(numberOfAffixes);
                while (affixes > 0)
                {
                    var attribute = RandomAttribute();
                    if (jewel.Attributes[attribute] == 0)
                    {
                        affixPower = CheckRareStrike(affixPower);
                        jewel.Attributes[attribute] = Utility.Random(affixPower);
                        affixes--;
                    }
                }

                if (numberOfElements > 0)
                {
                    while (numberOfElements > 0)
                    {
                        var attribute = RandomElementAttribute();
                        if (jewel.Resistances[attribute] >= 0)
                        {
                            affixPower = CheckRareStrike(affixPower);
                            jewel.Resistances[attribute] += Utility.Random(affixPower);
                            numberOfElements--;
                        }
                    }
                }

                if (numberOfSkills > 0)
                {
                    numberOfSkills--;
                    var bonuses = new AosSkillBonuses(jewel);
                    while (numberOfSkills >= 0)
                    {
                        bonuses.SetBonus(numberOfSkills, Utility.Random(25));
                        bonuses.SetSkill(numberOfSkills, RandomSkill());
                        numberOfSkills--;
                    }

                    jewel.SkillBonuses = bonuses;
                }

                CheckSetAttributes(jewel.Attributes);
            }

            public static void GenerateWeaponAttributes(
                BaseWeapon weapon, int numberOfAffixes, int affixPower, int numberOfSkills, int numberOfElements
            )
            {
                if (numberOfAffixes > 0)
                {
                    var affixes = Utility.RandomMinMax(1, numberOfAffixes);
                    while (affixes > 0)
                    {
                        var attribute = RandomWeaponAttribute();
                        if (weapon.WeaponAttributes[attribute] >= 0)
                        {
                            affixPower = CheckRareStrike(affixPower);
                            weapon.WeaponAttributes[attribute] += Utility.Random(affixPower);
                            affixes--;
                        }
                    }

                    affixes = Utility.RandomMinMax(1, numberOfAffixes);
                    while (affixes > 0)
                    {
                        var attribute = RandomAttribute();
                        if (weapon.Attributes[attribute] >= 0)
                        {
                            affixPower = CheckRareStrike(affixPower);
                            weapon.Attributes[attribute] += Utility.Random(affixPower);
                            affixes--;
                        }
                    }
                }
                if (numberOfElements > 0)
                {
                    while (numberOfElements > 0)
                    {
                        var attribute = RandomElementAttribute();
                        if (weapon.AosElementDamages[attribute] >= 0)
                        {
                            affixPower = CheckRareStrike(affixPower);
                            weapon.AosElementDamages[attribute] += Utility.Random(affixPower);
                            numberOfElements--;
                        }
                    }
                }

                if (numberOfSkills > 0)
                {
                    var bonuses = new AosSkillBonuses(weapon);
                    while (numberOfSkills >= 0)
                    {
                        bonuses.SetBonus(numberOfSkills, Utility.Random(25));
                        bonuses.SetSkill(numberOfSkills, RandomSkill());
                        numberOfSkills--;
                    }
                    weapon.SkillBonuses = bonuses;
                }

                CheckSetAttributes(weapon.Attributes);
                CheckSetWeaponAttributes(weapon.WeaponAttributes);
            }

            public static void CheckSetWeaponAttributes(AosWeaponAttributes attributes)
            {
                if (attributes.HitColdArea > 100)
                {
                    attributes.HitColdArea = 100;
                }

                if (attributes.HitEnergyArea > 100)
                {
                    attributes.HitEnergyArea = 100;
                }

                if (attributes.HitFireArea > 100)
                {
                    attributes.HitFireArea = 100;
                }

                if (attributes.HitPoisonArea > 100)
                {
                    attributes.HitPoisonArea = 100;
                }

                if (attributes.HitPhysicalArea > 100)
                {
                    attributes.HitPhysicalArea = 100;
                }

                if (attributes.LowerStatReq > 100)
                {
                    attributes.LowerStatReq = 100;
                }

                if (attributes.SelfRepair > 5)
                {
                    attributes.SelfRepair = 5;
                }

                if (attributes.HitLeechHits > 100)
                {
                    attributes.HitLeechHits = 100;
                }

                if (attributes.HitLeechStam > 100)
                {
                    attributes.HitLeechStam = 100;
                }

                if (attributes.HitLeechMana > 100)
                {
                    attributes.HitLeechMana = 100;
                }

                if (attributes.HitLowerAttack > 40)
                {
                    attributes.HitLowerAttack = 40;
                }

                if (attributes.HitLowerDefend > 50)
                {
                    attributes.HitLowerDefend = 50;
                }

                if (attributes.HitMagicArrow > 25)
                {
                    attributes.HitMagicArrow = 25;
                }

                if (attributes.HitHarm > 100)
                {
                    attributes.HitHarm = 100;
                }

                if (attributes.HitFireball > 50)
                {
                    attributes.HitFireball = 50;
                }

                if (attributes.HitLightning > 60)
                {
                    attributes.HitLightning = 60;
                }

                if (attributes.HitDispel > 50)
                {
                    attributes.HitDispel = 50;
                }

                if (attributes.ResistPhysicalBonus > 10)
                {
                    attributes.ResistPhysicalBonus = 10;
                }

                if (attributes.ResistEnergyBonus > 10)
                {
                    attributes.ResistEnergyBonus = 10;
                }

                if (attributes.ResistFireBonus > 10)
                {
                    attributes.ResistFireBonus = 10;
                }

                if (attributes.ResistPoisonBonus > 10)
                {
                    attributes.ResistPoisonBonus = 10;
                }

                if (attributes.ResistColdBonus > 10)
                {
                    attributes.ResistColdBonus = 10;
                }

                if (attributes.UseBestSkill > 1)
                {
                    attributes.UseBestSkill = 1;
                }

                if (attributes.MageWeapon > 30)
                {
                    attributes.MageWeapon = 30;
                }

                if (attributes.DurabilityBonus > 100)
                {
                    attributes.DurabilityBonus = 100;
                }
            }

            public static void CheckSetArmorAttributes(AosArmorAttributes attributes)
            {
                if (attributes.MageArmor > 1)
                {
                    attributes.MageArmor = 1;
                }

                if (attributes.LowerStatReq > 100)
                {
                    attributes.LowerStatReq = 100;
                }

                if (attributes.SelfRepair > 5)
                {
                    attributes.SelfRepair = 5;
                }

                if (attributes.DurabilityBonus > 100)
                {
                    attributes.DurabilityBonus = 100;
                }
            }

            public static void CheckSetAttributes(AosAttributes attributes)
            {
                if (attributes.RegenMana > 5)
                {
                    attributes.RegenMana = 5;
                }

                if (attributes.RegenStam > 5)
                {
                    attributes.RegenStam = 5;
                }

                if (attributes.RegenHits > 5)
                {
                    attributes.RegenHits = 5;
                }

                if (attributes.BonusDex > 15)
                {
                    attributes.BonusDex = 15;
                }

                if (attributes.BonusStr > 15)
                {
                    attributes.BonusStr = 15;
                }

                if (attributes.BonusInt > 15)
                {
                    attributes.BonusInt = 15;
                }

                if (attributes.BonusMana > 10)
                {
                    attributes.BonusMana = 10;
                }

                if (attributes.BonusStam > 10)
                {
                    attributes.BonusStam = 10;
                }

                if (attributes.BonusHits > 10)
                {
                    attributes.BonusHits = 10;
                }

                if (attributes.AttackChance > 30)
                {
                    attributes.AttackChance = 30;
                }

                if (attributes.CastRecovery > 3)
                {
                    attributes.CastRecovery = 3;
                }

                if (attributes.CastSpeed > 2)
                {
                    attributes.CastSpeed = 2;
                }

                if (attributes.SpellChanneling > 1)
                {
                    attributes.SpellChanneling = 1;
                }

                if (attributes.WeaponDamage > 60)
                {
                    attributes.WeaponDamage = 60;
                }

                if (attributes.WeaponSpeed > 50)
                {
                    attributes.WeaponSpeed = 50;
                }

                if (attributes.DefendChance > 20)
                {
                    attributes.DefendChance = 20;
                }

                if (attributes.SpellDamage > 12)
                {
                    attributes.SpellDamage = 12;
                }

                if (attributes.LowerManaCost > 12)
                {
                    attributes.LowerManaCost = 12;
                }

                if (attributes.LowerRegCost > 40)
                {
                    attributes.LowerRegCost = 40;
                }

                if (attributes.ReflectPhysical > 25)
                {
                    attributes.ReflectPhysical = 25;
                }

                if (attributes.EnhancePotions > 30)
                {
                    attributes.EnhancePotions = 30;
                }

                if (attributes.NightSight > 1)
                {
                    attributes.NightSight = 1;
                }

                if (attributes.IncreasedKarmaLoss > 5)
                {
                    attributes.IncreasedKarmaLoss = 5;
                }
            }

            public static SlayerName RandomSlayerName()
            {
                var slayers = Enum.GetValues(typeof(SlayerName));
                var randomSlayer = (SlayerName)slayers.GetValue(Utility.Random(slayers.Length));
                return randomSlayer;
            }

            public static void GenerateDamageLevel(BaseWeapon weapon, int tierOne, int tierTwo, int tierThree, int tierFour)
            {
                if (Utility.Random(100) < tierFour)
                {
                    if (Utility.RandomBool())
                    {
                        weapon.Slayer = RandomSlayerName();
                    }
                    weapon.DamageLevel = WeaponDamageLevel.Vanq;
                } else if (Utility.Random(100) < tierThree) {
                    weapon.DamageLevel = WeaponDamageLevel.Power;
                } else if (Utility.Random(100) < tierTwo) {
                    weapon.DamageLevel = WeaponDamageLevel.Force;
                } else if (Utility.Random(100) < tierOne) {
                    weapon.DamageLevel = Utility.RandomBool() ? WeaponDamageLevel.Might : WeaponDamageLevel.Ruin;
                }
            }

            public static void GenerateProtectionLevel(BaseArmor armor, int tierOne, int tierTwo, int tierThree, int tierFour)
            {
                if (Utility.Random(100) < tierFour)
                {
                    armor.ProtectionLevel = ArmorProtectionLevel.Invulnerability;
                } else if (Utility.Random(100) < tierThree) {
                    armor.ProtectionLevel = ArmorProtectionLevel.Fortification;
                } else if (Utility.Random(100) < tierTwo) {
                    armor.ProtectionLevel = ArmorProtectionLevel.Hardening;
                } else if (Utility.Random(100) < tierOne) {
                    armor.ProtectionLevel = Utility.RandomBool() ? ArmorProtectionLevel.Guarding : ArmorProtectionLevel.Defense;
                }
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                var dust = from.Backpack.FindItemByType<EnchanterDust>();
                if (targeted is Item item && item.IsChildOf(from.Backpack))
                {
                    if (dust is { Amount: >= 50 })
                    {
                        bool useResources = false;
                        if (item is RuneWord && dust.Amount >= 500)
                        {
                            // roll a new runeword
                            var rune = new RuneWord();
                            from.Backpack.DropItem(rune);
                            item.Delete();
                            dust.Consume(500);
                            return;
                        }

                        var tierOne = 0;
                        var tierTwo = 0;
                        var tierThree = 0;
                        var tierFour = 0;
                        var tierFive = 0;
                        var numberOfAffixes = 0;
                        var affixPower = 0;
                        var numberOfSkills = 0;
                        var numberOfElements = 0;
                        AffixChances(_talent.Level, ref tierOne, ref tierTwo, ref tierThree, ref tierFour, ref tierFive);
                        CalculateAffixPower(
                            ref affixPower,
                            ref numberOfAffixes,
                            ref numberOfSkills,
                            ref numberOfElements,
                            tierOne,
                            tierTwo,
                            tierThree,
                            tierFour,
                            tierFive
                        );
                        from.RevealingAction();
                        if (targeted is BaseWeapon weapon)
                        {
                            if (!weapon.Enchanted)
                            {
                                GenerateWeaponAttributes(
                                    weapon,
                                    numberOfAffixes,
                                    affixPower,
                                    numberOfSkills,
                                    numberOfElements
                                );
                                GenerateDamageLevel(weapon, tierOne, tierTwo, tierThree, tierFour);
                                if (numberOfAffixes > 0 || numberOfSkills > 0 || numberOfElements > 0)
                                {
                                    useResources = true;
                                    weapon.Enchanted = true;
                                }
                            }
                            else
                            {
                                from.SendMessage("This weapon already has an enchantment");
                                return;
                            }
                        }
                        else if (targeted is Items.BaseArmor armor)
                        {
                            if (!armor.Enchanted)
                            {
                                GenerateArmorAttributes(
                                    armor,
                                    numberOfAffixes,
                                    affixPower,
                                    numberOfSkills
                                );
                                GenerateProtectionLevel(armor, tierOne, tierTwo, tierThree, tierFour);
                                if (numberOfAffixes > 0 || numberOfSkills > 0)
                                {
                                    useResources = true;
                                    armor.Enchanted = true;
                                }
                            }
                            else
                            {
                                from.SendMessage("This armor already has an enchantment");
                                return;
                            }

                        }
                        else if (targeted is BaseJewel jewel)
                        {
                            if (!jewel.Enchanted)
                            {
                                GenerateJewelAttributes(
                                    jewel,
                                    numberOfAffixes,
                                    affixPower,
                                    numberOfSkills,
                                    numberOfElements
                                );
                                if (numberOfAffixes > 0 || numberOfSkills > 0 || numberOfElements > 0)
                                {
                                    useResources = true;
                                    jewel.Enchanted = true;
                                }
                            }
                            else
                            {
                                from.SendMessage("This jewellery already has an enchantment");
                                return;
                            }
                        }
                        else
                        {
                            from.SendMessage("You cannot enchant this item.");
                            return;
                        }

                        if (numberOfAffixes > 0)
                        {
                            Effects.SendLocationParticles(
                                EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration),
                                0x376A,
                                9,
                                32,
                                5024
                            );
                            from.SendSound(0x1F4);
                            from.SendMessage("You successfully enchant the item with magical properties.");
                        }
                        else
                        {
                            useResources = true;
                            if (Core.AOS)
                            {
                                from.FixedParticles(0x3735, 1, 30, 9503, EffectLayer.Waist);
                            }
                            else
                            {
                                from.FixedEffect(0x3735, 6, 30);
                            }

                            from.PlaySound(0x5C);
                            from.SendMessage("You fail to enchant the item with any magical properties.");
                        }

                        if (useResources)
                        {
                            dust.Consume(50);
                        }
                    }
                    else
                    {
                        from.SendMessage("You do not have enough enchanter dust in your backpack.");
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
