using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;
using Server.Targeting;

namespace Server.Talent
{
    public class Enchant : BaseTalent, ITalent
    {
        public Enchant() : base()
        {
            TalentDependency = typeof(Disenchant);
            DisplayName = "Enchant";
            Description = "Enchant crafted weaponary with random magical properties. Each level enables more affixes.";
            ImageID = 399;
            CanBeUsed = true;
            MaxLevel = 8;
            GumpHeight = 85;
            AddEndY = 105;
        }

        public override bool HasSkillRequirement(Mobile mobile)
        {
            return Disenchant.CanDisenchant(mobile, 70 + (Level * 3));
        }
        
        public override void OnUse(Mobile from)
        {
            if (from.Backpack != null) {
                from.SendMessage("What item do you wish to enchant?");
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

            public void AffixChances(int level, ref int tierOne, ref int tierTwo, ref int tierThree, ref int tierFour) {
                while (level > 0)
                {
                    if (level >= 1) {
                        tierOne += 10;
                    }
                    if (level >= 2) {
                        tierTwo += 8;
                    }
                    if (level >= 3) {
                        tierThree += 3;
                    }
                    if (level >= 4) {
                        tierFour += 1;
                    }
                    level--;
                }
            }

            public AosElementAttribute RandomElementAttribute() {
                Array attributes = Enum.GetValues(typeof(AosElementAttribute));
                AosElementAttribute randomAttribute = (AosElementAttribute)attributes.GetValue(Utility.Random(attributes.Length));
                return randomAttribute;
            }

            public AosAttribute RandomAttribute() {
                Array attributes = Enum.GetValues(typeof(AosAttribute));
                AosAttribute randomAttribute = (AosAttribute)attributes.GetValue(Utility.Random(attributes.Length));
                return randomAttribute;
            }

            public void CalculateAffixPower(ref int affixPower, ref int numberOfAffixes, ref int numberOfSkills, ref int numberOfElements, int tierOne, int tierTwo, int tierThree, int tierFour) {
                if (Utility.Random(100) < tierFour) {
                    numberOfAffixes = 4;
                    affixPower = 30;
                    numberOfElements = Utility.RandomBool() ? 1 : 0;
                    numberOfSkills = Utility.Random(4);
                }
                if (Utility.Random(100) < tierThree) {
                    numberOfAffixes = 3;
                    affixPower = 15;
                    numberOfElements = Utility.RandomBool() ? 1 : 0;
                    numberOfSkills = Utility.Random(2);
                }
                if (Utility.Random(100) < tierTwo) {
                    numberOfAffixes = 2;
                    affixPower = 6;
                    numberOfSkills = 1;
                    numberOfElements = 0;
                }
                if (Utility.Random(100) < tierOne) {
                    numberOfAffixes = 1;
                    affixPower = 3;
                    numberOfSkills = -1;
                    numberOfElements = 0;
                }
            }

            public AosArmorAttribute RandomArmorAttribute() {
                Array attributes = Enum.GetValues(typeof(AosArmorAttribute));
                AosArmorAttribute randomAttribute = (AosArmorAttribute)attributes.GetValue(Utility.Random(attributes.Length));
                return randomAttribute;
            }

             public BaseArmor GenerateArmorAttributes(BaseArmor armor, int numberOfAffixes, int affixPower, int numberOfSkills, int numberOfElements) {
                int affixes = Utility.Random(numberOfAffixes);
                while(affixes > 0) {
                    AosArmorAttribute attribute = RandomArmorAttribute();
                    if (armor.ArmorAttributes[attribute] >= 0) {
                        affixPower = CheckRareStrike(affixPower);
                        armor.ArmorAttributes[attribute] += Utility.Random(affixPower);
                        affixes--;
                    }
                }
                affixes = Utility.Random(numberOfAffixes);
                 while(affixes > 0) {
                    AosAttribute attribute = RandomAttribute();
                    if (armor.Attributes[attribute] >= 0) {
                        affixPower = CheckRareStrike(affixPower);
                        armor.Attributes[attribute] += Utility.Random(affixPower);
                        affixes--;
                    }
                }
                 if (numberOfSkills > 0) {
                    numberOfSkills--;
                    AosSkillBonuses bonuses = new AosSkillBonuses(armor);
                    while (numberOfSkills >= 0) {
                        bonuses.SetBonus(numberOfSkills, (double)Utility.Random(25));
                        bonuses.SetSkill(numberOfSkills, RandomSkill());
                        numberOfSkills--;
                    }
                    armor.SkillBonuses = bonuses;
                }
                CheckSetAttributes(armor.Attributes);
                CheckSetArmorAttributes(armor.ArmorAttributes);
                return armor;
            }

            public AosWeaponAttribute RandomWeaponAttribute() {
                Array attributes = Enum.GetValues(typeof(AosWeaponAttribute));
                AosWeaponAttribute randomAttribute = (AosWeaponAttribute)attributes.GetValue(Utility.Random(attributes.Length));
                return randomAttribute;
            }

            public SkillName RandomSkill() {
                Array skills = Enum.GetValues(typeof(SkillName));
                SkillName randomSkill = (SkillName)skills.GetValue(Utility.Random(skills.Length));
                return randomSkill;
            }

            public int CheckRareStrike(int affixPower) {
                if (Utility.Random(500) < 1) {
                    affixPower *= 2;
                }
                return affixPower;
            }

            public BaseJewel GenerateJewelAttributes(BaseJewel jewel, int numberOfAffixes, int affixPower, int numberOfSkills, int numberOfElements) {
                int affixes = Utility.Random(numberOfAffixes);
                while(affixes > 0) {
                    AosAttribute attribute = RandomAttribute();
                    if (jewel.Attributes[attribute] == 0) {
                        affixPower = CheckRareStrike(affixPower);
                        jewel.Attributes[attribute] = Utility.Random(affixPower);
                        affixes--;
                    }
                }
                if (numberOfElements > 0) {
                    while (numberOfElements > 0) {
                        AosElementAttribute attribute = RandomElementAttribute();
                        if (jewel.Resistances[attribute] >= 0) {
                            affixPower = CheckRareStrike(affixPower);
                            jewel.Resistances[attribute] += Utility.Random(affixPower);
                            numberOfElements--;
                        }
                    }
                }
                if (numberOfSkills > 0) {
                    numberOfSkills--;
                    AosSkillBonuses bonuses = new AosSkillBonuses(jewel);
                    while (numberOfSkills >= 0) {
                        bonuses.SetBonus(numberOfSkills, (double)Utility.Random(25));
                        bonuses.SetSkill(numberOfSkills, RandomSkill());
                        numberOfSkills--;
                    }
                    jewel.SkillBonuses = bonuses;
                }
                CheckSetAttributes(jewel.Attributes);
                return jewel;
            }

            public BaseWeapon GenerateWeaponAttributes(BaseWeapon weapon, int numberOfAffixes, int affixPower, int numberOfSkills, int numberOfElements) {
                int affixes = Utility.Random(numberOfAffixes);
                while(affixes > 0) {
                    AosWeaponAttribute attribute = RandomWeaponAttribute();
                    if (weapon.WeaponAttributes[attribute] >= 0) {
                        affixPower = CheckRareStrike(affixPower);
                        weapon.WeaponAttributes[attribute] += Utility.Random(affixPower);
                        affixes--;
                    }
                }
                affixes = Utility.Random(numberOfAffixes);
                 while(affixes > 0) {
                    AosAttribute attribute = RandomAttribute();
                    if (weapon.Attributes[attribute] >= 0) {
                        affixPower = CheckRareStrike(affixPower);
                        weapon.Attributes[attribute] += Utility.Random(affixPower);
                        affixes--;
                    }
                }
                if (numberOfElements > 0) {
                    while (numberOfElements > 0) {
                        AosElementAttribute attribute = RandomElementAttribute();
                        if (weapon.AosElementDamages[attribute] >= 0) {
                            affixPower = CheckRareStrike(affixPower);
                            weapon.AosElementDamages[attribute] += Utility.Random(affixPower);
                            numberOfElements--;
                        }
                    }
                }
                if (numberOfSkills > 0) {
                    numberOfSkills--;
                    AosSkillBonuses bonuses = new AosSkillBonuses(weapon);
                    while (numberOfSkills >= 0) {
                        bonuses.SetBonus(numberOfSkills, (double)Utility.Random(25));
                        bonuses.SetSkill(numberOfSkills, RandomSkill());
                        numberOfSkills--;
                    }
                    weapon.SkillBonuses = bonuses;
                }
                
                CheckSetAttributes(weapon.Attributes);
                CheckSetWeaponAttributes(weapon.WeaponAttributes);
                return weapon;
            }

            public void CheckSetWeaponAttributes(AosWeaponAttributes attributes) {
                if (attributes.HitColdArea > 100) {
                    attributes.HitColdArea = 100;
                }
                if (attributes.HitEnergyArea > 100) {
                    attributes.HitEnergyArea = 100;
                }
                if (attributes.HitFireArea > 100) {
                    attributes.HitFireArea = 100;
                }
                if (attributes.HitPoisonArea > 100) {
                    attributes.HitPoisonArea = 100;
                }
                if (attributes.HitPhysicalArea > 100) {
                    attributes.HitPhysicalArea = 100;
                }
                if (attributes.LowerStatReq > 100) {
                    attributes.LowerStatReq = 100;
                }
                if (attributes.SelfRepair > 5) {
                    attributes.SelfRepair = 5;
                }
                if (attributes.HitLeechHits > 100) {
                    attributes.HitLeechHits = 100;
                }
                if (attributes.HitLeechStam > 100) {
                    attributes.HitLeechStam = 100;
                }
                if (attributes.HitLeechMana > 100) {
                    attributes.HitLeechMana = 100;
                }
                if (attributes.HitLowerAttack > 40) {
                    attributes.HitLowerAttack = 40;
                }
                if (attributes.HitLowerDefend > 50) {
                    attributes.HitLowerDefend = 50;
                }
                if (attributes.HitMagicArrow > 25) {
                    attributes.HitMagicArrow = 25;
                }
                if (attributes.HitHarm > 100) {
                    attributes.HitHarm = 100;
                }
                if (attributes.HitFireball > 50) {
                    attributes.HitFireball = 50;
                }
                if (attributes.HitLightning > 60) {
                    attributes.HitLightning = 60;
                }
                if (attributes.HitDispel > 50) {
                    attributes.HitDispel = 50;
                }
                if (attributes.ResistPhysicalBonus > 10) {
                    attributes.ResistPhysicalBonus = 10;
                }
                if (attributes.ResistEnergyBonus > 10) {
                    attributes.ResistEnergyBonus = 10;
                }
                if (attributes.ResistFireBonus > 10) {
                    attributes.ResistFireBonus = 10;
                }
                if (attributes.ResistPoisonBonus > 10) {
                    attributes.ResistPoisonBonus = 10;
                }
                if (attributes.ResistColdBonus > 10) {
                    attributes.ResistColdBonus = 10;
                }
                if (attributes.UseBestSkill > 1) {
                    attributes.UseBestSkill = 1;
                }
                if (attributes.MageWeapon > 30) {
                    attributes.MageWeapon = 30;
                }
                if (attributes.DurabilityBonus > 100) {
                    attributes.DurabilityBonus = 100;
                }
            }
            
            public void CheckSetArmorAttributes(AosArmorAttributes attributes) {
                if (attributes.MageArmor > 1) {
                    attributes.MageArmor = 1;
                }
                if (attributes.LowerStatReq > 100) {
                    attributes.LowerStatReq = 100;
                }
                if (attributes.SelfRepair > 5) {
                    attributes.SelfRepair = 5;
                }
                if (attributes.DurabilityBonus > 100) {
                    attributes.DurabilityBonus = 100;
                }
            }

            public void CheckSetAttributes(AosAttributes attributes) {
                if (attributes.RegenMana > 5) {
                    attributes.RegenMana = 5;
                }
                if (attributes.RegenStam > 5) {
                    attributes.RegenStam = 5;
                }
                if (attributes.RegenHits > 5) {
                    attributes.RegenHits = 5;
                }
                if (attributes.BonusDex > 15) {
                    attributes.BonusDex = 15;
                }
                if (attributes.BonusStr > 15) {
                    attributes.BonusStr = 15;
                }
                if (attributes.BonusInt > 15) {
                    attributes.BonusInt = 15;
                }
                if (attributes.BonusMana > 10) {
                    attributes.BonusMana = 10;
                }
                if (attributes.BonusStam > 10) {
                    attributes.BonusStam = 10;
                }
                if (attributes.BonusHits > 10) {
                    attributes.BonusHits = 10;
                }
                if (attributes.AttackChance > 30) {
                    attributes.AttackChance = 30;
                }
                if (attributes.CastRecovery > 3) {
                    attributes.CastRecovery = 3;
                }
                if (attributes.CastSpeed > 2) {
                    attributes.CastSpeed = 2;
                }
                if (attributes.SpellChanneling > 1) {
                    attributes.SpellChanneling = 1;
                }
                if (attributes.WeaponDamage > 60) {
                    attributes.WeaponDamage = 60;
                }
                if (attributes.WeaponSpeed > 50) {
                    attributes.WeaponSpeed = 50;
                }
                if (attributes.DefendChance > 20) {
                    attributes.DefendChance = 20;
                }
                if (attributes.SpellDamage > 12) {
                    attributes.SpellDamage = 12;
                }
                if (attributes.LowerManaCost > 12) {
                    attributes.LowerManaCost = 12;
                }
                if (attributes.LowerRegCost > 40) {
                    attributes.LowerRegCost = 40;
                }
                if (attributes.ReflectPhysical > 25) {
                    attributes.ReflectPhysical = 25;
                }
                if (attributes.EnhancePotions > 30) {
                    attributes.EnhancePotions = 30;
                }
                if (attributes.NightSight > 1) {
                    attributes.NightSight = 1;
                }
                if (attributes.IncreasedKarmaLoss > 5) {
                    attributes.IncreasedKarmaLoss = 5;
                }
            }

            public SlayerName RandomSlayerName() {
                Array slayers = Enum.GetValues(typeof(SlayerName));
                SlayerName randomSlayer = (SlayerName)slayers.GetValue(Utility.Random(slayers.Length));
                return randomSlayer;
            }

            public BaseWeapon GenerateDamageLevel(BaseWeapon weapon, int tierOne, int tierTwo, int tierThree, int tierFour) 
            {
                if (Utility.Random(100) < tierFour) {
                   if (Utility.RandomBool()) {
                       weapon.Slayer = RandomSlayerName();
                   }
                   weapon.DamageLevel = WeaponDamageLevel.Vanq;
                }
                if (Utility.Random(100) < tierThree) {
                    weapon.DamageLevel = WeaponDamageLevel.Power;
                }
                if (Utility.Random(100) < tierTwo) {
                    weapon.DamageLevel = WeaponDamageLevel.Force;
                }
                if (Utility.Random(100) < tierOne) {
                    if (Utility.RandomBool()) {
                        weapon.DamageLevel = WeaponDamageLevel.Might;
                    } else {
                        weapon.DamageLevel = WeaponDamageLevel.Ruin;
                    }
                }
                return weapon;
            }

            public BaseArmor GenerateProtectionLevel(BaseArmor armor, int tierOne, int tierTwo, int tierThree, int tierFour)
            {
                if (Utility.Random(100) < tierFour) {
                   armor.ProtectionLevel = ArmorProtectionLevel.Invulnerability;
                }
                if (Utility.Random(100) < tierThree) {
                    armor.ProtectionLevel = ArmorProtectionLevel.Fortification;
                }
                if (Utility.Random(100) < tierTwo) {
                    armor.ProtectionLevel = ArmorProtectionLevel.Hardening;
                }
                if (Utility.Random(100) < tierOne) {
                    if (Utility.RandomBool()) {
                        armor.ProtectionLevel = ArmorProtectionLevel.Guarding;
                    } else {
                        armor.ProtectionLevel = ArmorProtectionLevel.Defense;
                    }
                }
                return armor;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                EnchanterDust dust = from.Backpack.FindItemByType<EnchanterDust>();
                if (targeted is Item item && item.IsChildOf(from.Backpack)) 
                {
                    if (dust != null && dust.Amount >= 50) {
                        int tierOne = 0;
                        int tierTwo = 0;
                        int tierThree = 0;
                        int tierFour = 0;
                        int numberOfAffixes = 0;
                        int affixPower = 0;
                        int numberOfSkills = 0;
                        int numberOfElements = 0;
                        AffixChances(m_Talent.Level, ref tierOne, ref tierTwo, ref tierThree, ref tierFour);
                        CalculateAffixPower(ref affixPower, ref numberOfAffixes, ref numberOfSkills, ref numberOfElements, tierOne, tierTwo, tierThree, tierFour);
                        from.RevealingAction();
                        if (targeted is BaseWeapon weapon && weapon.Crafter != null)
                        {
                            weapon = GenerateWeaponAttributes(weapon, numberOfAffixes, affixPower, numberOfSkills, numberOfElements);
                            weapon = GenerateDamageLevel(weapon, tierOne, tierTwo, tierThree, tierFour);
                        } else if (targeted is BaseArmor armor && armor.Crafter != null) 
                        {
                            armor = GenerateArmorAttributes(armor, numberOfAffixes, affixPower, numberOfSkills, numberOfElements);
                            armor = GenerateProtectionLevel(armor, tierOne, tierTwo, tierThree, tierFour);
                        } else if (targeted is BaseJewel jewel && jewel.Crafter != null) 
                        {
                            jewel = GenerateJewelAttributes(jewel, numberOfAffixes, affixPower, numberOfSkills, numberOfElements);
                        } else {
                            from.SendMessage("You cannot enchant this item.");
                            return;
                        }
                        if (numberOfAffixes > 0) {
                            Effects.SendLocationParticles(
                                EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration),
                                0x376A,
                                9,
                                32,
                                5024
                            );
                            from.SendSound(0x1F4);
                            from.SendMessage("You successfully enchant the item with magical properties.");
                        } else {
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
                        if (dust.Amount == 50) {
                            dust.Delete();
                        } else {
                            dust.Amount -= 50;
                        }   
                    } else {
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

