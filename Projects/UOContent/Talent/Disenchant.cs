using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;
using Server.Targeting;

namespace Server.Talent
{
    public class Disenchant : BaseTalent, ITalent
    {
        public Disenchant() : base()
        {
            DisplayName = "Disenchant";
            Description = "Can disenchant non-crafted magical items. Each level enables the disenchantment of more powerful items.";
            ImageID = 400;
            CanBeUsed = true;
            MaxLevel = 3;
            GumpHeight = 85;
            AddEndY = 105;
        }

        public static bool CanDisenchant(Mobile mobile, int level) {
            bool validMagic = mobile.Skills[SkillName.Necromancy].Base >= level || mobile.Skills[SkillName.Magery].Base >= level || mobile.Skills[SkillName.Mysticism].Base >= level 
                    || mobile.Skills[SkillName.Chivalry].Base >= level || mobile.Skills[SkillName.Spellweaving].Base >= level;
            bool validCrafting = mobile.Skills[SkillName.Blacksmith].Base >= level || mobile.Skills[SkillName.Tailoring].Base >= level || mobile.Skills[SkillName.Fletching].Base >= level 
                    || mobile.Skills[SkillName.Alchemy].Base >= level || mobile.Skills[SkillName.Tinkering].Base >= level;
            return validMagic && validCrafting;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            return CanDisenchant(mobile, 70 + (Level * 5));
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                OnCooldown = true;
                from.SendMessage("What item do you wish to disenchant?");
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

            public double TallyElementalAttributes(AosElementAttributes elementAttrs) {
                double amount = 0.0;
                if (elementAttrs != null)
                {
                    foreach (AosElementAttribute attribute in (AosElementAttribute[]) Enum.GetValues(typeof(AosElementAttribute)))
                    {
                        if (elementAttrs[attribute] > 0) {
                            amount += 0.50;
                        }
                    }
                }
                return amount;
            }

            public double TallyArmorAttributes(AosArmorAttributes armorAttrs) {
                double amount = 0.0;
                if (armorAttrs != null)
                {
                    foreach (AosArmorAttribute attribute in (AosArmorAttribute[]) Enum.GetValues(typeof(AosArmorAttribute)))
                    {
                        if (armorAttrs[attribute] > 0) {
                            amount += 0.50;
                        }
                    }
                }
                return amount;
            }

            public double TallyWeaponAttributes(AosWeaponAttributes weaponAttrs) {
                double amount = 0.0;
                if (weaponAttrs != null)
                {
                    foreach (AosWeaponAttribute attribute in (AosWeaponAttribute[]) Enum.GetValues(typeof(AosWeaponAttribute)))
                    {
                        if (weaponAttrs[attribute] > 0) {
                            amount += 0.25;
                        }
                    }
                }
                return amount;
            }

            public double TallyAttributes(AosAttributes attrs) {
                double amount = 0.0;
                if (attrs != null) {
                    foreach (AosAttribute attribute in (AosAttribute[]) Enum.GetValues(typeof(AosAttribute)))
                    {
                        if (attrs[attribute] > 0) {
                            amount += 0.25;
                        }
                    }
                }
                return amount;
            }

            public double TallySkillBonuses(AosSkillBonuses skillBonuses) {
                 double amount = 0.0;
                if (skillBonuses != null) {

                }
                for (var i = 0; i < 5; ++i)
                {
                    if (skillBonuses.GetValues(i, out var skill, out var bonus))
                    {
                        if (bonus > 0.0) {
                            amount += 0.25;
                        }
                    }
                }
                return amount;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item && item.IsChildOf(from.Backpack)) 
                {
                    double amount = 0.0;
                    from.RevealingAction();
                    if (targeted is BaseWeapon weapon && weapon.Crafter is null)
                    {
                        if (m_Talent.Level >= 2) {
                            amount += TallyWeaponAttributes(weapon.WeaponAttributes);
                            amount += TallyAttributes(weapon.Attributes);
                            amount += TallyElementalAttributes(weapon.AosElementDamages);
                            amount += TallySkillBonuses(weapon.SkillBonuses);
                        }
                        if (weapon.DamageLevel == WeaponDamageLevel.Ruin && m_Talent.Level >= 1) {
                            amount += 0.25;
                        }
                        if (weapon.DamageLevel == WeaponDamageLevel.Might && m_Talent.Level >= 1) {
                            amount += 0.5;
                        }
                        if (weapon.DamageLevel == WeaponDamageLevel.Force && m_Talent.Level >= 2) {
                            amount += 1.0;
                        }
                        if (weapon.DamageLevel == WeaponDamageLevel.Power && m_Talent.Level >= 2) {
                            amount += 2.5;
                        }
                        if (weapon.DamageLevel == WeaponDamageLevel.Vanq && m_Talent.Level >= 3) {
                            amount += 4.0;
                        }
                        if (weapon.Slayer != SlayerName.None && weapon.Slayer2 != SlayerName.None && m_Talent.Level >= 3) {
                            amount += 2.5;
                        }
                    } else if (targeted is BaseArmor armor && armor.Crafter is null) {
                        if (m_Talent.Level >= 2) {
                            amount += TallyArmorAttributes(armor.ArmorAttributes);
                            amount += TallyAttributes(armor.Attributes);
                            amount += TallySkillBonuses(armor._skillBonuses);
                        }
                        if (armor.ProtectionLevel == ArmorProtectionLevel.Guarding && m_Talent.Level > 1) {
                            amount += 0.50;
                        }
                        if (armor.ProtectionLevel == ArmorProtectionLevel.Hardening && m_Talent.Level > 2) {
                            amount += 0.75;
                        }
                        if (armor.ProtectionLevel == ArmorProtectionLevel.Fortification && m_Talent.Level > 2) {
                            amount += 1.5;
                        }
                        if (armor.ProtectionLevel == ArmorProtectionLevel.Invulnerability && m_Talent.Level > 3) {
                            amount += 3.5;
                        }
                    } else if (targeted is BaseJewel jewel && jewel.Crafter is null) {
                        if (m_Talent.Level >= 2) {
                            amount += (double)jewel.ArtifactRarity;
                            amount += TallyAttributes(jewel.Attributes);
                            amount += TallySkillBonuses(jewel.SkillBonuses);
                            amount += TallyElementalAttributes(jewel.Resistances);
                        }
                    }

                    
                    if (amount > 0) {

                        EnchanterDust dust = new EnchanterDust((int)Math.Ceiling(amount));
                        if (from.Backpack != null) {
                            Effects.SendLocationParticles(
                                EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration),
                                0x376A,
                                9,
                                32,
                                5024
                            );
                            from.SendSound(0x1F4);
                            from.Backpack.AddItem(dust);
                            from.SendMessage("You disenchant magical dust from the item.");
                        }
                    } else {
                        from.SendMessage("You cannot disenchant this item.");
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

