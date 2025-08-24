using System;
using System.Collections.Generic;
using System.Linq;
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
        public static string FailedRequirements = "You do not meet the requirements to use this talent";
        public const int InvalidTalentIndex = 999;

        public static readonly Type[] TalentTypes =
        {
            typeof(ViperAspect),
            typeof(VenomBlood),
            typeof(WyvernAspect),
            typeof(GreaterPoisonElemental),
            typeof(PoisonNova),
            typeof(FireAffinity),
            typeof(DragonAspect),
            typeof(Warmth),
            typeof(GreaterFireElemental),
            typeof(Firewalker),
            typeof(DarkAffinity),
            typeof(SummonerCommand),
            typeof(SpectralScream),
            typeof(MasterOfDeath),
            typeof(DeathKnightForm),
            typeof(LightAffinity),
            typeof(GuardianLight),
            typeof(HolyAvenger),
            typeof(Reckoning),
            typeof(HolyBolt),
            typeof(SonicAffinity),
            typeof(Resonance),
            typeof(MassConfusion),
            typeof(DominateCreature),
            typeof(EternalChord),
            typeof(NatureAffinity),
            typeof(RangerCommand),
            typeof(BondingMaster),
            typeof(PackLeader),
            typeof(Cannibalism),
            typeof(ExoticTamer),
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
            typeof(WeaponMaster),
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
            typeof(LoreMaster),
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
            IgnoreRequirements = false;
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
            HasMovementEffect = false;
            HasDamageAbsorptionEffect = false;
            HasDefenseEffect = false;
            HasDeathEffect = false;
            HasKillEffect = false;
            HasGroupKillEffect = false;
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
            TalentDependencyMinimum = 1;
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

        public bool IgnoreRequirements { get; set; }

        public bool HasBeforeDeathSave { get; set; }

        public bool HasKillEffect { get; set; }

        public bool HasGroupKillEffect { get; set; }

        public bool HasDefenseEffect { get; set; }

        public bool CanAbsorbSpells { get; set; }

        public bool HasMovementEffect { get; set; }

        public bool HasDamageAbsorptionEffect { get; set; }

        public bool HasDeathEffect { get; set; }

        public bool CanBeUsed { get; set; }

        public bool Activated { get; set; }

        public bool UpgradeCost { get; set; }

        private bool _onCooldown;

        public PlayerMobile User { get; set; }

        public bool OnCooldown
        {
            get => _onCooldown;
            set
            {
                _onCooldown = value;
                if (User is not null)
                {
                    User.CloseGump<TalentBarGump>();
                    User.SendGump(new TalentBarGump(User, 1, 0, new List<TalentGump.TalentGumpPage>()));
                }
            }
        }

        public int ManaRequired { get; set; }

        public int StamRequired { get; set; }

        public Deity.Alignment DeityAlignment { get; set; }

        public int TalentDependencyPoints { get; set; }

        public int TalentDependencyMinimum { get; set; }

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

        public Type[] TalentDependencies { get; set; }

        public int ImageID { get; set; }

        public virtual void CheckMovementEffect(Mobile from)
        {
        }

        public virtual void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
        }

        public virtual void UpdateMobile(Mobile mobile)
        {
        }

        public virtual int ModifySpellMultiplier() => Level;

        public virtual bool HasSkillRequirement(Mobile mobile) => true;
        public virtual bool HasUpgradeRequirement(Mobile mobile) => true;

        public bool HasAllDependencies(List<BaseTalent[]> dependencyMatrices)
        {
            if (dependencyMatrices is not null && dependencyMatrices.Count > 0)
            {
                bool hasEnoughDependencies = false;
                int numberOfDependenciesSatisfied = 0;
                foreach (var dependencyMatrix in dependencyMatrices)
                {
                    BaseTalent dependsOn = dependencyMatrix.Length > 0 ? dependencyMatrix[0] : null;
                    BaseTalent hasDependency = dependencyMatrix.Length > 1 ? dependencyMatrix[1] : null;
                    var dependencyMet = dependsOn is not null && hasDependency is not null &&
                                        (hasDependency.Level >= TalentDependencyPoints ||
                                         hasDependency.Level == hasDependency.MaxLevel);
                    if (dependencyMet)
                    {
                        numberOfDependenciesSatisfied++;
                        hasEnoughDependencies = numberOfDependenciesSatisfied >= TalentDependencyMinimum;
                        if (hasEnoughDependencies)
                        {
                            break;
                        }
                    }
                }

                return hasEnoughDependencies;
            }

            return true;
        }

        public static List<BaseTalent[]> GetTalentDependencies(PlayerMobile player, BaseTalent talent)
        {
            List<BaseTalent[]> dependencies = new List<BaseTalent[]>();
            if (talent.TalentDependencies is not null)
            {
                foreach (var dependencyType in talent.TalentDependencies)
                {
                    BaseTalent dependsOn = TalentConstructor.Construct(dependencyType) as BaseTalent;
                    if (dependsOn is not null)
                    {
                        BaseTalent hasDependency = player.GetTalent(dependsOn.GetType());
                        dependencies.Add(
                            new[]
                            {
                                dependsOn,
                                hasDependency
                            }
                        );
                    }
                }
            }

            return dependencies;
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

        public virtual void CheckGroupKillEffect(Mobile victim, Mobile killer)
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

        public static int GetCriticalStrikeDice() => Utility.RandomMinMax(1, 5);

        public static void CriticallyWeaken(Mobile target, StatType statType)
        {
            target.AddStatMod(
                new StatMod(statType, "CriticalStrikeWeakness", Utility.RandomMinMax(1, 3), TimeSpan.FromSeconds(30))
            );
            target.PrivateOverheadMessage(
                MessageType.Regular,
                target.SpeechHue,
                true,
                "You are critically hit, causing temporary weakness.",
                target.NetState
            );
        }

        public static void CriticalStrike(Mobile target, Mobile from, ref double damage)
        {
            if (Utility.Random(10) < 4)
            {
                damage *= 2.0;
            }
            else if (target is PlayerMobile player)
            {
                ApplyCriticalStrikeEffect(player, from);
            }
            else if (target is BaseCreature creature)
            {
                ApplyCriticalStrikeEffect(creature, from);
            }
        }

        public static void CriticalStrike(Mobile target, Mobile from, ref int damage)
        {
            if (Utility.Random(10) < 4)
            {
                damage *= 2;
            }
            else
            {
                if (target is PlayerMobile player)
                {
                    ApplyCriticalStrikeEffect(player, from);
                }
                else if (target is BaseCreature creature)
                {
                    target.PublicOverheadMessage(
                        MessageType.Regular,
                        target.SpeechHue,
                        false,
                        $"* The {creature.Name} suffers a critical strike effect *"
                    );
                    ApplyCriticalStrikeEffect(creature, from);
                }
            }
        }

        public static void ApplyCriticalStrikeEffect(PlayerMobile player, Mobile from)
        {
            switch (GetCriticalStrikeDice())
            {
                case 1:
                    {
                        Blindness.BlindMobile(player); // head
                        break;
                    }
                case 2:
                    {
                        BleedAttack.BeginBleed(player, from); // torso
                        break;
                    }
                case 3:
                    {
                        Disarm.BeginDisarm(from, player); // torso
                        break;
                    }
                case 4:
                    {
                        CriticallyWeaken(player, StatType.Str); // right arm
                        break;
                    }
                default:
                    {
                        CriticallyWeaken(player, StatType.Dex);
                        break;
                    }
            }

            ;

        }

        public static void ApplyCriticalStrikeEffect(BaseCreature creature, Mobile from)
        {
            switch (GetCriticalStrikeDice())
            {
                case 1:
                    {
                        Blindness.BlindMobile(creature); // head
                        break;
                    }
                case 2:
                    {
                        BleedAttack.BeginBleed(creature, from); // torso
                        break;
                    }
                case 3:
                    {
                        Disarm.BeginDisarm(from, creature); // torso
                        break;
                    }
                case 4:
                    {
                        CriticallyWeaken(creature, StatType.Str); // right arm
                        break;
                    }
                default:
                    {
                        CriticallyWeaken(creature, StatType.Dex);
                        break;
                    }
            }

            ;

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

        public int GetExtraResourceCheck(int amount) =>
            Utility.Random(100) < ModifySpellMultiplier() ? AOS.Scale(amount, Level * 2) : 0;

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
            var items = mobile.BankBox.FindItemsByType(type);
            while (items.MoveNext())
            {
                itemAmount += items.Current.Amount;
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
            creature.MoveSpeedMod = creature.CurrentSpeed * SlowModifier;
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
                creature.MoveSpeedMod = 0;
            }
        }

        public static Point3D CalculatePushbackFromAnchor(Point3D anchorPosition, int distance, Mobile from)
        {
            var newLocation = new Point3D(anchorPosition.X, anchorPosition.Y + distance, anchorPosition.Z);
            if (from.Direction.HasFlag(Direction.East))
            {
                newLocation = new Point3D(anchorPosition.X - distance, anchorPosition.Y, anchorPosition.Z);
            }
            else if (from.Direction.HasFlag(Direction.West))
            {
                newLocation = new Point3D(anchorPosition.X + distance, anchorPosition.Y, anchorPosition.Z);
            }
            else if (from.Direction.HasFlag(Direction.South))
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
            if (frostFire != null && fire > 0)
            {
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
            var group = SkillsGumpGroup.Groups.FirstOrDefault(group => group.Name == "Lore & Knowledge");
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

        public static BaseTalent RandomTalent() =>
            TalentConstructor.Construct(TalentTypes[Utility.Random(TalentTypes.Length)]) as BaseTalent;

        public static List<SkillName> GetPlayerSkillNames(PlayerMobile player, bool crafting, bool ranger)
        {
            List<SkillName> skillNames = new List<SkillName>();
            List<Skill> skills = new List<Skill>();
            GetTopSkills(player, ref skills, 6);
            foreach (var skill in skills)
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
                    var lesser = skills.MinBy(s => s.Base);
                    if (skills.Count > amount)
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

        public static List<DefaultSkillMod> GetTopDefaultSkillMods(List<Skill> skills, double modifier, string name)
        {
            List<DefaultSkillMod> skillMods = new List<DefaultSkillMod>();
            int affix = 0;
            foreach (var skill in skills)
            {
                skillMods.Add(new DefaultSkillMod(skill.SkillName, name + affix, true, modifier));
                affix++;
            }

            return skillMods;
        }

        public static void CheckSetTalentEffect(Mobile ally, BaseTalent? baseTalent)
        {
            if (ally is PlayerMobile playerMobile)
            {
                playerMobile.TalentEffect = baseTalent;
            }
            else if (ally is BaseCreature baseCreature)
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

        public static bool IsWeaponMaster(Mobile attacker) => attacker is PlayerMobile playerMobile &&
                                                              playerMobile.GetTalent(typeof(WeaponMaster)) is not null;

        public static int WeaponMasterModifier(Mobile attacker)
        {
            if (attacker is PlayerMobile playerMobile)
            {
                if (playerMobile.GetTalent(typeof(WeaponMaster)) is WeaponMaster weaponMaster)
                {
                    return weaponMaster.Level * 10;
                }
            }

            return 0;
        }

        public static Type[] ExoticTames =
        {
            typeof(ManaDrake)
        };

        public static int SummonerCommandLevel(Mobile mobile)
        {
            var summonerCommandLevel = 0;
            if (mobile is PlayerMobile playerMobile)
            {
                var summonerCommand = playerMobile.GetTalent(typeof(SummonerCommand));
                if (summonerCommand != null)
                {
                    summonerCommandLevel = summonerCommand.Level;
                }
            }

            return summonerCommandLevel;
        }
    }
}

