using System;
using System.Collections.Generic;
using System.Linq;
using Server.Factions;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Pantheon;
using Server.Spells;

namespace Server.Talent
{
    public class BaseTalent : ITalent
    {
        public static string CriticalDamageDetail = "A critical strike will do double damage.";
        public static string PassiveDetail = "This is a passive talent and does not incur any costs.";
        public const int InvalidTalentIndex = 999;
        public static readonly Type[] TalentTypes =
        {
            typeof(ViperAspect),
            typeof(VenomBlood),
            typeof(WyvernAspect),
            typeof(GreaterPoisonElemental),
            typeof(FireAffinity),
            typeof(DragonAspect),
            typeof(Warmth),
            typeof(GreaterFireElemental),
            typeof(DarkAffinity),
            typeof(SummonerCommand),
            typeof(SpectralScream),
            typeof(MasterOfDeath),
            typeof(LightAffinity),
            typeof(GuardianLight),
            typeof(HolyAvenger),
            typeof(HolyBolt),
            typeof(SonicAffinity),
            typeof(Resonance),
            typeof(MassConfusion),
            typeof(DominateCreature),
            typeof(NatureAffinity),
            typeof(RangerCommand),
            typeof(BondingMaster),
            typeof(PackLeader),
            typeof(Cannibalism),
            typeof(DivineStrength),
            typeof(GiantsHeritage),
            typeof(IronSkin),
            typeof(BoneBreaker),
            typeof(PainManagement),
            typeof(DivineDexterity),
            typeof(KeenEye),
            typeof(TrueSighted),
            typeof(KeenSenses),
            typeof(EscapeDeath),
            typeof(HandFinesse),
            typeof(DivineIntellect),
            typeof(FastLearner),
            typeof(ManaShield),
            typeof(PlanarShift),
            typeof(WarMagicFocus),
            typeof(SpellMind),
            typeof(FrostFire),
            typeof(DryThunderstorm),
            typeof(SwordsmanshipFocus),
            typeof(SwordSpecialist),
            typeof(BarrierGuard),
            typeof(PolearmSpecialist),
            typeof(Cleave),
            typeof(AxeSpecialist),
            typeof(FencingFocus),
            typeof(SpearSpecialist),
            typeof(Javelin),
            typeof(FencingSpecialist),
            typeof(BackStab),
            typeof(Riposte),
            typeof(Fearless),
            typeof(MacefightingFocus),
            typeof(MageCombatant),
            typeof(MaceSpecialist),
            typeof(MountedCombat),
            typeof(CranialStrike),
            typeof(TwoHandedMaceSpecialist),
            typeof(GroundSlam),
            typeof(ShieldFocus),
            typeof(ShieldBash),
            typeof(Phalanx),
            typeof(ClothWarmaster),
            typeof(LeatherWarmaster),
            typeof(BoneWarmaster),
            typeof(ChainWarmaster),
            typeof(PlateWarmaster),
            typeof(DragonWarmaster),
            typeof(SpellWard),
            typeof(ArcherFocus),
            typeof(BlindingShot),
            typeof(ChemicalWarfare),
            typeof(Disengage),
            typeof(CrossbowSpecialist),
            typeof(IceBolt),
            typeof(BowSpecialist),
            typeof(MultiShot),
            typeof(CarefulShooter),
            typeof(LoreSeeker),
            typeof(LoreDisciples),
            typeof(LoreTeacher),
            typeof(ExperiencedHunter),
            typeof(AbyssalHunter),
            typeof(ArachnidHunter),
            typeof(ElementalHunter),
            typeof(HumanoidHunter),
            typeof(ReptilianHunter),
            typeof(UndeadHunter),
            typeof(WarCraftFocus),
            typeof(ResourcefulCrafter),
            typeof(OptimisedConsumption),
            typeof(TycoonCrafter),
            typeof(StrongTools),
            typeof(ResourcefulHarvester),
            typeof(EfficientCarver),
            typeof(EfficientSkinner),
            typeof(EfficientSmelter),
            typeof(EfficientSpinner),
            typeof(SlaveDriver),
            typeof(SmoothTalker),
            typeof(TaxCollector),
            typeof(Gambler),
            typeof(HireHenchman),
            typeof(MerchantPorter),
            typeof(Polymeter),
            typeof(WhizzyGig),
            typeof(ThingAMaBob),
            typeof(Telewarper),
            typeof(Inventive),
            typeof(BugFixer),
            typeof(Automaton),
            typeof(Disenchant),
            typeof(Enchant),
            typeof(Meld),
            typeof(UnMeld),
            typeof(AstralBorn),
            typeof(GraveDigger),
            typeof(ExperimentalFood),
            typeof(ExperimentalDrink),
            typeof(Fortitude),
            typeof(CharmUndead),
            typeof(Mediumship),
            typeof(Detective),
            typeof(OrnateCrafter),
            typeof(Runebinding),
            typeof(SummonChaosElemental),
            typeof(FlameWave),
            typeof(ChaoticGrip),
            typeof(Zealot),
            typeof(DivineProtection),
            typeof(SummonCelestial),
            typeof(Heroism),
            typeof(LayOnHands),
            typeof(GodlyStrike),
            typeof(SiphonLife),
            typeof(WellOfDeath),
            typeof(BloodLink),
            typeof(Bribery),
            typeof(Gluttony),
            typeof(Deception),
            typeof(Wisdom),
            typeof(KeyToTheCity),
            typeof(Leadership)
        };

        public TimerExecutionToken _talentTimerToken;


        public BaseTalent()
        {
            DeityAlignment = Deity.Alignment.None;
            RequiresDeityFavor = false;
            StatModNames = Array.Empty<string>();
            AddEndAdditionalDetailsY = 50;
            CooldownSeconds = 0;
            ManaRequired = 0;
            StamRequired = 0;
            SlowedCreatures = new List<BaseCreature>();
            SlowModifier = 2;
            CanAbsorbSpells = false;
            UpgradeCost = false;
            Activated = false;
            OnCooldown = false;
            CanBeUsed = false;
            HasDamageAbsorptionEffect = false;
            HasDefenseEffect = false;
            HasDeathEffect = false;
            HasKillEffect = false;
            UpdateOnEquip = false;
            HasBeforeDeathSave = false;
            MobilePercentagePerPoint = 3;
            BlockedBy = Array.Empty<Type>();
            RequiredWeapon = Array.Empty<Type>();
            RequiredWeaponSkill = SkillName.Carpentry;
            RequiredSpell = Array.Empty<Type>();
            IncreaseHitChance = false;
            IncreaseParryChance = false;
            TalentDependencyPoints = 1;
            Level = 0;
            MaxLevel = 5;
            DisplayName = "Basic Talent";
            Description = "Does something.";
            AdditionalDetail = "";
            ImageID = 30145;
            AddEndY = 110;
            GumpHeight = 200;
        }


        public virtual double SpecialDamageScalar => Core.AOS ? 0.16 : 0.05;

        public string[] StatModNames { get; set; }
        public int CooldownSeconds { get; set; }
        public int SlowModifier { get; set; }

        public List<BaseCreature> SlowedCreatures { get; set; }

        public bool RequiresDeityFavor { get; set; }

        public bool UpdateOnEquip { get; set; }

        public bool HasBeforeDeathSave { get; set; }

        public bool HasKillEffect { get; set; }

        public bool HasDefenseEffect { get; set; }

        public bool CanAbsorbSpells { get; set; }

        public bool HasDamageAbsorptionEffect { get; set; }

        public bool HasDeathEffect { get; set; }

        public bool CanBeUsed { get; set; }

        public bool Activated { get; set; }

        public bool UpgradeCost { get; set; }

        public bool OnCooldown { get; set; }

        public int ManaRequired { get; set; }

        public int StamRequired { get; set; }

        public Deity.Alignment DeityAlignment { get; set; }

        public int TalentDependencyPoints { get; set; }

        public int GumpHeight { get; set; }

        public bool IncreaseParryChance { get; set; }

        public bool IncreaseHitChance { get; set; }

        public SkillName RequiredWeaponSkill { get; set; }

        public string Description { get; set; }

        public string AdditionalDetail { get; set; }

        public string DisplayName { get; set; }

        public int MobilePercentagePerPoint { get; set; }

        public Type[] RequiredWeapon { get; set; }

        public Type[] RequiredSpell { get; set; }

        public ResistanceMod ResMod { get; set; }

        public int Level { get; set; }

        public int AddEndAdditionalDetailsY { get; set; }

        public int AddEndY { get; set; }

        public Type[] BlockedBy { get; set; }

        public int MaxLevel { get; set; }

        public Type TalentDependency { get; set; }

        public int ImageID { get; set; }

        public virtual void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
        }

        public virtual void UpdateMobile(Mobile mobile)
        {
        }

        public virtual int ModifySpellMultiplier() => Level;

        public virtual bool HasSkillRequirement(Mobile mobile) => true;
        public virtual bool HasUpgradeRequirement(Mobile mobile) => true;

        public static BaseTalent[] GetTalentDependency(PlayerMobile player, BaseTalent talent)
        {
            if (talent.TalentDependency != null)
            {
                BaseTalent dependsOn = TalentConstructor.Construct(talent.TalentDependency) as BaseTalent;
                if (dependsOn != null)
                {
                    BaseTalent hasDependency = player.GetTalent(dependsOn.GetType());
                    return new[]
                    {
                        dependsOn,
                        hasDependency
                    };
                }
            }
            return Array.Empty<BaseTalent>();
        }

        public virtual bool CanApplyHitEffect(Item i)
        {
            var valid = RequiredWeapon.FirstOrDefault(w => w == i.GetType() || i.GetType().IsSubclassOf(w));
            var validSkill = false;
            if (i is BaseWeapon bw)
            {
                validSkill = RequiredWeaponSkill == bw.DefSkill;
            }

            return valid != null || validSkill;
        }

        public virtual void CheckMissEffect(Mobile attacker, Mobile target)
        {
        }

        public virtual void CheckDefenderMissEffect(Mobile attacker, Mobile target)
        {
        }

        public virtual int GetHitChanceModifier() => Level;

        public virtual bool CanScaleSpellDamage(Spell spell)
        {
            var valid = RequiredSpell.FirstOrDefault(w => w == spell.GetType() || spell.GetType().IsSubclassOf(w));
            return valid != null;
        }

        public virtual double ModifySpellScalar() => 0.0;

        public virtual void CheckSpellEffect(Mobile attacker, Mobile target)
        {
        }

        public virtual int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage) => damage;

        public virtual void CheckDefenseEffect(Mobile defender, Mobile attacker, int damage)
        {
        }

        public virtual void CheckDamageEffect(Mobile defender, Mobile target, int damage)
        {
        }


        public virtual void CheckDeathEffect(Mobile target)
        {
        }

        public virtual void CheckKillEffect(Mobile victim, Mobile killer)
        {
        }

        public virtual void CheckBeforeDeathEffect(Mobile target)
        {
        }

        public virtual Mobile ScaleMobile(Mobile mobile)
        {
            mobile = ScaleMobileStats(mobile);
            mobile = ScaleMobileSkills(mobile, "Magical");
            mobile = ScaleMobileSkills(mobile, "Combat Ratings");
            mobile = ScaleMobileSkills(mobile, "Lore & Knowledge");
            return mobile;
        }

        public virtual Mobile ScaleMobileStats(Mobile mobile)
        {
            // default affinity scaling is 3% per level
            var modifier = Level * MobilePercentagePerPoint;
            mobile.RawDex += AOS.Scale(mobile.RawDex, modifier);
            mobile.RawInt += AOS.Scale(mobile.RawInt, modifier);
            mobile.RawStr += AOS.Scale(mobile.RawStr, modifier);
            return mobile;
        }

        public virtual Mobile ScaleMobileSkills(Mobile mobile, string skillGroupName)
        {
            var group = SkillsGumpGroup.Groups.FirstOrDefault(group => group.Name == skillGroupName);
            if (group != null)
            {
                foreach (var skill in group.Skills)
                {
                    mobile.Skills[skill].Base += Level * MobilePercentagePerPoint;
                }
            }

            return mobile;
        }


        public static void CriticalStrike(ref int damage)
        {
            damage *= 2;
        }

        public static void CriticalStrike(ref double damage)
        {
            damage *= 2.0;
        }

        public virtual void OnUse(Mobile from)
        {
            if (!Activated && !OnCooldown)
            {
                Activated = true;
            }
        }

        public virtual int CalculateResetValue(int value)
        {
            if (Activated)
            {
                // been activated once before, so remove last levels values
                value -= Level - 1;
            }

            return value;
        }

        public virtual int CalculateNewValue(int value)
        {
            Activated = true;
            return value += Level;
        }

        public virtual bool IgnoreTalentBlock(Mobile mobile) => false;

        public virtual void ExpireTalentCooldown()
        {
            OnCooldown = false;
            Activated = false;
        }

        public static bool IsMobileType(Type[] group, Type type)
        {
            var contains = false;
            for (var j = 0; !contains && j < group.Length; ++j)
            {
                contains = group[j].IsAssignableFrom(type);
            }

            return contains;
        }

        public int GetExtraResourceCheck(int amount) => Utility.Random(100) < ModifySpellMultiplier() ? AOS.Scale(amount, Level * 2): 0;

        public virtual bool CanAffordLoss(PlayerMobile player, int amount)
        {
            Container bank = player.FindBankNoCreate();
            var validBankGold = Banker.GetBalance(player) >= amount;
            var validGold = player.Backpack?.GetAmount(typeof(Gold)) >= amount;
            return validGold || validBankGold;
        }

        public bool HasResourceQuantity(Mobile mobile, Type type, int amount)
        {
            int itemAmount = 0;
            var items = mobile.BankBox?.FindItemsByType(type);
            if (items != null)
            {
                itemAmount += items.Sum(item => item.Amount);
            }
            if (amount > 0 && itemAmount >= amount)
            {
                mobile.BankBox?.ConsumeTotal(type, amount);
            }
            return itemAmount >= amount;
        }

        public virtual void ProcessGoldGain(PlayerMobile player, int amount, bool loss, bool ignoreMod = false)
        {
            var modifiedAmount = (ignoreMod) ? amount : (int)(amount * Utility.RandomDouble());
            if (player.Backpack != null)
            {
                Container bank = player.FindBankNoCreate();
                if (loss && (player.Backpack?.ConsumeTotal(typeof(Gold), modifiedAmount) == true ||
                             Banker.Withdraw(player, modifiedAmount)))
                {
                    return;
                }

                if (!loss)
                {
                    if (modifiedAmount > 1000)
                    {
                        player.AddToBackpack(new BankCheck(modifiedAmount));
                    }
                    else if (modifiedAmount > 0)
                    {
                        player.AddToBackpack(new Gold(modifiedAmount));
                    }
                }
            }
        }

        public virtual void SlowCreature(BaseCreature creature, int duration, bool startTimerImmediately)
        {
            creature.SpeedMod = creature.CurrentSpeed*SlowModifier;
            SlowedCreatures.Add(creature);
            if (startTimerImmediately)
            {
                Timer.StartTimer(TimeSpan.FromSeconds(duration), ExpireSlowEffect);
            }
        }

        public virtual void ExpireSlowEffect()
        {
            foreach (var creature in SlowedCreatures)
            {
                creature.SpeedMod = 0;
            }
        }

        public static Point3D CalculatePushbackFromAnchor(Point3D anchorPosition, int distance, Mobile from)
        {
            var newLocation = new Point3D(anchorPosition.X, anchorPosition.Y + distance, anchorPosition.Z);
            if (from.Direction.HasFlag(Direction.East))
            {
                newLocation = new Point3D(anchorPosition.X - distance, anchorPosition.Y, anchorPosition.Z);
            } else if (from.Direction.HasFlag(Direction.West))
            {
                newLocation = new Point3D(anchorPosition.X + distance, anchorPosition.Y, anchorPosition.Z);
            } else if (from.Direction.HasFlag(Direction.South))
            {
                newLocation = new Point3D(anchorPosition.X, anchorPosition.Y - distance, anchorPosition.Z);
            }
            return newLocation;
        }

        public virtual void ResetDeityPower()
        {
        }

        public virtual void ApplyManaCost(Mobile mobile)
        {
            mobile.Mana -= ManaRequired;
            if (mobile.Mana < 0)
            {
                mobile.Mana = 0;
            }
        }

        public virtual void ApplyStaminaCost(Mobile mobile)
        {
            mobile.Stam -= StamRequired;
            if (mobile.Stam < 0)
            {
                mobile.Stam = 0;
            }
        }

        public virtual void ResetMobileMods(Mobile mobile)
        {
            foreach (var statModName in StatModNames)
            {
                mobile.RemoveStatMod(statModName);
            }
        }

        public static void ApplyFrostFireEffect(PlayerMobile from, ref int fire, ref int cold, ref int hue, Mobile target)
        {
            BaseTalent frostFire = from.GetTalent(typeof(FrostFire));
            if (frostFire != null && fire > 0) {
                ((FrostFire)frostFire).ModifyFireSpell(ref fire, ref cold, target, ref hue);
            }
        }

        public static void EmptyCreatureBackpack(BaseCreature creature)
        {
            if (creature.Backpack != null)
            {
                for (var x = creature.Backpack.Items.Count - 1; x >= 0; x--)
                {
                    var item = creature.Backpack.Items[x];
                    item.Delete();
                }
            }
        }

        public static void EmptyCorpse(Corpse corpse)
        {
            foreach (var item in corpse.Items.ToList())
            {
                Point3D point = new Point3D(
                    corpse.Location.X + Utility.RandomMinMax(-2, 2),
                    corpse.Location.Y + Utility.RandomMinMax(-2, 2),
                    corpse.Location.Z
                );
                item.MoveToWorld(point, corpse.Map);
            }
        }
        public static bool SetSkillOrder(Mobile ally, Skill skill, ref SkillName skillOrder)
        {
            if (ally.Skills[skillOrder].Base < skill.Base)
            {
                skillOrder = skill.SkillName;
                return true;
            }

            return false;
        }

        public static bool IsRangerSkill(SkillName skillToCheck)
        {
            bool isRangerSkill = false;
            SkillName[] rangerGroup = new[]
            {
                SkillName.Veterinary,
                SkillName.AnimalLore,
                SkillName.AnimalTaming
            };
            foreach (SkillName rangerSkillName in rangerGroup)
            {
                if (rangerSkillName == skillToCheck)
                {
                    isRangerSkill = true;
                    break;
                }
            }
            return isRangerSkill;
        }

        public static bool IsLoreSkill(SkillName skillToCheck)
        {
            bool isLoreSkill = false;
            var group =  SkillsGumpGroup.Groups.FirstOrDefault(group => group.Name == "Lore & Knowledge");
            if (group is not null)
            {
                foreach (SkillName loreSkill in group.Skills)
                {
                    if (loreSkill == skillToCheck)
                    {
                        isLoreSkill = true;
                        break;
                    }
                }
            }
            return isLoreSkill;
        }

        public static List<SkillName> GetPlayerSkillNames(PlayerMobile player, bool crafting, bool ranger)
        {
            List<SkillName> skillNames = new List<SkillName>();
            List<Skill> skills = new List<Skill>();
            GetTopSkills(player, ref skills, 6);
            foreach(var skill in skills)
            {
                bool isCrafting = IsCraftingSkill(skill.SkillName);
                bool isRanger = IsRangerSkill(skill.SkillName);
                if (
                    (!ranger && crafting && isCrafting)
                    || (!ranger && !crafting && !isCrafting)
                    || (ranger && isRanger))
                {
                    skillNames.Add(skill.SkillName);
                }
            }
            return skillNames;
        }

        public static bool IsCraftingSkill(SkillName skillToCheck)
        {
            bool isCraftingSkill = false;
            SkillsGumpGroup craftingGroup = SkillsGumpGroup.Groups.Where(group => group.Name == "Crafting").FirstOrDefault();
            SkillName[] harvestingGroup = new[]
            {
                SkillName.Camping,
                SkillName.Fishing,
                SkillName.Herding,
                SkillName.Lumberjacking,
                SkillName.Mining
            };
            if (craftingGroup is not null)
            {
                SkillName[] combinedSkills = craftingGroup.Skills.Concat(harvestingGroup).ToArray();
                foreach (SkillName craftingSkillName in combinedSkills)
                {
                    if (craftingSkillName == skillToCheck)
                    {
                        isCraftingSkill = true;
                        break;
                    }
                }
            }

            return isCraftingSkill;
        }


        public static void GetTopSkills(Mobile target, ref List<Skill> skills, int amount)
        {
            foreach (var skill in target.Skills)
            {
                var targetSkill = target.Skills[skill.SkillName];
                if (targetSkill.Base > 10.0)
                {
                    var lesser = skills.Where(s => s.Base < targetSkill.Base).Min();
                    if (lesser is not null && skills.Count > amount)
                    {
                        var index = skills.FindIndex(0, s => s.SkillName == lesser.SkillName);
                        skills[index] = targetSkill;
                    }
                    else
                    {
                        skills.Add(skill);
                    }
                }
            }
        }
        public static List<DefaultSkillMod> GetTopDefaultSkillMods(List<Skill> skills, double modifier)
        {
            List<DefaultSkillMod> skillMods = new List<DefaultSkillMod>();
            foreach (var skill in skills)
            {
                skillMods.Add(new DefaultSkillMod(skill.SkillName, true, modifier));
            }

            return skillMods;
        }

        public static void CheckSetTalentEffect(Mobile ally, BaseTalent? baseTalent)
        {
            if (ally is PlayerMobile playerMobile)
            {
                playerMobile.TalentEffect = baseTalent;
            } else if (ally is BaseCreature baseCreature)
            {
                baseCreature.TalentEffect = baseTalent;
            }
        }

        public void AlterDamage(Mobile to, PlayerMobile player, ref int amount)
        {
            if (to is BaseCreature creature)
            {
                creature.AlterDamageFromByLevel(player, ref amount);
            }
        }

        public void AlterDamage(Mobile to, PlayerMobile player, ref double amount)
        {
            if (to is BaseCreature creature)
            {
                creature.AlterDamageFromByLevel(player, ref amount);
            }
        }
    }
}
