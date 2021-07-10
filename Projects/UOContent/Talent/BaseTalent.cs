using Server.Spells;
using System;

namespace Server.Talent
{
    public class BaseTalent : ITalent
    {
        private int m_AddY;
        public int AddY
        {
            get { return m_AddY; }
            set { m_AddY = value;  }
        }
        private Type[] m_BlockTalents;
        public Type[] BlockedBy
        {
            get { return m_BlockTalents; }
            set { m_BlockTalents = value; }
        }
        private Type m_TalentDependency;
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

        public Type TalentDependency
        {
            get { return m_TalentDependency; }
            set { m_TalentDependency = value; }
        }

        private int m_Level;
        private ResistanceMod m_ResistanceMod;

        public ResistanceMod ReistanceMod
        {
            get { return m_ResistanceMod; }
            set { m_ResistanceMod = value; }
        }
        public static readonly Type[] TalentTypes =
        {
            typeof(ViperAspect),
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
            typeof(MindMatter),
            typeof(PlanarShift),
            typeof(SpellMind),
            typeof(SwordsmanshipFocus),
            typeof(PolearmSpecialist),
            typeof(SwordSpecialist),
            typeof(AxeSpecialist),
            typeof(FencingFocus),
            typeof(SpearSpecialist),
            typeof(FencingSpecialist),
            typeof(Riposte),
            typeof(MacefightingFocus),
            typeof(MageCombatant),
            typeof(MaceSpecialist),
            typeof(TwoHandedMaceSpecialist),
            typeof(ShieldFocus),
            typeof(ShieldBash),
            typeof(ArcherFocus),
            typeof(CrossbowSpecialist),
            typeof(BowSpecialist),
            typeof(CarefulShooter),
            typeof(LoreSeeker),
            typeof(LoreTeacher),
            typeof(LoreDisciples),
            typeof(CraftFocus),
            typeof(ResourcefulCrafter),
            typeof(OptimisedConsumption),
            typeof(TycoonCrafter),
            typeof(StrongTools)
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
            m_Level = 0;
            m_MaxLevel = 5;
            m_DisplayName = "Basic Talent";
            m_Description = "Does something.";
            m_ImageID = 30145;
            m_AddY = 110;
        }

        public void CheckHitEffect(Mobile m, Mobile t)
        {
            return;
        }

        public void UpdateMobile(Mobile m)
        {
            return;
        }

        public void CheckSpellEffect(Spell spell)
        {
            return;
        }
        public virtual bool HasSkillRequirement(Mobile mobile) {
            return true;
        }

        public void CheckDefenseEffect(Mobile m, Mobile t, int d)
        {
            return;
        }
        public bool IsMobileType(Mobile mobile, Type[] group, Type type)
        {
            var contains = false;
            for (var j = 0; !contains && j < group.Length; ++j)
            {
                contains = group[j].IsAssignableFrom(type);
            }
            return contains;
        }

    }
}
