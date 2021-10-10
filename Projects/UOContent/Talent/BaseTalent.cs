using Server.Items;
using Server.Spells;
using Server.Gumps;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Talent
{
    public class BaseTalent : ITalent
    {
        public virtual double SpecialDamageScalar => Core.AOS ? 0.16 : 0.05;

        public TimerExecutionToken _talentTimerToken;

        private bool m_HasBeforeDeathSave;
        public bool HasBeforeDeathSave
        {
            get { return m_HasBeforeDeathSave; }
            set { m_HasBeforeDeathSave = value; }
        }

        private bool m_HasKillEffect;
        public bool HasKillEffect
        {
            get { return m_HasKillEffect; }
            set { m_HasKillEffect = value; }
        }

        private bool m_HasDefenseEffect;
        public bool HasDefenseEffect
        {
            get { return m_HasDefenseEffect; }
            set { m_HasDefenseEffect = value; }
        }


        private bool m_HasDeathEffect;
        public bool HasDeathEffect
        {
            get { return m_HasDeathEffect; }
            set { m_HasDeathEffect = value; }
        }

        private bool m_CanBeUsed;
        public bool CanBeUsed
        {
            get { return m_CanBeUsed; }
            set { m_CanBeUsed = value; }
        }

        private bool m_Activated;
        public bool Activated
        {
            get { return m_Activated; }
            set { m_Activated = value; }
        }
        private bool m_OnCooldown;
        public bool OnCooldown
        {
            get { return m_OnCooldown; }
            set { m_OnCooldown = value; }
        }

        private int m_TalentDependencyPoints;
        public int TalentDependencyPoints
        {
            get { return m_TalentDependencyPoints; }
            set { m_TalentDependencyPoints = value; }
        }
        private int m_GumpHeight;
        public int GumpHeight
        {
            get { return m_GumpHeight; }
            set { m_GumpHeight = value; }
        }
        private int m_AddEndY;
        public int AddEndY
        {
            get { return m_AddEndY; }
            set { m_AddEndY = value;  }
        }
        private bool m_IncreaseParryChance;
        public bool IncreaseParryChance
        {
            get { return m_IncreaseParryChance; }
            set { m_IncreaseParryChance = value; }
        }
        private bool m_IncreaseHitChance;
        public bool IncreaseHitChance
        {
            get { return m_IncreaseHitChance; }
            set { m_IncreaseHitChance = value; }
        }
        private Type[] m_BlockTalents;
        public Type[] BlockedBy
        {
            get { return m_BlockTalents; }
            set { m_BlockTalents = value; }
        }
        private Type m_TalentDependency;
        private Type[] m_RequiredWeapon;
        private SkillName m_RequiredWeaponSkill;
        public SkillName RequiredWeaponSkill
        {
            get { return m_RequiredWeaponSkill; }
            set { m_RequiredWeaponSkill = value; }
        }

        private Type[] m_RequiredSpell;

        private string m_Description;
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        private string m_DisplayName;
        public string DisplayName
        {
            get { return m_DisplayName; }
            set { m_DisplayName = value; }
        }
        private int m_MaxLevel;
        public int MaxLevel
        {
            get { return m_MaxLevel; }
            set { m_MaxLevel = value; }
        }

        private int m_MobilePercentagePerPoint;

        public int MobilePercentagePerPoint
        {
            get { return m_MobilePercentagePerPoint; }
            set { m_MobilePercentagePerPoint = value; }
        }

        public Type TalentDependency
        {
            get { return m_TalentDependency; }
            set { m_TalentDependency = value; }
        }

        public Type[] RequiredWeapon
        {
            get { return m_RequiredWeapon; }
            set { m_RequiredWeapon = value; }
        }
        public Type[] RequiredSpell
        {
            get { return m_RequiredSpell; }
            set { m_RequiredSpell = value; }
        }
        private int m_Level;
        private ResistanceMod m_ResistanceMod;

        public ResistanceMod ResMod
        {
            get { return m_ResistanceMod; }
            set { m_ResistanceMod = value; }
        }
        public static readonly Type[] TalentTypes =
        {
            typeof(ViperAspect),
            typeof(VenomBlood),
            typeof(WyvernAspect),
            typeof(FireAffinity),
            typeof(DragonAspect),
            typeof(GreaterFireElemental),
            typeof(DarkAffinity),
            typeof(SummonerCommand),
            typeof(MasterOfDeath),
            typeof(LightAffinity),
            typeof(GuardianLight),
            typeof(HolyAvenger),
            typeof(SonicAffinity),
            typeof(Resonance),
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
            typeof(KeenSenses),
            typeof(EscapeDeath),
            typeof(HandFinesse),
            typeof(DivineIntellect),
            typeof(FastLearner),
            typeof(ManaShield),
            typeof(PlanarShift),
            typeof(SpellStrike),
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
            typeof(MacefightingFocus),
            typeof(MageCombatant),
            typeof(MaceSpecialist),
            typeof(Concussion),
            typeof(TwoHandedMaceSpecialist),
            typeof(GroundSlam),
            typeof(ShieldFocus),
            typeof(ShieldBash),
            typeof(Phalanx),
            typeof(SpellWard),
            typeof(ArcherFocus),
            typeof(ChemicalWarfare),
            typeof(Disengage),
            typeof(CrossbowSpecialist),
            typeof(IceBolt),
            typeof(BowSpecialist),
            typeof(MultiShot),
            typeof(CarefulShooter),
            typeof(LoreSeeker),
            typeof(LoreTeacher),
            typeof(LoreDisciples),
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
            typeof(EfficientCarver),
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
        };

        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        private int m_ImageID;
        public int ImageID
        {
            get { return m_ImageID; }
            set { m_ImageID = value; }
        }

        public BaseTalent()
        {
            m_Activated = false;
            m_OnCooldown = false;
            m_CanBeUsed = false;
            m_HasDefenseEffect = false;
            m_HasDeathEffect = false;
            m_HasKillEffect = false;
            m_HasBeforeDeathSave = false;
            m_MobilePercentagePerPoint = 3;
            m_BlockTalents = new Type[] { };
            m_RequiredWeapon = new Type[] { typeof(BaseWeapon) };
            m_RequiredWeaponSkill = SkillName.Alchemy;
            m_RequiredSpell = Array.Empty<Type>();
            m_IncreaseHitChance = false;
            m_IncreaseParryChance = false;
            m_TalentDependencyPoints = 3;
            m_Level = 0;
            m_MaxLevel = 5;
            m_DisplayName = "Basic Talent";
            m_Description = "Does something.";
            m_ImageID = 30145;
            m_AddEndY = 110;
            m_GumpHeight = 200;
        }

        public virtual bool CanApplyHitEffect(Item i)
        {
            bool debug = i.GetType().IsSubclassOf(typeof(BaseWeapon));
            Type valid = RequiredWeapon.Where(w => w == i.GetType() || i.GetType().IsSubclassOf(w)).FirstOrDefault();

            bool validSkill = false;
            if (i is BaseWeapon bw)
            {
                validSkill = (RequiredWeaponSkill == bw.DefSkill);
            }
            
            return valid != null || validSkill;
        }

        public virtual void CheckHitEffect(Mobile a, Mobile t, int d)
        {
            return;
        }

        public virtual void CheckMissEffect(Mobile a, Mobile t)
        {
            return;
        }

        public virtual void CheckDefenderMissEffect(Mobile a, Mobile t)
        {
            return;
        }


        public virtual void UpdateMobile(Mobile m)
        {
            return;
        }

        public virtual int GetHitChanceModifier()
        {
            return Level;
        }

        public virtual bool CanScaleSpellDamage(Spell spell)
        {
            Type valid = RequiredSpell.Where(w => w == spell.GetType() || spell.GetType().IsSubclassOf(w)).FirstOrDefault();
            return valid != null;
        }

        public virtual int ModifySpellMultiplier()
        {
            return Level;
        }
        public virtual double ModifySpellScalar()
        {
            return 0.0;
        }
        public virtual void CheckSpellEffect(Mobile m, Mobile t)
        {
            return;
        }
        public virtual bool HasSkillRequirement(Mobile mobile) {
            return true;
        }
        public virtual void CheckDefenseEffect(Mobile m, Mobile t, int d)
        {
            return;
        }

        public virtual void CheckDeathEffect(Mobile t)
        {
            return;
        }

        public virtual void CheckKillEffect(Mobile v, Mobile k)
        {
            return;
        }

        public virtual void CheckBeforeDeathEffect(Mobile t)
        {
            return;
        }

        public virtual Mobile ScaleMobile(Mobile m)
        {
            m = ScaleMobileStats(m);
            m = ScaleMobileSkills(m, "Magical");
            m = ScaleMobileSkills(m, "Combat Ratings");
            m = ScaleMobileSkills(m, "Lore & Knowledge");
            return m;
        }

        public virtual Mobile ScaleMobileStats(Mobile m)
        {
            // default affinity scaling is 3% per level
            int modifier = (Level * MobilePercentagePerPoint);
            m.RawDex += AOS.Scale(m.RawDex, modifier);
            m.RawInt += AOS.Scale(m.RawInt, modifier);
            m.RawStr += AOS.Scale(m.RawStr, modifier);
            return m;
        }

        public virtual Mobile ScaleMobileSkills(Mobile m, string skillGroupName)
        {
            SkillsGumpGroup group = SkillsGumpGroup.Groups.Where(group => group.Name == skillGroupName).FirstOrDefault();
            if (group != null)
            {
                foreach (SkillName skill in group.Skills)
                {
                    m.Skills[skill].Base += (Level * MobilePercentagePerPoint);
                }
            }
            return m;
        }



        public void CriticalStrike(Mobile m, Mobile t, int d)
        {
            // double damage crit so damage them with half total damage again
            t.Damage((int)(d / 2), m);
        }


        public virtual void OnUse(Mobile m)
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

        public virtual bool IgnoreTalentBlock(Mobile mobile)
        {
            return false;
        }

        public virtual void ExpireTalentCooldown()
        {
            OnCooldown = false;
            Activated = false;
        }
        public bool IsMobileType(Type[] group, Type type)
        {
            var contains = false;
            for (var j = 0; !contains && j < group.Length; ++j)
            {
                contains = group[j].IsAssignableFrom(type);
            }
            return contains;
        }
        public virtual int GetExtraResourceCheck()
        {
            if (Utility.RandomDouble() < Level / 20)
            { // 0.5% per point
                return Level;
            }
            return 0;
        }
        public int GoldAmount(int amount)
        {
            return (int)(amount * Utility.RandomDouble());
        }

        public virtual bool CanAffordLoss(PlayerMobile player, int amount)
        {
            Container bank = player.FindBankNoCreate();
            bool validBankGold = Banker.GetBalance(player) >= amount;
            bool validGold = player.Backpack?.GetAmount(typeof(Gold)) >= amount;
            return (validGold || validBankGold);
        }

        public virtual void ProcessGoldGain(PlayerMobile player, int amount, bool loss)
        {
            int modifiedAmount = (int)(amount * Utility.RandomDouble());
            if (player.Backpack != null)
            {
                Container bank = player.FindBankNoCreate();
                if (loss && (player.Backpack?.ConsumeTotal(typeof(Gold), modifiedAmount) == true || Banker.Withdraw(player, modifiedAmount)))
                {
                    return;
                } else if (!loss) {
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
            return;
        }
    }
}
