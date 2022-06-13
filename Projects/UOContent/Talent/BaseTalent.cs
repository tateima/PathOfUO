using System;
using System.Linq;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Talent
{
    public class BaseTalent : ITalent
    {
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
            typeof(Concussion),
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
            typeof(MultiShot), // up to here test
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
            typeof(Runebinding)
        };

        public TimerExecutionToken _talentTimerToken;


        public BaseTalent()
        {
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
            RequiredWeapon = new[] { typeof(BaseWeapon) };
            RequiredWeaponSkill = SkillName.Alchemy;
            RequiredSpell = Array.Empty<Type>();
            IncreaseHitChance = false;
            IncreaseParryChance = false;
            TalentDependencyPoints = 3;
            Level = 0;
            MaxLevel = 5;
            DisplayName = "Basic Talent";
            Description = "Does something.";
            ImageID = 30145;
            AddEndY = 110;
            GumpHeight = 200;
        }

        public virtual double SpecialDamageScalar => Core.AOS ? 0.16 : 0.05;

        public bool UpdateOnEquip { get; set; }

        public bool HasBeforeDeathSave { get; set; }

        public bool HasKillEffect { get; set; }

        public bool HasDefenseEffect { get; set; }

        public bool HasDamageAbsorptionEffect { get; set; }

        public bool HasDeathEffect { get; set; }

        public bool CanBeUsed { get; set; }

        public bool Activated { get; set; }

        public bool UpgradeCost { get; set; }

        public bool OnCooldown { get; set; }

        public int TalentDependencyPoints { get; set; }

        public int GumpHeight { get; set; }

        public bool IncreaseParryChance { get; set; }

        public bool IncreaseHitChance { get; set; }

        public SkillName RequiredWeaponSkill { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public int MobilePercentagePerPoint { get; set; }

        public Type[] RequiredWeapon { get; set; }

        public Type[] RequiredSpell { get; set; }

        public ResistanceMod ResMod { get; set; }

        public int Level { get; set; }

        public int AddEndY { get; set; }

        public Type[] BlockedBy { get; set; }

        public int MaxLevel { get; set; }

        public Type TalentDependency { get; set; }

        public int ImageID { get; set; }

        public virtual void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
        }


        public virtual void UpdateMobile(Mobile mobile)
        {
        }

        public virtual int ModifySpellMultiplier() => Level;

        public virtual bool HasSkillRequirement(Mobile mobile) => true;
        public virtual bool HasUpgradeRequirement(Mobile mobile) => true;

        public virtual bool CanApplyHitEffect(Item i)
        {
            var debug = i.GetType().IsSubclassOf(typeof(BaseWeapon));
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

        public virtual void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
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


        public void CriticalStrike(Mobile mobile, Mobile target, int damage)
        {
            target.Damage(damage / 2, mobile);
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
    }
}
