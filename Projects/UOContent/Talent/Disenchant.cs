using System;
using System.Linq;
using Server.Items;
using Server.Targeting;

namespace Server.Talent
{
    public class Disenchant : BaseTalent
    {
        public Disenchant()
        {
            DisplayName = "Disenchant";
            Description =
                "Can disenchant non-crafted magical items. Each level enables the disenchantment of more powerful items. Requires at least one magic and one crafting skill above 70+.";
            ImageID = 400;
            CanBeUsed = true;
            MaxLevel = 3;
            GumpHeight = 90;
            AddEndY = 125;
        }

        public override bool HasSkillRequirement(Mobile mobile) => CanDisenchant(mobile, 70 + Level * 5);

        public static bool CanDisenchant(Mobile mobile, int level)
        {
            var validMagic = mobile.Skills[SkillName.Necromancy].Base >= level ||
                             mobile.Skills[SkillName.Magery].Base >= level ||
                             mobile.Skills[SkillName.Mysticism].Base >= level
                             || mobile.Skills[SkillName.Chivalry].Base >= level ||
                             mobile.Skills[SkillName.Spellweaving].Base >= level;
            var validCrafting = mobile.Skills[SkillName.Blacksmith].Base >= level ||
                                mobile.Skills[SkillName.Tailoring].Base >= level ||
                                mobile.Skills[SkillName.Fletching].Base >= level
                                || mobile.Skills[SkillName.Alchemy].Base >= level ||
                                mobile.Skills[SkillName.Tinkering].Base >= level;
            return validMagic && validCrafting;
        }

        public static double TallyElementalAttributes(AosElementAttributes elementAttrs)
        {
            var amount = 0.0;
            if (elementAttrs != null)
            {
                amount += (from attribute in (AosElementAttribute[])Enum.GetValues(typeof(AosElementAttribute)) where elementAttrs[attribute] > 0 select 1.50).AsParallel().Sum();
            }

            return amount;
        }

        public static double TallyArmorAttributes(AosArmorAttributes armorAttrs)
        {
            var amount = 0.0;
            if (armorAttrs != null)
            {
                amount += (from attribute in (AosArmorAttribute[])Enum.GetValues(typeof(AosArmorAttribute)) where armorAttrs[attribute] > 0 select 1.50).AsParallel().Sum();
            }

            return amount;
        }

        public static double TallyWeaponAttributes(AosWeaponAttributes weaponAttrs)
        {
            var amount = 0.0;
            if (weaponAttrs != null)
            {
                amount += (from attribute in (AosWeaponAttribute[])Enum.GetValues(typeof(AosWeaponAttribute)) where weaponAttrs[attribute] > 0 select 1.50).AsParallel().Sum();
            }

            return amount;
        }

        public static double TallyAttributes(AosAttributes attrs)
        {
            var amount = 0.0;
            if (attrs != null)
            {
                amount += (from attribute in (AosAttribute[])Enum.GetValues(typeof(AosAttribute)) where attrs[attribute] > 0 select 1.50).AsParallel().Sum();
            }

            return amount;
        }

        public static double TallySkillBonuses(AosSkillBonuses skillBonuses)
        {
            var amount = 0.0;
            for (var i = 0; i < 5; ++i)
            {
                if (skillBonuses != null && skillBonuses.GetValues(i, out var skill, out var bonus))
                {
                    if (bonus > 0.0)
                    {
                        amount += 1.25;
                    }
                }
            }

            return amount;
        }

        public override void OnUse(Mobile from)
        {
            from.SendMessage("What item do you wish to disenchant?");
            from.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private readonly BaseTalent m_Talent;

            public InternalTarget(BaseTalent talent) : base(
                2,
                false,
                TargetFlags.None
            ) =>
                m_Talent = talent;

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item && item.IsChildOf(from.Backpack))
                {
                    var amount = 0.0;
                    from.RevealingAction();
                    if (targeted is BaseWeapon weapon)
                    {
                        if (m_Talent.Level >= 2)
                        {
                            amount += TallyWeaponAttributes(weapon.WeaponAttributes);
                            amount += TallyAttributes(weapon.Attributes);
                            amount += TallyElementalAttributes(weapon.AosElementDamages);
                            amount += TallySkillBonuses(weapon.SkillBonuses);
                        }

                        if (weapon.DamageLevel == WeaponDamageLevel.Ruin && m_Talent.Level >= 1)
                        {
                            amount += 1.0;
                        }

                        if (weapon.DamageLevel == WeaponDamageLevel.Might && m_Talent.Level >= 1)
                        {
                            amount += 2.5;
                        }

                        if (weapon.DamageLevel == WeaponDamageLevel.Force && m_Talent.Level >= 2)
                        {
                            amount += 3.0;
                        }

                        if (weapon.DamageLevel == WeaponDamageLevel.Power && m_Talent.Level >= 2)
                        {
                            amount += 5.5;
                        }

                        if (weapon.DamageLevel == WeaponDamageLevel.Vanq && m_Talent.Level >= 3)
                        {
                            amount += 8.0;
                        }

                        if (weapon.Slayer != SlayerName.None && weapon.Slayer2 != SlayerName.None && m_Talent.Level >= 3)
                        {
                            amount += 8.0;
                        }
                    }
                    else if (targeted is BaseArmor armor)
                    {
                        if (m_Talent.Level >= 2)
                        {
                            amount += TallyArmorAttributes(armor.ArmorAttributes);
                            amount += TallyAttributes(armor.Attributes);
                            amount += TallySkillBonuses(armor._skillBonuses);
                        }

                        if (armor.ProtectionLevel == ArmorProtectionLevel.Guarding && m_Talent.Level > 1)
                        {
                            amount += 1.50;
                        }

                        if (armor.ProtectionLevel == ArmorProtectionLevel.Hardening && m_Talent.Level > 2)
                        {
                            amount += 2.50;
                        }

                        if (armor.ProtectionLevel == ArmorProtectionLevel.Fortification && m_Talent.Level > 2)
                        {
                            amount += 3.5;
                        }

                        if (armor.ProtectionLevel == ArmorProtectionLevel.Invulnerability && m_Talent.Level > 3)
                        {
                            amount += 5.5;
                        }
                    }
                    else if (targeted is BaseJewel jewel)
                    {
                        if (m_Talent.Level >= 2)
                        {
                            amount += jewel.ArtifactRarity;
                            amount += TallyAttributes(jewel.Attributes);
                            amount += TallySkillBonuses(jewel.SkillBonuses);
                            amount += TallyElementalAttributes(jewel.Resistances);
                        }
                    }


                    if (amount > 0)
                    {
                        item.Delete();
                        var dust = new EnchanterDust((int)Math.Ceiling(amount));
                        if (from.Backpack != null)
                        {
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
                    }
                    else
                    {
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
