using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using Server.Collections;
using Server.ContextMenus;
using Server.Engines.ConPVP;
using Server.Engines.MLQuests;
using Server.Engines.Quests.Doom;
using Server.Engines.Quests.Haven;
using Server.Engines.Spawners;
using Server.Ethics;
using Server.Factions;
using Server.Items;
using Server.Misc;
using Server.Multis;
using Server.Network;
using Server.Pantheon;
using Server.Regions;
using Server.SkillHandlers;
using Server.Spells;
using Server.Spells.Bushido;
using Server.Spells.Necromancy;
using Server.Spells.Sixth;
using Server.Spells.Spellweaving;
using Server.Talent;
using Server.Targeting;

namespace Server.Mobiles
{
    /// <summary>
    ///     Summary description for MobileAI.
    /// </summary>
    public enum FightMode
    {
        None,      // Never focus on others
        Aggressor, // Only attack aggressors
        Strongest, // Attack the strongest
        Weakest,   // Attack the weakest
        Closest,   // Attack the closest
        Evil       // Only attack aggressor -or- negative karma
    }

    public enum OrderType
    {
        None,   // When no order, let's roam
        Come,   // "(All/Name) come"  Summons all or one pet to your location.
        Drop,   // "(Name) drop"  Drops its loot to the ground (if it carries any).
        Follow, // "(Name) follow"  Follows targeted being.

        // "(All/Name) follow me"  Makes all or one pet follow you.
        Friend,   // "(Name) friend"  Allows targeted player to confirm resurrection.
        Unfriend, // Remove a friend
        Guard,    // "(Name) guard"  Makes the specified pet guard you. Pets can only guard their owner.

        // "(All/Name) guard me"  Makes all or one pet guard you.
        Attack, // "(All/Name) kill",

        // "(All/Name) attack"  All or the specified pet(s) currently under your control attack the target.
        Patrol,  // "(Name) patrol"  Roves between two or more guarded targets.
        Release, // "(Name) release"  Releases pet back into the wild (removes "tame" status).
        Stay,    // "(All/Name) stay" All or the specified pet(s) will stop and stay in current spot.
        Stop,    // "(All/Name) stop Cancels any current orders to attack, guard or follow.
        Transfer // "(Name) transfer" Transfers complete ownership to targeted player.
    }

    [Flags]
    public enum FoodType
    {
        None = 0x0000,
        Meat = 0x0001,
        FruitsAndVegies = 0x0002,
        GrainsAndHay = 0x0004,
        Fish = 0x0008,
        Eggs = 0x0010,
        Gold = 0x0020
    }

    [Flags]
    public enum PackInstinct
    {
        None = 0x0000,
        Canine = 0x0001,
        Ostard = 0x0002,
        Feline = 0x0004,
        Arachnid = 0x0008,
        Daemon = 0x0010,
        Bear = 0x0020,
        Equine = 0x0040,
        Bull = 0x0080,
        Reptile = 0x0090
    }

    public enum ScaleType
    {
        Red,
        Yellow,
        Black,
        Green,
        White,
        Blue,
        All
    }

    public enum MeatType
    {
        Ribs,
        Bird,
        LambLeg
    }

    public enum HideType
    {
        Regular,
        Spined,
        Horned,
        Barbed
    }

    public class DamageStore : IComparable<DamageStore>
    {
        public int m_Damage;
        public bool m_HasRight;
        public Mobile m_Mobile;

        public DamageStore(Mobile m, int damage)
        {
            m_Mobile = m;
            m_Damage = damage;
        }

        public int CompareTo(DamageStore ds) => (ds?.m_Damage ?? 0).CompareTo(m_Damage);
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class FriendlyNameAttribute : Attribute
    {
        public FriendlyNameAttribute(TextDefinition friendlyName) => FriendlyName = friendlyName;
        // future use: Talisman 'Protection/Bonus vs. Specific Creature

        public TextDefinition FriendlyName { get; }

        public static TextDefinition GetFriendlyNameFor(Type t)
        {
            if (t.IsDefined(typeof(FriendlyNameAttribute), false))
            {
                var objs = t.GetCustomAttributes(typeof(FriendlyNameAttribute), false);

                if (objs.Length > 0)
                {
                    return (objs[0] as FriendlyNameAttribute)?.FriendlyName ?? "";
                }
            }

            return t.Name;
        }
    }

    public abstract class BaseCreature : Mobile, IHonorTarget, IQuestGiver
    {
        public enum Allegiance
        {
            None,
            Ally,
            Enemy
        }

        public enum TeachResult
        {
            Success,
            Failure,
            KnowsMoreThanMe,
            KnowsWhatIKnow,
            SkillNotRaisable,
            NotEnoughFreePoints
        }

        public const int MaxLoyalty = 100;

        public const int MaxOwners = 5;

        public const int DefaultRangePerception = 16;
        public const int OldRangePerception = 10;

        private const double ChanceToRummage = 0.5; // 50%

        private const double MinutesToNextRummageMin = 1.0;
        private const double MinutesToNextRummageMax = 4.0;

        private const double MinutesToNextChanceMin = 0.25;
        private const double MinutesToNextChanceMax = 0.75;

        public const int ShoutRange = 8;

        private static readonly Type[] m_AnimateDeadTypes =
        {
            typeof(MoundOfMaggots), typeof(HellSteed), typeof(SkeletalMount),
            typeof(WailingBanshee), typeof(Wraith), typeof(SkeletalDragon),
            typeof(LichLord), typeof(FleshGolem), typeof(Lich),
            typeof(SkeletalKnight), typeof(BoneKnight), typeof(Mummy),
            typeof(SkeletalMage), typeof(BoneMagi), typeof(PatchworkSkeleton)
        };

        private static Mobile m_NoDupeGuards;

        private static readonly bool EnableRummaging = true;
        public static readonly TimeSpan ShoutDelay = TimeSpan.FromMinutes(1);

        private DateTime m_NextGambleTime;
        public DateTime NextGambleTime
        {
            get => m_NextGambleTime;
            set => m_NextGambleTime = value;
        }
        private int m_GambleLosses;
        public int GambleLosses
        {
            get => m_GambleLosses;
            set => m_GambleLosses = value;
        }

        private int m_CannibalPoints;

        public int CannibalPoints
        {
            get
            {
                return m_CannibalPoints;
            }
            set
            {
                m_CannibalPoints = value;
            }
        }

        private static readonly Type[] m_Eggs =
        {
            typeof(FriedEggs), typeof(Eggs)
        };

        private static readonly Type[] m_Fish =
        {
            typeof(FishSteak), typeof(RawFishSteak)
        };

        private static readonly Type[] m_GrainsAndHay =
        {
            typeof(BreadLoaf), typeof(FrenchBread), typeof(SheafOfHay)
        };

        private static readonly Type[] m_Meat =
        {
            /* Cooked */
            typeof(Bacon), typeof(CookedBird), typeof(Sausage),
            typeof(Ham), typeof(Ribs), typeof(LambLeg),
            typeof(ChickenLeg),

            /* Uncooked */
            typeof(RawBird), typeof(RawRibs), typeof(RawLambLeg),
            typeof(RawChickenLeg),

            /* Body Parts */
            typeof(Head), typeof(LeftArm), typeof(LeftLeg),
            typeof(Torso), typeof(RightArm), typeof(RightLeg)
        };

        private static readonly Type[] m_FruitsAndVegies =
        {
            typeof(HoneydewMelon), typeof(YellowGourd), typeof(GreenGourd),
            typeof(Banana), typeof(Bananas), typeof(Lemon), typeof(Lime),
            typeof(Dates), typeof(Grapes), typeof(Peach), typeof(Pear),
            typeof(Apple), typeof(Watermelon), typeof(Squash),
            typeof(Cantaloupe), typeof(Carrot), typeof(Cabbage),
            typeof(Onion), typeof(Lettuce), typeof(Pumpkin)
        };

        private static readonly Type[] m_Gold =
        {
            // white wyrms eat gold..
            typeof(Gold)
        };

        private bool _summoned;
        private bool m_Blinded;
        private bool m_Feared;

        private bool m_bTamable;
        private int m_ColdResistance;

        private bool m_Controlled;        // Is controlled
        private Mobile m_ControlMaster;   // My master
        private OrderType m_ControlOrder; // My order

        private AIType m_CurrentAI; // The current AI

        private double m_CurrentSpeed; // The current speed, lets say it could be changed by something;
        private int m_DamageMax = -1;

        private int m_DamageMin = -1;
        private AIType m_DefaultAI; // The default AI

        private DeleteTimer m_DeleteTimer;
        private int m_EnergyResistance;

        private int m_FailedReturnHome; /* return to home failure counter */
        private int m_FireResistance;

        private bool m_HasGeneratedLoot; // have we generated our loot yet?
        private TimerExecutionToken _healTimerToken;
        private TimerExecutionToken _provocationTimer;
        private TimerExecutionToken _peacemakingTimer;
        private TimerExecutionToken _minionTimer;

        private Point3D m_Home; // The home position of the creature, used by some AI

        private DateTime m_IdleReleaseTime;

        private bool m_IsBonded;

        private bool m_IsStabled;
        protected int m_KillersLuck;

        private int m_Loyalty;

        private DateTime m_MLNextShout;

        private List<MLQuest> m_MLQuests;

        private long m_NextAura;

        private long m_NextHealOwnerTime = Core.TickCount;

        private long m_NextHealTime = Core.TickCount;

        private long m_NextRummageTime;

        private bool m_Paragon;
        private bool m_Heroic;
        private bool m_Veteran;
        private bool m_Boss;
        private bool m_Minion;
        private List<Mobile> m_Minions;
        private int m_NumberOfMinions;
        private bool m_MagicResistant;
        private bool m_Reflective;
        private bool m_Regenerative;
        private bool m_Illusionist;
        private bool m_Ethereal;
        private bool m_Corruptor;
        private bool m_Corrupted;
        private bool m_Electrified;
        private bool m_Burning;
        private bool m_Frozen;
        private bool m_Toxic;
        private bool m_SoulFeeder;

        private int m_Level;
        private int m_PhysicalResistance;
        private int m_PoisonResistance;

        /* until we are sure about who should be getting deleted, move them instead */
        /* On OSI, they despawn */

        private bool m_ReturnQueued;

        protected bool m_Spawning;

        private Mobile m_SummonMaster;

        private SkillName m_Teaching = (SkillName)(-1);

        private int m_Team; // Monster Team

        public BaseCreature(
            AIType ai,
            FightMode mode = FightMode.Closest,
            int iRangePerception = 10,
            int iRangeFight = 1
        )
        {
            if (iRangePerception == OldRangePerception)
            {
                iRangePerception = DefaultRangePerception;
            }

            m_Level = 1;
            m_Loyalty = MaxLoyalty; // Wonderfully Happy
            m_NextGambleTime = DateTime.Now;
            m_GambleLosses = 0;
            m_CannibalPoints = 0;
            m_CurrentAI = ai;
            m_DefaultAI = ai;
            m_Minions = new List<Mobile>();
            m_NumberOfMinions = 0;
            m_Blinded = false;
            m_Feared = false;

            RangePerception = iRangePerception;
            RangeFight = iRangeFight;

            FightMode = mode;

            ResetSpeeds();

            m_Team = 0;

            Debug = false;

            m_Controlled = false;
            m_ControlMaster = null;
            ControlTarget = null;
            m_ControlOrder = OrderType.None;

            m_bTamable = false;

            Owners = new List<Mobile>();

            NextReacquireTime = Core.TickCount + (int)ReacquireDelay.TotalMilliseconds;

            ChangeAIType(AI);

            var speechType = SpeechType;

            speechType?.OnConstruct(this);

            if (IsInvulnerable && !Core.AOS)
            {
                NameHue = 0x35;
            }

            GenerateLoot(true);
        }

        public BaseCreature(Serial serial) : base(serial)
        {
            Debug = false;
        }

        public virtual string DefaultName => null;
        public virtual string CorpseName => null;

        [CommandProperty(AccessLevel.GameMaster)]
        public override string Name
        {
            get
            {
                if (NameMod == null && base.Name == null)
                {
                    return DefaultName;
                }

                return base.Name;
            }
            set => base.Name = value == DefaultName ? null : value;
        }

        public virtual InhumanSpeech SpeechType => null;

        /* Do not serialize this till the code is finalized */

        [CommandProperty(AccessLevel.GameMaster)]
        public bool SeeksHome { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Feared
        {
            get
            {
                return m_Feared;
            }
            set
            {
                m_Feared = value;
                if (value) {
                    SpeedMod = CurrentSpeed*2;
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Blinded
        {
            get
            {
                return m_Blinded;
            }
            set
            {
                m_Blinded = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string CorpseNameOverride { get; set; }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public bool IsStabled
        {
            get => m_IsStabled;
            set
            {
                m_IsStabled = value;
                if (m_IsStabled)
                {
                    StopDeleteTimer();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public Mobile StabledBy { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsPrisoner { get; set; }

        protected DateTime SummonEnd { get; set; }

        public virtual Faction FactionAllegiance => null;
        public virtual int FactionSilverWorth => 30;

        public virtual double WeaponAbilityChance => 0.4;

        public BaseTalent TalentEffect { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ExperienceValue { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxLevel { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsParagon
        {
            get => m_Paragon;
            set
            {
                if (m_Paragon == value)
                {
                    return;
                }

                if (value)
                {
                    Paragon.Convert(this);
                }
                else
                {
                    Paragon.UnConvert(this);
                }

                m_Paragon = value;

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsVeteran
        {
            get => m_Veteran;
            set
            {
                if (m_Veteran == value)
                {
                    return;
                }
                m_Veteran = value;
                if (value)
                {
                    Veteran.Convert(this);
                }
                else
                {
                    Veteran.UnConvert(this);
                }
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsCorrupted
        {
            get => m_Corrupted;
            set
            {
                if (m_Corrupted == value)
                {
                    return;
                }
                m_Corrupted = value;
                if (value)
                {
                    MonsterBuff.AddCorrupted(this);
                }
                else
                {
                    MonsterBuff.RemoveCorrupted(this);
                }
                InvalidateProperties();
            }
        }
        public int Level
        {
            get => m_Level;
            set
            {
                m_Level = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SoulFeeds { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsSoulFeeder
        {
            get => m_SoulFeeder;
            set
            {
                if (m_SoulFeeder == value)
                {
                    return;
                }
                m_SoulFeeder = value;
                if (value)
                {
                    MonsterBuff.AddCorrupted(this);
                }
                else
                {
                    MonsterBuff.RemoveCorrupted(this);
                }
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsFrozen
        {
            get => m_Frozen;
            set
            {
                if (m_Frozen == value)
                {
                    return;
                }
                m_Frozen = value;
                if (value)
                {
                    MonsterBuff.AddElementalProperties(this);
                }
                else
                {
                    MonsterBuff.RemoveElementalProperties(this);
                }
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsBurning
        {
            get => m_Burning;
            set
            {
                if (m_Burning == value)
                {
                    return;
                }
                m_Burning = value;
                if (value)
                {
                    MonsterBuff.AddElementalProperties(this);
                }
                else
                {
                    MonsterBuff.RemoveElementalProperties(this);
                }
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsElectrified
        {
            get => m_Electrified;
            set
            {
                if (m_Electrified == value)
                {
                    return;
                }
                m_Electrified = value;
                if (value)
                {
                    MonsterBuff.AddElementalProperties(this);
                }
                else
                {
                    MonsterBuff.RemoveElementalProperties(this);
                }
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsToxic
        {
            get => m_Toxic;
            set
            {
                if (m_Toxic == value)
                {
                    return;
                }
                m_Toxic = value;
                if (value)
                {
                    MonsterBuff.AddElementalProperties(this);
                }
                else
                {
                    MonsterBuff.RemoveElementalProperties(this);
                }
                InvalidateProperties();
            }
        }


        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsCorruptor
        {
            get => m_Corruptor;
            set
            {
                if (m_Corruptor == value)
                {
                    return;
                }
                m_Corruptor = value;
                if (value)
                {
                    MonsterBuff.AddCorruptor(this);
                }
                else
                {
                    MonsterBuff.RemoveCorruptor(this);
                }
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsEthereal
        {
            get => m_Ethereal;
            set
            {
                if (m_Ethereal == value)
                {
                    return;
                }
                m_Ethereal = value;
                if (value)
                {
                    MonsterBuff.AddEthereal(this);
                }
                else
                {
                    MonsterBuff.RemoveEthereal(this);
                }
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsIllusionist
        {
            get => m_Illusionist;
            set
            {
                if (m_Illusionist == value)
                {
                    return;
                }
                m_Illusionist = value;
                if (value)
                {
                    MonsterBuff.AddIllusionist(this);
                }
                else
                {
                    MonsterBuff.RemoveIllusionist(this);
                }
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsRegenerative
        {
            get => m_Regenerative;
            set
            {
                if (m_Regenerative == value)
                {
                    return;
                }
                m_Regenerative = value;
                if (value)
                {
                    MonsterBuff.AddRegenerative(this);
                }
                else
                {
                    MonsterBuff.RemoveRegenerative(this);
                }
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsReflective
        {
            get => m_Reflective;
            set
            {
                if (m_Reflective == value)
                {
                    return;
                }
                m_Reflective = value;
                if (value)
                {
                    MonsterBuff.AddReflective(this);
                }
                else
                {
                    MonsterBuff.RemoveReflective(this);
                }
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsMagicResistant
        {
            get => m_MagicResistant;
            set
            {
                if (m_MagicResistant == value)
                {
                    return;
                }
                m_MagicResistant = value;
                if (value)
                {
                    MonsterBuff.AddMagicResistant(this);
                }
                else
                {
                    MonsterBuff.RemoveMagicResistant(this);
                }
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsMinion
        {
            get => m_Minion;
            set
            {
                m_Minion = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsBoss
        {
            get => m_Boss;
            set
            {
                if (m_Boss == value)
                {
                    return;
                }
                m_Boss = value;
                if (value)
                {
                    var constructor = GetType().GetConstructor(Type.EmptyTypes);
                    if (constructor != null) // dont try and spawn boss types that have optional parameters (i.e mounts or monsters spawned by other monsters)
                    {
                        MonsterBuff.Bossify(this);
                    } else
                    {
                        m_Boss = false;
                    }
                }
                else
                {
                    MonsterBuff.UnBossify(this);
                }
                InvalidateProperties();
            }
        }

        public void Fear(int duration , string message = "* Feared *")
        {
            Feared = true;
            PublicOverheadMessage(
                MessageType.Regular,
                0x3B2,
                false,
                message
                );
            Timer.StartTimer(TimeSpan.FromSeconds(duration), UnFear);
        }

        public void UnFear()
        {
            Feared = false;
            SpeedMod = 0;
        }

        public void Blind(int duration, string message = "* Blinded *")
        {
            Blinded = true;
            PublicOverheadMessage(
                MessageType.Regular,
                0x3B2,
                false,
                message
                );
            Timer.StartTimer(TimeSpan.FromSeconds(duration), UnBlind);
        }
        public void UnBlind()
        {
            Blinded = false;
        }

        public void InitBossTimer()
        {
            Timer.StartTimer(TimeSpan.FromSeconds(3), CheckMinions, out _minionTimer);
        }


        public void CheckMinions()
        {
            if (Alive && !Deleted)
            {
                if (Minions != null && Minions.Count > 0)
                {
                    if (Combatant != null)
                    {
                        List<Mobile> mobilesInRange = new List<Mobile>(GetMobilesInRange(12));
                        foreach (Mobile minion in Minions.ToList())
                        {
                            if (!minion.Alive)
                            {
                                Minions.Remove(minion);
                                continue;
                            }
                            Mobile nearby = mobilesInRange.Find(m => m.Serial == minion.Serial);
                            if (nearby != null)
                            {
                                if (nearby.Combatant != Combatant)
                                {
                                    nearby.Attack(Combatant);
                                    nearby.Combatant = Combatant;
                                }
                            }
                            else
                            {
                                Point3D point = new Point3D(Location.X, Location.Y, Location.Z);
                                minion.MoveToWorld(point, Map);
                            }
                        }
                        InitBossTimer();
                    } else
                    {
                        MonsterBuff.DespawnMinions(this);
                    }
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsHeroic
        {
            get => m_Heroic;
            set
            {
                if (m_Heroic == value)
                {
                    return;
                }
                m_Heroic = value;
                if (value) {
                    Heroic.Convert(this);
                } else
                {
                    Heroic.UnConvert(this);
                }
                InvalidateProperties();
            }
        }

        public virtual bool HasManaOveride => false;

        public virtual FoodType FavoriteFood => FoodType.Meat;
        public virtual PackInstinct PackInstinct => PackInstinct.None;

        public List<Mobile> Owners { get; private set; }

        public virtual bool AllowMaleTamer => true;
        public virtual bool AllowFemaleTamer => true;
        public virtual bool SubdueBeforeTame => false;
        public virtual bool StatLossAfterTame => SubdueBeforeTame;
        public virtual bool ReduceSpeedWithDamage => true;
        public virtual bool IsSubdued => SubdueBeforeTame && Hits < HitsMax / 10;

        public virtual bool Commandable => true;

        public virtual Poison HitPoison => null;
        public virtual double HitPoisonChance => 0.5;
        public virtual Poison PoisonImmune => null;

        public virtual bool BardImmune => false;
        public virtual bool Unprovokable => BardImmune || IsDeadPet;
        public virtual bool Uncalmable => BardImmune || IsDeadPet;
        public virtual bool AreaPeaceImmune => BardImmune || IsDeadPet;

        public virtual bool BleedImmune => false;
        public virtual double BonusPetDamageScalar => 1.0;

        public virtual bool DeathAdderCharmable => false;

        // TODO: Find the pub 31 tweaks to the DispelDifficulty and apply them of course.
        // at this skill level we dispel 50% chance
        public virtual double DispelDifficulty => 0.0;

        // at difficulty - focus we have 0%, at difficulty + focus we have 100%
        public virtual double DispelFocus => 20.0;

        public virtual bool DisplayWeight => Backpack is StrongBackpack;

        public virtual bool CanFly => false;

        public virtual bool IsInvulnerable => false;

        public BaseAI AIObject { get; private set; }

        public virtual OppositionGroup[] OppositionGroups => null;

        public virtual bool IsAnimatedDead
        {
            get
            {
                if (!Summoned)
                {
                    return false;
                }

                var type = GetType();

                var contains = false;

                for (var i = 0; !contains && i < m_AnimateDeadTypes.Length; ++i)
                {
                    contains = type == m_AnimateDeadTypes[i];
                }

                return contains;
            }
        }

        public virtual bool IsNecroFamiliar =>
            Summoned && m_ControlMaster != null &&
            SummonFamiliarSpell.Table.TryGetValue(m_ControlMaster, out var bc) && bc == this;

        public virtual bool DeleteCorpseOnDeath => !Core.AOS && _summoned;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Loyalty
        {
            get => m_Loyalty;
            set => m_Loyalty = Math.Clamp(value, 0, MaxLoyalty);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public WayPoint CurrentWayPoint { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public IPoint2D TargetLocation { get; set; }

        public virtual Mobile ConstantFocus => null;

        public virtual bool DisallowAllMoves => false;

        public virtual bool InitialInnocent => false;

        public virtual bool AlwaysMurderer => false;

        public virtual bool AlwaysAttackable => false;

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual int DamageMin
        {
            get => m_DamageMin;
            set => m_DamageMin = value;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual int DamageMax
        {
            get => m_DamageMax;
            set => m_DamageMax = value;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override int HitsMax =>
            HitsMaxSeed <= 0 ? Str : Math.Clamp(HitsMaxSeed + GetStatOffset(StatType.Str), 1, 65000);

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitsMaxSeed { get; set; } = -1;

        [CommandProperty(AccessLevel.GameMaster)]
        public override int StamMax =>
            StamMaxSeed <= 0 ? Dex : Math.Clamp(StamMaxSeed + GetStatOffset(StatType.Dex), 1, 65000);

        [CommandProperty(AccessLevel.GameMaster)]
        public int StamMaxSeed { get; set; } = -1;

        [CommandProperty(AccessLevel.GameMaster)]
        public override int ManaMax =>
            ManaMaxSeed <= 0 ? Int : Math.Clamp(ManaMaxSeed + GetStatOffset(StatType.Int), 1, 65000);

        [CommandProperty(AccessLevel.GameMaster)]
        public int ManaMaxSeed { get; set; } = -1;

        public virtual bool CanOpenDoors => !Body.IsAnimal && !Body.IsSea;

        public virtual bool CanMoveOverObstacles => Core.AOS || Body.IsMonster;

        public virtual bool CanDestroyObstacles => false;

        /*
        Seems this actually was removed on OSI somewhere between the original bug report and now.
        We will call it ML, until we can get better information. I suspect it was on the OSI TC when
        originally it taken out of RunUO, and not implemented on OSIs production shards until more
        recently.  Either way, this is, or was, accurate OSI behavior, and just entirely
        removing it was incorrect.  OSI followers were distracted by being attacked well into
        AoS, at very least.

        */

        public virtual bool CanBeDistracted => !Core.ML;

        public override bool ShouldCheckStatTimers => false;

        public virtual bool CanAngerOnTame => false;

        protected virtual BaseAI ForcedAI => null;

        [CommandProperty(AccessLevel.GameMaster)]
        public AIType AI
        {
            get => m_CurrentAI;
            set
            {
                m_CurrentAI = value;

                if (m_CurrentAI == AIType.AI_Use_Default)
                {
                    m_CurrentAI = m_DefaultAI;
                }

                ChangeAIType(m_CurrentAI);
            }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public bool Debug { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Team
        {
            get => m_Team;
            set
            {
                m_Team = value;
                OnTeamChange();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile FocusMob { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public FightMode FightMode { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RangePerception { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RangeFight { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RangeHome { get; set; } = 10;

        [CommandProperty(AccessLevel.GameMaster)]
        public double ActiveSpeed { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public double PassiveSpeed { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public double SpeedMod { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public double CurrentSpeed
        {
            get => TargetLocation != null ? 0.3 : SpeedMod <= 0 ? m_CurrentSpeed : SpeedMod;
            set
            {
                if (m_CurrentSpeed != value)
                {
                    m_CurrentSpeed = value;

                    if (SpeedMod <= 0)
                    {
                        AIObject?.OnCurrentSpeedChanged();
                    }
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Home
        {
            get => m_Home;
            set => m_Home = value;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map HomeMap { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Controlled
        {
            get => m_Controlled;
            set
            {
                if (m_Controlled == value)
                {
                    return;
                }

                m_Controlled = value;
                Delta(MobileDelta.Noto);

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile ControlMaster
        {
            get => m_ControlMaster;
            set
            {
                if (m_ControlMaster == value || this == value)
                {
                    return;
                }

                RemoveFollowers();
                m_ControlMaster = value;
                AddFollowers();
                if (m_ControlMaster != null)
                {
                    StopDeleteTimer();
                }

                Delta(MobileDelta.Noto);
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile SummonMaster
        {
            get => m_SummonMaster;
            set
            {
                if (m_SummonMaster == value || this == value)
                {
                    return;
                }

                RemoveFollowers();
                m_SummonMaster = value;
                AddFollowers();

                Delta(MobileDelta.Noto);
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile ControlTarget { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D ControlDest { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public OrderType ControlOrder
        {
            get => m_ControlOrder;
            set
            {
                m_ControlOrder = value;

                AIObject?.OnCurrentOrderChanged();

                InvalidateProperties();

                m_ControlMaster?.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool BardProvoked { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool BardPacified { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile BardMaster { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile BardTarget { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime BardEndTime { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public double MinTameSkill { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Tamable
        {
            get => m_bTamable && !m_Paragon;
            set => m_bTamable = value;
        }

        [CommandProperty(AccessLevel.Administrator)]
        public bool Summoned
        {
            get => _summoned;
            set
            {
                if (_summoned == value)
                {
                    return;
                }

                NextReacquireTime = Core.TickCount;

                _summoned = value;
                Delta(MobileDelta.Noto);

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int ControlSlots { get; set; } = 1;

        public virtual bool NoHouseRestrictions => false;
        public virtual bool IsHouseSummonable => false;

        public virtual bool AutoDispel => false;
        public virtual double AutoDispelChance => Core.SE ? .10 : 1.0;

        public virtual bool IsScaryToPets => false;
        public virtual bool IsScaredOfScaryThings => true;

        public virtual bool CanRummageCorpses => false;

        public virtual bool DeleteOnRelease => _summoned;

        public virtual bool CanDrop => IsBonded;

        public virtual int TreasureMapLevel => -1;

        public virtual bool IgnoreYoungProtection => false;

        public bool NoKillAwards { get; set; }

        public virtual bool GivesMLMinorArtifact => false;

        /* To save on cpu usage, RunUO creatures only reacquire creatures under the following circumstances:
         *  - 10 seconds have elapsed since the last time it tried
         *  - The creature was attacked
         *  - Some creatures, like dragons, will reacquire when they see someone move
         *
         * This functionality appears to be implemented on OSI as well
         */

        public long NextReacquireTime { get; set; }

        public virtual TimeSpan ReacquireDelay => TimeSpan.FromSeconds(10.0);
        public virtual bool ReacquireOnMovement => false;
        public virtual bool AcquireOnApproach => OppositionGroups is not null || m_Paragon;
        public virtual int AcquireOnApproachRange => 20;

        public static bool Summoning { get; set; }

        public virtual bool IsDispellable => Summoned && !IsAnimatedDead;
        public virtual bool OverrideDispellable { get; set; }

        // If they are following a waypoint, they'll continue to follow it even if players aren't around
        public virtual bool PlayerRangeSensitive => CurrentWayPoint == null;

        public virtual bool ReturnsToHome =>
            SeeksHome && Home != Point3D.Zero && !m_ReturnQueued && !Controlled && !Summoned;

        public virtual bool ScaleSpeedByDex => NPCSpeeds.ScaleSpeedByDex && !IsMonster;

        // used for deleting untamed creatures [in houses]
        [CommandProperty(AccessLevel.GameMaster)]
        public bool RemoveIfUntamed { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RemoveStep { get; set; }

        public virtual bool CanGiveMLQuest => MLQuests.Count != 0;
        public virtual bool StaticMLQuester => true;

        public virtual bool CanShout => false;

        public static bool BondingEnabled { get; private set; }

        public virtual bool IsBondable => BondingEnabled && !Summoned;
        public virtual TimeSpan BondingDelay => TimeSpan.FromDays(7.0);
        public virtual TimeSpan BondingAbandonDelay => TimeSpan.FromDays(1.0);

        public override bool CanRegenHits => !IsDeadPet && !Summoned && base.CanRegenHits;
        public override bool CanRegenStam => !IsParagon && !IsDeadPet && base.CanRegenStam;
        public override bool CanRegenMana => !IsDeadPet && base.CanRegenMana;

        public override bool IsDeadBondedPet => IsDeadPet;

        [CommandProperty(AccessLevel.GameMaster)]
        public Spawner MySpawner => Spawner as Spawner;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile LastOwner
        {
            get
            {
                if (Owners == null || Owners.Count == 0)
                {
                    return null;
                }

                return Owners[^1];
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsBonded
        {
            get => m_IsBonded;
            set
            {
                m_IsBonded = value;
                InvalidateProperties();
            }
        }

        public bool IsDeadPet { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime BondingBegin { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime OwnerAbandonTime { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan DeleteTimeLeft
        {
            get
            {
                if (m_DeleteTimer?.Running == true)
                {
                    return m_DeleteTimer.Next - Core.Now;
                }

                return TimeSpan.Zero;
            }
        }

        public override int BasePhysicalResistance => m_PhysicalResistance;
        public override int BaseFireResistance => m_FireResistance;
        public override int BaseColdResistance => m_ColdResistance;
        public override int BasePoisonResistance => m_PoisonResistance;
        public override int BaseEnergyResistance => m_EnergyResistance;

        [CommandProperty(AccessLevel.GameMaster)]
        public int PhysicalResistanceSeed
        {
            get => m_PhysicalResistance;
            set
            {
                m_PhysicalResistance = value;
                UpdateResistances();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FireResistSeed
        {
            get => m_FireResistance;
            set
            {
                m_FireResistance = value;
                UpdateResistances();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ColdResistSeed
        {
            get => m_ColdResistance;
            set
            {
                m_ColdResistance = value;
                UpdateResistances();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PoisonResistSeed
        {
            get => m_PoisonResistance;
            set
            {
                m_PoisonResistance = value;
                UpdateResistances();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EnergyResistSeed
        {
            get => m_EnergyResistance;
            set
            {
                m_EnergyResistance = value;
                UpdateResistances();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PhysicalDamage { get; set; } = 100;

        [CommandProperty(AccessLevel.GameMaster)]
        public int FireDamage { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ColdDamage { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PoisonDamage { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EnergyDamage { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ChaosDamage { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DirectDamage { get; set; }

        // Is immune to breath damages
        public virtual bool BreathImmune => false;

        public virtual bool CanFlee => !m_Paragon;

        public DateTime EndFleeTime { get; set; }

        public List<Mobile> Friends { get; private set; }

        public virtual bool AllowNewPetFriend => Friends == null || Friends.Count < 5;

        public virtual Ethic EthicAllegiance => null;

        public virtual int Feathers => 0;
        public virtual int Wool => 0;

        public virtual MeatType MeatType => MeatType.Ribs;
        public virtual int Meat => 0;

        public virtual int Hides => 0;
        public virtual HideType HideType => HideType.Regular;

        public virtual int Scales => 0;
        public virtual ScaleType ScaleType => ScaleType.Red;

        public virtual bool CanTeach => false;

        public virtual bool CanHeal => false;
        public virtual bool CanHealOwner => false;
        public virtual double HealScalar => 1.0;

        public virtual int HealSound => 0x57;
        public virtual int HealStartRange => 2;
        public virtual int HealEndRange => RangePerception;
        public virtual double HealTrigger => 0.78;
        public virtual double HealDelay => 6.5;
        public virtual double HealInterval => 0.0;
        public virtual bool HealFully => true;
        public virtual double HealOwnerTrigger => 0.78;
        public virtual double HealOwnerDelay => 6.5;
        public virtual double HealOwnerInterval => 30.0;
        public virtual bool HealOwnerFully => false;

        public bool IsHealing => _healTimerToken.Running;

        public virtual bool HasAura => false;
        public virtual TimeSpan AuraInterval => TimeSpan.FromSeconds(5);
        public virtual int AuraRange => 4;

        public virtual int AuraBaseDamage => 5;
        public virtual int AuraPhysicalDamage => 0;
        public virtual int AuraFireDamage => 100;
        public virtual int AuraColdDamage => 0;
        public virtual int AuraPoisonDamage => 0;
        public virtual int AuraEnergyDamage => 0;
        public virtual int AuraChaosDamage => 0;

        public HonorContext ReceivedHonorContext { get; set; }

       public int NumberOfMinions
        {
            get => m_NumberOfMinions;
            set
            {
                m_NumberOfMinions = value;
            }
        }
        public List<Mobile> Minions
        {
            get => m_Minions;
            set
            {
                m_Minions = value;
            }
        }
        public List<MLQuest> MLQuests
        {
            get
            {
                if (m_MLQuests == null)
                {
                    if (StaticMLQuester)
                    {
                        m_MLQuests = MLQuestSystem.FindQuestList(GetType());
                    }
                    else
                    {
                        m_MLQuests = ConstructQuestList();
                    }

                    if (m_MLQuests == null)
                    {
                        // return EmptyList, but don't cache it (run construction again next time)
                        return MLQuestSystem.EmptyList;
                    }
                }

                return m_MLQuests;
            }
        }

        public virtual MonsterAbility[] GetMonsterAbilities() => null;

        public virtual MonsterAbility GetAbility(MonsterAbilityType type)
        {
            var abilities = GetMonsterAbilities();

            if (abilities == null)
            {
                return null;
            }

            for (var i = 0; i < abilities.Length; i++)
            {
                var ability = abilities[i];
                if (ability is MonsterAbilityGroup group)
                {
                    ability = group.GetAbilityWithType(type);
                    if (ability != null)
                    {
                        return ability;
                    }
                }
                else if (ability.AbilityType == type)
                {
                    return ability;
                }
            }

            return null;
        }

        public virtual bool HasAbility(MonsterAbility ability)
        {
            if (ability == null)
            {
                return false;
            }

            var abilities = GetMonsterAbilities();

            if (abilities == null)
            {
                return false;
            }

            for (var i = 0; i < abilities.Length; i++)
            {
                if (abilities[i] == ability || (ability as MonsterAbilityGroup)?.HasAbility(ability) == true)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool TriggerAbility(MonsterAbilityTrigger trigger, Mobile defender)
        {
            var abilities = GetMonsterAbilities();

            if (abilities == null)
            {
                return false;
            }

            var triggered = false;
            for (var i = 0; i < abilities.Length; i++)
            {
                var ability = abilities[i];
                if (ability.WillTrigger(trigger) && ability.CanTrigger(this, trigger))
                {
                    ability.Trigger(trigger, this, defender);
                    triggered = true;
                }
            }

            return triggered;
        }

        public virtual void TriggerAbilityMove(MonsterAbilityTrigger trigger, Mobile defender, Direction d)
        {
            var abilities = GetMonsterAbilities();

            if (abilities == null)
            {
                return;
            }

            for (var i = 0; i < abilities.Length; i++)
            {
                var ability = abilities[i];
                if (ability.WillTrigger(trigger) && ability.CanTrigger(this, trigger))
                {
                    ability.Move(this, d);
                }
            }
        }

        public virtual void TriggerAbilityAlterDamage(MonsterAbilityTrigger trigger, Mobile defender, ref int damage)
        {
            var abilities = GetMonsterAbilities();

            if (abilities == null)
            {
                return;
            }

            for (var i = 0; i < abilities.Length; i++)
            {
                var ability = abilities[i];
                if (ability.WillTrigger(trigger) && ability.CanTrigger(this, trigger))
                {
                    if ((trigger & MonsterAbilityTrigger.GiveMeleeDamage) != 0)
                    {
                        ability.AlterMeleeDamageTo(this, defender, ref damage);
                    }

                    if ((trigger & MonsterAbilityTrigger.TakeMeleeDamage) != 0)
                    {
                        ability.AlterMeleeDamageFrom(this, defender, ref damage);
                    }

                    if ((trigger & MonsterAbilityTrigger.GiveSpellDamage) != 0)
                    {
                        ability.AlterSpellDamageTo(this, defender, ref damage);
                    }

                    if ((trigger & MonsterAbilityTrigger.TakeSpellDamage) != 0)
                    {
                        ability.AlterSpellDamageFrom(this, defender, ref damage);
                    }
                }
            }
        }

        public virtual void TriggerAbilityAlterDamageScalar(
            MonsterAbilityTrigger trigger,
            Mobile defender,
            ref double scalar
        )
        {
            var abilities = GetMonsterAbilities();

            if (abilities == null)
            {
                return;
            }

            for (var i = 0; i < abilities.Length; i++)
            {
                var ability = abilities[i];
                if (ability.WillTrigger(trigger) && ability.CanTrigger(this, trigger))
                {
                    if ((trigger & MonsterAbilityTrigger.GiveSpellDamage) != 0)
                    {
                        ability.AlterSpellDamageScalarTo(this, defender, ref scalar);
                    }

                    if ((trigger & MonsterAbilityTrigger.TakeSpellDamage) != 0)
                    {
                        ability.AlterSpellDamageScalarFrom(this, defender, ref scalar);
                    }
                }
            }
        }

        public virtual WeaponAbility GetWeaponAbility() => null;

        public bool IsOpposing(Mobile m)
        {
            bool opposingGroup = false;
            if (OppositionGroups is not null)
            {
                foreach (var oppositionGroup in OppositionGroups)
                {

                    opposingGroup = oppositionGroup.IsEnemy(this, m);
                    if (opposingGroup)
                    {
                        break;
                    }
                }
            }
            return opposingGroup;
        }

        public virtual bool IsEnemy(Mobile m)
        {
            if (IsOpposing(m))
            {
                return true;
            }

            if (m is BaseGuard)
            {
                return false;
            }

            if (GetFactionAllegiance(m) == Allegiance.Ally)
            {
                return false;
            }

            var ourEthic = EthicAllegiance;
            var pl = Ethics.Player.Find(m, true);

            if (pl?.IsShielded == true && (ourEthic == null || ourEthic == pl.Ethic))
            {
                return false;
            }

            if (m is PlayerMobile mobile && (mobile.HonorActive || Deity.AlignmentCheck(this, mobile.CombatAlignment, false)))
            {
                return false;
            }

            if (m is not BaseCreature c || m is MilitiaFighter)
            {
                return true;
            }

            if (TransformationSpellHelper.UnderTransformation(m, typeof(EtherealVoyageSpell)))
            {
                return false;
            }

            if (FightMode == FightMode.Evil && m.Karma < 0 || c.FightMode == FightMode.Evil && Karma < 0)
            {
                return true;
            }

            return m_Team != c.m_Team || (_summoned || m_Controlled) != (c._summoned || c.m_Controlled);
        }

        public override string ApplyNameSuffix(string suffix)
        {
            if (IsParagon && !GivesMLMinorArtifact)
            {
                suffix = suffix.Length == 0 ? "(Paragon)" : $"{suffix} (Paragon)";
            }
            if (IsHeroic)
            {
                suffix = suffix.Length == 0 ? "(Heroic)" : $"{suffix} (Heroic)";
            } else if (IsVeteran)
            {
                suffix = suffix.Length == 0 ? "(Veteran)" : $"{suffix} (Veteran)";
            }

            return base.ApplyNameSuffix(suffix);
        }

        public virtual bool CheckControlChance(Mobile m)
        {
            if (GetControlChance(m) > Utility.RandomDouble() || ((PlayerMobile)ControlMaster).GetTalent(typeof(Leadership)) is not null)
            {
                Loyalty += 1;
                return true;
            }

            PlaySound(GetAngerSound());

            if (Body.IsAnimal)
            {
                Animate(10, 5, 1, true, false, 0);
            }
            else if (Body.IsMonster)
            {
                Animate(18, 5, 1, true, false, 0);
            }

            Loyalty -= 3;
            return false;
        }

        public virtual bool CanBeControlledBy(Mobile m) => GetControlChance(m) > 0.0;

        public virtual double GetControlChance(Mobile m, bool useBaseSkill = false)
        {
            if (MinTameSkill <= 29.1 || _summoned || m.AccessLevel >= AccessLevel.GameMaster)
            {
                return 1.0;
            }

            var minTameSkill = MinTameSkill;

            if (minTameSkill > -24.9 && AnimalTaming.CheckMastery(m, this))
            {
                minTameSkill = -24.9;
            }

            var taming = useBaseSkill
                ? m.Skills.AnimalTaming.BaseFixedPoint
                : m.Skills.AnimalTaming.Fixed;
            var lore = useBaseSkill
                ? m.Skills.AnimalLore.BaseFixedPoint
                : m.Skills.AnimalLore.Fixed;

            int bonus;

            if (Core.ML)
            {
                var skillBonus = taming - (int)(minTameSkill * 10);
                var loreBonus = lore - (int)(minTameSkill * 10);

                var skillMod = 6;
                var loreMod = 6;

                if (skillBonus < 0)
                {
                    skillMod = 28;
                }

                if (loreBonus < 0)
                {
                    loreMod = 14;
                }

                skillBonus *= skillMod;
                loreBonus *= loreMod;

                bonus = (skillBonus + loreBonus) / 2;
            }
            else
            {
                var difficulty = (int)(minTameSkill * 10);
                var weighted = (taming * 4 + lore) / 5;
                bonus = weighted - difficulty;

                if (bonus <= 0)
                {
                    bonus *= 14;
                }
                else
                {
                    bonus *= 6;
                }
            }

            var chance = Math.Clamp(700 + bonus, 220, 990);

            chance -= (MaxLoyalty - m_Loyalty) * 10;

            return chance / 1000.0;
        }

        public override void Damage(int amount, Mobile from = null, bool informMount = true)
        {
            var oldHits = Hits;
            if (IsEthereal && Utility.Random(100) < 30)
            {
                amount = 0;
                PublicOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    false,
                    "* The creature absorbs your attack *"
                );
            }
            bool corruptionOffset = false;
            if (IsCorruptor && amount > 0)
            {
                foreach (Mobile mobile in GetMobilesInRange(MonsterBuff.CorruptionRange))
                {
                    if (mobile is BaseCreature { IsCorrupted: true } && mobile != this)
                    {
                        corruptionOffset = true;
                        mobile.Damage(amount, from);
                        mobile.Combatant = from;
                        amount = 0;
                        break;
                    }
                }
                if (corruptionOffset)
                {
                    PublicOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        false,
                        "* The creature redirects the attack to a nearby corrupted target *"
                    );
                }
            }

            if (Core.AOS && !Summoned && Controlled && Utility.RandomDouble() < 0.2)
            {
                amount = (int)(amount * BonusPetDamageScalar);
            }

            if (EvilOmenSpell.EndEffect(this))
            {
                double modifier = 1.25;
                if (from is PlayerMobile attacker)
                {
                    BaseTalent darkAffinity = attacker.GetTalent(typeof(DarkAffinity));
                    if (darkAffinity != null)
                    {
                        // increase damage by 1% for each point in dark affinity
                        modifier += darkAffinity.Level / 100.00;
                    }
                }
                amount = (int)(amount * modifier);
            }

            if (from != null && BloodOathSpell.GetBloodOath(from) == this)
            {
                amount = (int)(amount * 1.1);
                from.Damage(amount, from);
            }

            base.Damage(amount, from, informMount);

            if (IsReflective)
            {
                if (from != null)
                {
                    from.Damage(AOS.Scale(amount, 50), this);
                    from.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                }
            }

            if (SubdueBeforeTame && !Controlled && oldHits > HitsMax / 10 && Hits <= HitsMax / 10)
            {
                PublicOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    false,
                    "* The creature has been beaten into subjugation! *"
                );
            }
        }

        public override void SetLocation(Point3D newLocation, bool isTeleport)
        {
            base.SetLocation(newLocation, isTeleport);

            if (isTeleport)
            {
                AIObject?.OnTeleported();
            }
        }

        public void CheckBuffs()
        {
            Type type = GetType();
            if (type != typeof(BaseVendor) && type != typeof(BaseGuard) && type != typeof(BaseFamiliar) && !Region.IsPartOf<TownRegion>())
            {
                // everything gets a base minion buff
                MonsterBuff.Convert(this,
                    MonsterBuff.NoBuff,
                    MonsterBuff.MinionHitsBuff,
                    MonsterBuff.MinionStrBuff,
                    MonsterBuff.MinionIntBuff,
                    MonsterBuff.MinionDexBuff,
                    MonsterBuff.MinionSkillsBuff,
                    MonsterBuff.MinionSpeedBuff,
                    MonsterBuff.MinionFameBuff,
                    MonsterBuff.MinionKarmaBuff,
                    MonsterBuff.MinionDamageBuff);

                if (DynamicExperienceValue() >= 475) // skeleton strength and up
                {
                    // 5% chance in Dungeons 1% chance everywhere else
                    int chance = Region.IsPartOf<DungeonRegion>() ? 200 : 50;
                    IsHeroic = Utility.Random(10000) < chance;

                    if (!IsHeroic)
                    {
                        chance = Region.IsPartOf<DungeonRegion>() ? 2000 : 1000;
                        IsVeteran = Utility.Random(10000) < chance;
                    }
                    chance = 50;
                    IsIllusionist = Utility.Random(10000) < chance;
                    IsCorruptor = Utility.Random(10000) < chance;
                    IsEthereal = Utility.Random(10000) < chance;

                    if (DynamicExperienceValue() <= 1700)
                    {
                        IsBoss = Utility.Random(10000) < chance;
                    }
                    chance = 100;
                    IsMagicResistant = Utility.Random(10000) < chance;
                    switch (Utility.Random(4))
                    {
                        case 1:
                            IsFrozen = Utility.Random(10000) < chance;
                            break;
                        case 2:
                            IsBurning = Utility.Random(10000) < chance;
                            break;
                        case 3:
                            IsElectrified = Utility.Random(10000) < chance;
                            break;
                        case 4:
                            IsToxic = Utility.Random(10000) < chance;
                            break;
                    }
                    IsReflective = Utility.Random(10000) < chance;
                    IsRegenerative = Utility.Random(10000) < chance;
                    IsSoulFeeder = Utility.Random(10000) < chance;
                }
            }
        }
        public override void OnRawDexChange(int oldValue)
        {
            // This only really happens for pets or when a GM modifies a mob.
            if (oldValue != RawDex && ScaleSpeedByDex)
            {
                ResetSpeeds();
            }
        }

        public void SetLevel()
        {
            int baseXp = (int)DynamicExperienceValue() + ExperienceValue;
            // 38.4615384615
            // level range caps at 65 levels, and there are 7 tiers divided into groups of 500
            double levelRangePerTier = Math.Floor(65.00 / 7.00);
            double tier = baseXp / 500.00;
            int midLevel = (int)(tier * levelRangePerTier);
            int minLevel = midLevel - 5;
            if (minLevel <= 1)
            {
                minLevel = 2;
            }

            int maxLevelModifier = (int)Math.Ceiling(tier);
            MaxLevel = midLevel + maxLevelModifier;
            if (MaxLevel > 80)
            {
                ExperienceValue += (MaxLevel - 80) * 250;
                MaxLevel = 80;
                minLevel = 72;
                midLevel = 76;
            }
            Level = Utility.RandomMinMax(minLevel, MaxLevel);
            double buff = 1.00;
            buff += Level <= midLevel ? 0 : ((Level - midLevel)*5)/100.00;
            SetHits(Hits + Utility.RandomMinMax(1, Level));
            VirtualArmorMod += Level <= midLevel ? 0 : Level - midLevel;
            MonsterBuff.Convert(this, buff, buff, buff, buff, buff, buff, 1.00, buff, buff, 0);
        }

        public override void OnBeforeSpawn(Point3D location, Map m)
        {
            SetLevel();
            CheckBuffs();
            if (Paragon.CheckConvert(this, location, m))
            {
                IsParagon = true;
            }

            base.OnBeforeSpawn(location, m);
        }

        public override ApplyPoisonResult ApplyPoison(Mobile from, Poison poison)
        {
            if (!Alive || IsDeadPet)
            {
                return ApplyPoisonResult.Immune;
            }

            if (EvilOmenSpell.EndEffect(this))
            {
                poison = PoisonImpl.IncreaseLevel(poison);
            }

            var result = base.ApplyPoison(from, poison);

            if (from != null && result == ApplyPoisonResult.Poisoned && PoisonTimer is PoisonImpl.PoisonTimer timer)
            {
                timer.From = from;
            }

            return result;
        }

        public override bool CheckPoisonImmunity(Mobile from, Poison poison) =>
            base.CheckPoisonImmunity(from, poison) ||
            (m_Paragon ? PoisonImpl.IncreaseLevel(PoisonImmune) : PoisonImmune)?.Level >= poison.Level;

        public void Unpacify()
        {
            BardEndTime = Core.Now;
            BardPacified = false;
        }

        public virtual void CheckDistracted(Mobile from)
        {
            if (Utility.RandomDouble() < .10)
            {
                ControlTarget = from;
                ControlOrder = OrderType.Attack;
                Combatant = from;
                Warmode = true;
            }
        }

        public override void OnHitsChange(int oldValue)
        {
            base.OnHitsChange(oldValue);
            InvalidateProperties();
        }

        public override void OnManaChange(int oldValue)
        {
            base.OnManaChange(oldValue);
            InvalidateProperties();
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (BardPacified && (HitsMax - Hits) * 0.001 > Utility.RandomDouble())
            {
                Unpacify();
            }

            if ((BaseInstrument.IsMageryCreature(this) || Skills.Necromancy.Base > 5.0) && Hits >= AOS.Scale(HitsMax, 45))
            {
                Mana += amount / 2;
            }

            if (TalentEffect?.HasDamageAbsorptionEffect == true)
            {
                amount = TalentEffect.CheckDamageAbsorptionEffect(this, from, amount);
            }

            int disruptThreshold;
            // NPCs can use bandages too!
            if (!Core.AOS)
            {
                disruptThreshold = 0;
            }
            else if (from?.Player == true)
            {
                disruptThreshold = 18;
            }
            else
            {
                disruptThreshold = 25;
            }

            if (amount > disruptThreshold)
            {
                var c = BandageContext.GetContext(this);

                c?.Slip();
            }

            Confidence.StopRegenerating(this);

            WeightOverloading.FatigueOnDamage(this, amount);

            var speechType = SpeechType;

            if (speechType != null && !willKill)
            {
                speechType.OnDamage(this, amount);
            }

            ReceivedHonorContext?.OnTargetDamaged(from, amount);

            if (!willKill)
            {
                if (CanBeDistracted && ControlOrder == OrderType.Follow)
                {
                    CheckDistracted(from);
                }
            }
            else if (from is PlayerMobile mobile)
            {
                Timer.StartTimer(TimeSpan.FromSeconds(10), mobile.RecoverAmmo);
            }

            if (IsIllusionist && Utility.Random(100) < Utility.RandomMinMax(25, 45))
            {
                MonsterBuff.AddIllusion(this, from);
            }

            base.OnDamage(amount, from, willKill);
        }

        public virtual void OnDamagedBySpell(Mobile from, int damage)
        {
            if (CanBeDistracted && ControlOrder == OrderType.Follow)
            {
                CheckDistracted(from);
            }

            TriggerAbility(MonsterAbilityTrigger.TakeSpellDamage, from);
        }

        public virtual void OnDamageSpell(Mobile defender, int damage)
        {
            TriggerAbility(MonsterAbilityTrigger.GiveSpellDamage, defender);
        }

        public virtual void OnHarmfulSpell(Mobile from)
        {
        }

        public virtual void CheckReflect(Mobile caster, ref bool reflect)
        {
        }

        public virtual void OnCarve(Mobile from, Corpse corpse, Item with)
        {
            var feathers = Feathers;
            var wool = Wool;
            var meat = Meat;
            var hides = Hides;
            var scales = Scales;
            ResourcefulHarvester resourceful = null;
            EfficientSkinner skinner = null;
            if (from is PlayerMobile player)
            {
                resourceful = player.GetTalent(typeof(ResourcefulHarvester)) as ResourcefulHarvester;
                skinner = player.GetTalent(typeof(EfficientSkinner)) as EfficientSkinner;
            }

            if (feathers == 0 && wool == 0 && meat == 0 && hides == 0 && scales == 0 || Summoned || IsBonded ||
                corpse.Animated)
            {
                if (corpse.Animated)
                {
                    corpse.SendLocalizedMessageTo(from, 500464); // Use this on corpses to carve away meat and hide
                }
                else
                {
                    from.SendLocalizedMessage(500485); // You see nothing useful to carve from the corpse.
                }
            }
            else
            {
                if (Core.ML && from.Race == Race.Human)
                {
                    hides = (int)Math.Ceiling(hides * 1.1); // 10% bonus only applies to hides, ore & logs
                }

                if (resourceful != null)
                {
                    hides += resourceful.GetExtraResourceCheck(hides);
                    meat += resourceful.GetExtraResourceCheck(meat);
                    wool += resourceful.GetExtraResourceCheck(wool);
                    feathers += resourceful.GetExtraResourceCheck(feathers);
                    scales += resourceful.GetExtraResourceCheck(scales);
                }
                hides += BaseHides.CheckEfficientSkinner(from, hides);
                meat += BaseHides.CheckEfficientSkinner(from, meat);
                wool += BaseHides.CheckEfficientSkinner(from, wool);
                scales += BaseHides.CheckEfficientSkinner(from, scales);
                feathers += BaseHides.CheckEfficientSkinner(from, feathers);

                if (corpse.Map == Map.Felucca)
                {
                    feathers *= 2;
                    wool *= 2;
                    hides *= 2;

                    if (Core.ML)
                    {
                        meat *= 2;
                        scales *= 2;
                    }
                }

                new Blood(0x122D).MoveToWorld(corpse.Location, corpse.Map);

                if (feathers != 0)
                {
                    corpse.AddCarvedItem(new Feather(feathers), from);
                    from.SendLocalizedMessage(500479); // You pluck the bird. The feathers are now on the corpse.
                }

                if (wool != 0)
                {
                    corpse.AddCarvedItem(new TaintedWool(wool), from);
                    from.SendLocalizedMessage(500483); // You shear it, and the wool is now on the corpse.
                }

                if (meat != 0)
                {
                    if (MeatType == MeatType.Ribs)
                    {
                        corpse.AddCarvedItem(new RawRibs(meat), from);
                    }
                    else if (MeatType == MeatType.Bird)
                    {
                        corpse.AddCarvedItem(new RawBird(meat), from);
                    }
                    else if (MeatType == MeatType.LambLeg)
                    {
                        corpse.AddCarvedItem(new RawLambLeg(meat), from);
                    }

                    from.SendLocalizedMessage(500467); // You carve some meat, which remains on the corpse.
                }

                if (hides != 0)
                {
                    var holding = from.Weapon as Item;

                    if (Core.AOS && holding is SkinningKnife)
                    {
                        var leather = HideType switch
                        {
                            HideType.Regular => (Item)new Leather(hides),
                            HideType.Spined  => new SpinedLeather(hides),
                            HideType.Horned  => new HornedLeather(hides),
                            HideType.Barbed  => new BarbedLeather(hides),
                            _                => null
                        };

                        if (leather != null)
                        {
                            if (!from.PlaceInBackpack(leather))
                            {
                                corpse.DropItem(leather);
                                from.SendLocalizedMessage(500471); // You skin it, and the hides are now in the corpse.
                            }
                            else
                            {
                                from.SendLocalizedMessage(
                                    1073555
                                ); // You skin it and place the cut-up hides in your backpack.
                            }
                        }
                    }
                    else
                    {
                        if (HideType == HideType.Regular)
                        {
                            corpse.DropItem(new Hides(hides));
                        }
                        else if (HideType == HideType.Spined)
                        {
                            corpse.DropItem(new SpinedHides(hides));
                        }
                        else if (HideType == HideType.Horned)
                        {
                            corpse.DropItem(new HornedHides(hides));
                        }
                        else if (HideType == HideType.Barbed)
                        {
                            corpse.DropItem(new BarbedHides(hides));
                        }

                        from.SendLocalizedMessage(500471); // You skin it, and the hides are now in the corpse.
                    }
                }

                if (scales != 0)
                {
                    var sc = ScaleType;

                    switch (sc)
                    {
                        case ScaleType.Red:
                            {
                                corpse.AddCarvedItem(new RedScales(scales), from);
                                break;
                            }
                        case ScaleType.Yellow:
                            {
                                corpse.AddCarvedItem(new YellowScales(scales), from);
                                break;
                            }
                        case ScaleType.Black:
                            {
                                corpse.AddCarvedItem(new BlackScales(scales), from);
                                break;
                            }
                        case ScaleType.Green:
                            {
                                corpse.AddCarvedItem(new GreenScales(scales), from);
                                break;
                            }
                        case ScaleType.White:
                            {
                                corpse.AddCarvedItem(new WhiteScales(scales), from);
                                break;
                            }
                        case ScaleType.Blue:
                            {
                                corpse.AddCarvedItem(new BlueScales(scales), from);
                                break;
                            }
                        case ScaleType.All:
                            {
                                corpse.AddCarvedItem(new RedScales(scales), from);
                                corpse.AddCarvedItem(new YellowScales(scales), from);
                                corpse.AddCarvedItem(new BlackScales(scales), from);
                                corpse.AddCarvedItem(new GreenScales(scales), from);
                                corpse.AddCarvedItem(new WhiteScales(scales), from);
                                corpse.AddCarvedItem(new BlueScales(scales), from);
                                break;
                            }
                    }

                    from.SendMessage("You cut away some scales, but they remain on the corpse.");
                }

                corpse.Carved = true;

                if (corpse.IsCriminalAction(from))
                {
                    from.CriminalAction(true);
                }
            }
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(30); // version
            // version 30
            writer.Write(m_Level);
            // version 29
            writer.Write(SoulFeeds);
            writer.Write(m_SoulFeeder);
            // version 28
            writer.Write(m_NextGambleTime);
            writer.Write(m_GambleLosses);
            // version 27
            writer.Write(m_Feared);
            // version 26
            writer.Write(m_Blinded);
            // version 25
            writer.Write(m_NumberOfMinions); // generate spawner bottle neck fix
            // version 24
            writer.Write(m_Frozen);
            writer.Write(m_Burning);
            writer.Write(m_Toxic);
            writer.Write(m_Electrified);
            // version 23
            writer.Write(m_Illusionist);
            writer.Write(m_Corruptor);
            writer.Write(m_Corrupted);
            writer.Write(m_Ethereal);
            // version 22
            writer.Write(m_Veteran);
            writer.Write(m_Boss);
            writer.Write(m_Minion);
            writer.Write(m_MagicResistant);
            writer.Write(m_Regenerative);
            writer.Write(m_Reflective);
            writer.Write(m_Minions);
            // version 21
            writer.Write(CannibalPoints);
            writer.Write(m_Heroic);
            // version 20
            writer.Write(ExperienceValue);
            writer.Write(MaxLevel);
            writer.Write((int)m_CurrentAI);
            writer.Write((int)m_DefaultAI);

            writer.Write(RangePerception);
            writer.Write(RangeFight);

            writer.Write(m_Team);

            writer.Write(ActiveSpeed);
            writer.Write(PassiveSpeed);
            writer.Write(m_CurrentSpeed);

            writer.Write(m_Home.X);
            writer.Write(m_Home.Y);
            writer.Write(m_Home.Z);

            // Version 1
            writer.Write(RangeHome);

            // Version 2
            writer.Write((int)FightMode);

            writer.Write(m_Controlled);
            writer.Write(m_ControlMaster);
            writer.Write(ControlTarget);
            writer.Write(ControlDest);
            writer.Write((int)m_ControlOrder);
            writer.Write(MinTameSkill);
            // Removed in version 9
            // writer.Write( (double) m_dMaxTameSkill );
            writer.Write(m_bTamable);
            writer.Write(_summoned);

            if (_summoned)
            {
                writer.WriteDeltaTime(SummonEnd);
            }

            writer.Write(ControlSlots);

            // Version 3
            writer.Write(m_Loyalty);

            // Version 4
            writer.Write(CurrentWayPoint);

            // Verison 5
            writer.Write(m_SummonMaster);

            // Version 6
            writer.Write(HitsMaxSeed);
            writer.Write(StamMaxSeed);
            writer.Write(ManaMaxSeed);
            writer.Write(m_DamageMin);
            writer.Write(m_DamageMax);

            // Version 7
            writer.Write(m_PhysicalResistance);
            writer.Write(PhysicalDamage);

            writer.Write(m_FireResistance);
            writer.Write(FireDamage);

            writer.Write(m_ColdResistance);
            writer.Write(ColdDamage);

            writer.Write(m_PoisonResistance);
            writer.Write(PoisonDamage);

            writer.Write(m_EnergyResistance);
            writer.Write(EnergyDamage);

            // Version 8
            Owners.Tidy();
            writer.Write(Owners);

            // Version 10
            writer.Write(IsDeadPet);
            writer.Write(m_IsBonded);
            writer.Write(BondingBegin);
            writer.Write(OwnerAbandonTime);

            // Version 11
            writer.Write(m_HasGeneratedLoot);

            // Version 12
            writer.Write(m_Paragon);

            var hasFriends = Friends?.Count > 0;

            // Version 13
            writer.Write(hasFriends);

            if (hasFriends)
            {
                Friends.Tidy();
                writer.Write(Friends);
            }

            // Version 14
            writer.Write(RemoveIfUntamed);
            writer.Write(RemoveStep);

            // Version 17
            if (IsStabled || Controlled && ControlMaster != null)
            {
                writer.Write(TimeSpan.Zero);
            }
            else
            {
                writer.Write(DeleteTimeLeft);
            }

            // Version 18
            writer.Write(CorpseNameOverride);

            // Version 19
            writer.Write(HomeMap);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
            if (version >= 30)
            {
                m_Level = reader.ReadInt();
            }
            if (version >= 29)
            {
                SoulFeeds = reader.ReadInt();
                m_SoulFeeder = reader.ReadBool();
            }
            if (version >= 28)
            {
                m_NextGambleTime = reader.ReadDateTime();
                m_GambleLosses = reader.ReadInt();
            }
            if (version >= 27) {
                m_Feared = reader.ReadBool();
                if (m_Feared) {
                    Timer.StartTimer(TimeSpan.FromSeconds(Utility.Random(10)), UnFear);
                }
            }
            if (version >= 26)
            {
                m_Blinded = reader.ReadBool();
                if (m_Blinded) {
                    Timer.StartTimer(TimeSpan.FromSeconds(Utility.Random(10)), UnBlind);
                }
            }
            if (version >= 25)
            {
                m_NumberOfMinions = reader.ReadInt();
            } else
            {
                m_NumberOfMinions = 0;
            }
            if (version >= 24)
            {
                m_Frozen = reader.ReadBool();
                m_Burning = reader.ReadBool();
                m_Toxic = reader.ReadBool();
                m_Electrified = reader.ReadBool();
            } else
            {
                m_Frozen = false;
                m_Burning = false;
                m_Toxic = false;
                m_Electrified = false;
            }
            if (version >= 23)
            {
                m_Illusionist = reader.ReadBool();
                m_Corruptor = reader.ReadBool();
                m_Corrupted = reader.ReadBool();
                m_Ethereal = reader.ReadBool();
                MonsterBuff.CheckTimers(this);
            } else
            {
                m_Illusionist = false;
                m_Corruptor = false;
                m_Corrupted = false;
                m_Ethereal = false;
            }
            if (version >= 22)
            {
                m_Veteran = reader.ReadBool();
                m_Boss = reader.ReadBool();
                m_Minion = reader.ReadBool();
                m_MagicResistant = reader.ReadBool();
                m_Regenerative = reader.ReadBool();
                m_Reflective = reader.ReadBool();
                m_Minions = reader.ReadEntityList<Mobile>();
            } else
            {
                m_Veteran = false;
                m_Boss = false;
                m_Minion = false;
                m_MagicResistant = false;
                m_Regenerative = false;
                m_Reflective = false;
                m_Minions = new List<Mobile>();
            }
            if (version >= 21)
            {
                m_CannibalPoints = reader.ReadInt();
                m_Heroic = reader.ReadBool();
            }
            else
            {
                m_CannibalPoints = 0;
                m_Heroic = false;
            }

            if (version >= 20) {
                ExperienceValue = reader.ReadInt();
                MaxLevel = reader.ReadInt();
            }

            m_CurrentAI = (AIType)reader.ReadInt();
            m_DefaultAI = (AIType)reader.ReadInt();

            RangePerception = reader.ReadInt();
            RangeFight = reader.ReadInt();

            m_Team = reader.ReadInt();

            ActiveSpeed = reader.ReadDouble();
            PassiveSpeed = reader.ReadDouble();
            m_CurrentSpeed = reader.ReadDouble();

            if (RangePerception == OldRangePerception)
            {
                RangePerception = DefaultRangePerception;
            }

            m_Home.X = reader.ReadInt();
            m_Home.Y = reader.ReadInt();
            m_Home.Z = reader.ReadInt();

            if (version >= 1)
            {
                RangeHome = reader.ReadInt();

                if (version < 20)
                {
                    // Spell Attacks
                    var iCount = reader.ReadInt(); // Count
                    for (var i = 0; i < iCount; i++)
                    {
                        reader.ReadString(); // Spell Type
                    }

                    // Spell Defenses
                    iCount = reader.ReadInt(); // Count
                    for (var i = 0; i < iCount; i++)
                    {
                        reader.ReadString(); // Spell Type
                    }
                }
            }
            else
            {
                RangeHome = 0;
            }

            if (version >= 2)
            {
                FightMode = (FightMode)reader.ReadInt();

                m_Controlled = reader.ReadBool();
                m_ControlMaster = reader.ReadEntity<Mobile>();
                ControlTarget = reader.ReadEntity<Mobile>();
                ControlDest = reader.ReadPoint3D();
                m_ControlOrder = (OrderType)reader.ReadInt();

                MinTameSkill = reader.ReadDouble();

                if (version < 9)
                {
                    reader.ReadDouble();
                }

                m_bTamable = reader.ReadBool();
                _summoned = reader.ReadBool();

                if (_summoned)
                {
                    SummonEnd = reader.ReadDeltaTime();
                    new UnsummonTimer(this, SummonEnd - Core.Now).Start();
                }

                ControlSlots = reader.ReadInt();
            }
            else
            {
                FightMode = FightMode.Closest;

                m_Controlled = false;
                m_ControlMaster = null;
                ControlTarget = null;
                m_ControlOrder = OrderType.None;
            }

            if (version >= 3)
            {
                m_Loyalty = reader.ReadInt();
            }
            else
            {
                m_Loyalty = MaxLoyalty; // Wonderfully Happy
            }

            if (version >= 4)
            {
                CurrentWayPoint = reader.ReadEntity<WayPoint>();
            }

            if (version >= 5)
            {
                m_SummonMaster = reader.ReadEntity<Mobile>();
            }

            if (version >= 6)
            {
                HitsMaxSeed = reader.ReadInt();
                StamMaxSeed = reader.ReadInt();
                ManaMaxSeed = reader.ReadInt();
                m_DamageMin = reader.ReadInt();
                m_DamageMax = reader.ReadInt();
            }

            if (version >= 7)
            {
                m_PhysicalResistance = reader.ReadInt();
                PhysicalDamage = reader.ReadInt();

                m_FireResistance = reader.ReadInt();
                FireDamage = reader.ReadInt();

                m_ColdResistance = reader.ReadInt();
                ColdDamage = reader.ReadInt();

                m_PoisonResistance = reader.ReadInt();
                PoisonDamage = reader.ReadInt();

                m_EnergyResistance = reader.ReadInt();
                EnergyDamage = reader.ReadInt();
            }

            if (version >= 8)
            {
                Owners = reader.ReadEntityList<Mobile>();
            }
            else
            {
                Owners = new List<Mobile>();
            }

            if (version >= 10)
            {
                IsDeadPet = reader.ReadBool();
                m_IsBonded = reader.ReadBool();
                BondingBegin = reader.ReadDateTime();
                OwnerAbandonTime = reader.ReadDateTime();
            }

            if (version >= 11)
            {
                m_HasGeneratedLoot = reader.ReadBool();
            }
            else
            {
                m_HasGeneratedLoot = true;
            }

            if (version >= 12)
            {
                m_Paragon = reader.ReadBool();
            }
            else
            {
                m_Paragon = false;
            }

            if (version >= 13 && reader.ReadBool())
            {
                Friends = reader.ReadEntityList<Mobile>();
            }
            else if (version < 13 && m_ControlOrder >= OrderType.Unfriend)
            {
                ++m_ControlOrder;
            }

            if (version < 16 && Loyalty != MaxLoyalty)
            {
                Loyalty *= 10;
            }

            if (version >= 14)
            {
                RemoveIfUntamed = reader.ReadBool();
                RemoveStep = reader.ReadInt();
            }

            var deleteTime = TimeSpan.Zero;

            if (version >= 17)
            {
                deleteTime = reader.ReadTimeSpan();
            }

            if (deleteTime > TimeSpan.Zero || LastOwner != null && !Controlled && !IsStabled)
            {
                if (deleteTime == TimeSpan.Zero)
                {
                    deleteTime = TimeSpan.FromDays(3.0);
                }

                m_DeleteTimer = new DeleteTimer(this, deleteTime);
                m_DeleteTimer.Start();
            }

            if (version >= 18)
            {
                CorpseNameOverride = reader.ReadString();
            }

            if (version >= 19)
            {
                HomeMap = reader.ReadMap();
            }

            if (version <= 14 && m_Paragon && Hue == 0x31)
            {
                Hue = Paragon.Hue; // Paragon hue fixed, should now be 0x501.
            }

            if (Core.AOS && NameHue == 0x35)
            {
                NameHue = -1;
            }

            CheckStatTimers();

            ChangeAIType(m_CurrentAI);

            AddFollowers();

            if (IsAnimatedDead)
            {
                AnimateDeadSpell.Register(m_SummonMaster, this);
            }
        }

        public virtual bool IsHumanInTown() => Body.IsHuman && Region.IsPartOf<GuardedRegion>();

        public virtual bool CheckGold(Mobile from, Item dropped) => dropped is Gold gold && OnGoldGiven(from, gold);

        public virtual bool OnGoldGiven(Mobile from, Gold dropped)
        {
            if (CheckTeachingMatch(from))
            {
                if (Teach(m_Teaching, from, dropped.Amount, true))
                {
                    dropped.Delete();
                    return true;
                }
            }
            else if (IsHumanInTown())
            {
                Direction = GetDirectionTo(from);

                var oldSpeechHue = SpeechHue;

                SpeechHue = 0x23F;
                SayTo(from, "Thou art giving me gold?");

                SayTo(from, dropped.Amount >= 400 ? "'Tis a noble gift." : "Money is always welcome.");
                Deity.RewardPoints((PlayerMobile)from,
                    new[]
                    {
                        Utility.RandomMinMax(1, dropped.Amount/100),
                        Utility.RandomMinMax(-1, -(dropped.Amount/100))
                    }, new []
                    {
                        Deity.Alignment.Charity,
                        Deity.Alignment.Greed
                    });
                SpeechHue = 0x3B2;
                SayTo(from, 501548); // I thank thee.

                SpeechHue = oldSpeechHue;

                dropped.Delete();
                return true;
            }

            return false;
        }

        public virtual bool OverrideBondingReqs() => false;

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (CheckFeed(from, dropped))
            {
                return true;
            }

            if (CheckGold(from, dropped))
            {
                return true;
            }

            // Note: Yes, this happens for all questers (regardless of type, e.g. escorts),
            // even if they can't offer you anything at the moment
            if (MLQuestSystem.Enabled && CanGiveMLQuest && from is PlayerMobile mobile)
            {
                // You need to mark your quest items so I don't take the wrong object.  Then speak to me.
                MLQuestSystem.Tell(this, mobile, 1074893);
                return false;
            }

            return base.OnDragDrop(from, dropped);
        }

        public void ChangeAIType(AIType newAI)
        {
            AIObject?.m_Timer.Stop();

            if (ForcedAI != null)
            {
                AIObject = ForcedAI;
                return;
            }

            AIObject = newAI switch
            {
                AIType.AI_Melee   => new MeleeAI(this),
                AIType.AI_Animal  => new AnimalAI(this),
                AIType.AI_Berserk => new BerserkAI(this),
                AIType.AI_Archer  => new ArcherAI(this),
                AIType.AI_Healer  => new HealerAI(this),
                AIType.AI_Vendor  => new VendorAI(this),
                AIType.AI_Mage    => new MageAI(this),
                AIType.AI_Predator =>
                    // m_AI = new PredatorAI(this);
                    new MeleeAI(this),
                AIType.AI_Thief => new ThiefAI(this),
                _               => null
            };
        }

        public virtual void OnTeamChange()
        {
        }

        public override void RevealingAction()
        {
            InvisibilitySpell.StopTimer(this);

            base.RevealingAction();
        }

        public void RemoveFollowers()
        {
            if (m_ControlMaster != null)
            {
                m_ControlMaster.Followers -= ControlSlots;
                if (m_ControlMaster is PlayerMobile mobile)
                {
                    mobile.AllFollowers.Remove(this);
                    if (mobile.AutoStabled.Contains(this))
                    {
                        mobile.AutoStabled.Remove(this);
                    }
                }
            }
            else if (m_SummonMaster != null)
            {
                m_SummonMaster.Followers -= ControlSlots;
                (m_SummonMaster as PlayerMobile)?.AllFollowers.Remove(this);
            }

            if (m_ControlMaster?.Followers < 0)
            {
                m_ControlMaster.Followers = 0;
            }

            if (m_SummonMaster?.Followers < 0)
            {
                m_SummonMaster.Followers = 0;
            }
        }

        public void AddFollowers()
        {
            if (m_ControlMaster != null)
            {
                m_ControlMaster.Followers += ControlSlots;
                if (m_ControlMaster is PlayerMobile mobile)
                {
                    mobile.AllFollowers.Add(this);
                }
            }
            else if (m_SummonMaster != null)
            {
                m_SummonMaster.Followers += ControlSlots;
                if (m_SummonMaster is PlayerMobile mobile)
                {
                    mobile.AllFollowers.Add(this);
                }
            }
        }

        public virtual void OnGotMeleeAttack(Mobile attacker, int damage)
        {
            if (AutoDispel && attacker is BaseCreature creature && creature.IsDispellable &&
                AutoDispelChance > Utility.RandomDouble())
            {
                Dispel(creature);
            }

            TriggerAbility(MonsterAbilityTrigger.TakeMeleeDamage, attacker);
        }

        public override bool Move(Direction d)
        {
            if (!base.Move(d))
            {
                return false;
            }

            TriggerAbilityMove(MonsterAbilityTrigger.Movement, this, d);
            return true;
        }

        public virtual void Dispel(Mobile m)
        {
            Effects.SendLocationParticles(
                EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration),
                0x3728,
                8,
                20,
                5042
            );
            Effects.PlaySound(m, 0x201);

            m.Delete();
        }

        public virtual void OnGaveMeleeAttack(Mobile defender, int damage)
        {
            var p = m_Paragon ? PoisonImpl.IncreaseLevel(HitPoison) : HitPoison;

            if (p != null && HitPoisonChance >= Utility.RandomDouble())
            {
                defender.ApplyPoison(this, p);

                if (Controlled)
                {
                    CheckSkill(SkillName.Poisoning, 0, Skills.Poisoning.Cap);
                }
            }

            if (AutoDispel && defender is BaseCreature creature && creature.IsDispellable &&
                AutoDispelChance > Utility.RandomDouble())
            {
                Dispel(creature);
            }

            TriggerAbility(MonsterAbilityTrigger.GiveMeleeDamage, defender);

            MonsterBuff.CheckElementalAttack(this, defender);

        }

        public override void OnAfterDelete()
        {
            if (AIObject != null)
            {
                AIObject.m_Timer?.Stop();
                AIObject = null;
            }

            if (m_DeleteTimer != null)
            {
                m_DeleteTimer.Stop();
                m_DeleteTimer = null;
            }

            FocusMob = null;

            if (IsAnimatedDead)
            {
                AnimateDeadSpell.Unregister(m_SummonMaster, this);
            }

            if (MLQuestSystem.Enabled)
            {
                MLQuestSystem.HandleDeletion(this);
            }

            base.OnAfterDelete();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DebugSay(string text)
        {
            // Moved the debug check to implementation layer so we can avoid string formatting when we do not need it
            // PublicOverheadMessage(MessageType.Regular, 41, false, text);
        }

        /*
         * This function can be overridden.. so a "Strongest" mobile, can have a different definition depending
         * on who check for value
         * -Could add a FightMode.Preferred
         *
         */

        public virtual double GetFightModeRanking(Mobile m, FightMode acqType, bool bPlayerOnly)
        {
            if (bPlayerOnly && !m.Player)
            {
                return double.MinValue;
            }

            return acqType switch
            {
                FightMode.Strongest => m.Skills.Tactics.Value + m.Str, // returns strongest mobile
                FightMode.Weakest   => -m.Hits,                        // returns weakest mobile
                _                   => -GetDistanceToSqrt(m)
            };
        }

        // Turn, - for left, + for right
        // Basic for now, needs work
        public virtual void Turn(int iTurnSteps)
        {
            var v = (int)Direction;

            Direction = (Direction)((((v & 0x7) + iTurnSteps) & 0x7) | (v & 0x80));
        }

        public virtual void TurnInternal(int iTurnSteps)
        {
            var v = (int)Direction;

            SetDirection((Direction)((((v & 0x7) + iTurnSteps) & 0x7) | (v & 0x80)));
        }

        public bool IsHurt() => Hits != HitsMax;

        public double GetHomeDistance() => GetDistanceToSqrt(m_Home);

        public virtual int GetTeamSize(int iRange)
        {
            var iCount = 0;

            foreach (var m in GetMobilesInRange(iRange))
            {
                if (m != this && m is BaseCreature creature && !creature.Deleted && creature.Team == Team &&
                    CanSee(creature))
                {
                    iCount++;
                }
            }

            return iCount;
        }

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            if (aggressor is PlayerMobile player && IsFriend(aggressor))
            {
                player.CombatAlignment = Deity.Alignment.None;
                Timer.StartTimer(TimeSpan.FromMinutes(15), player.ResetCombatAlignment);
                player.SendMessage("You have committed an aggressive action towards an aligned creature! Your alignment is temporarily removed.");
            }

            if (ControlMaster != null && NotorietyHandlers.CheckAggressor(ControlMaster.Aggressors, aggressor))
            {
                aggressor.Aggressors.Add(AggressorInfo.Create(this, aggressor, true));
            }

            var ct = m_ControlOrder;

            if (AIObject != null)
            {
                if (!Core.ML || ct != OrderType.Follow && ct != OrderType.Stop && ct != OrderType.Stay)
                {
                    AIObject.OnAggressiveAction(aggressor);
                }
                else
                {
                    DebugSay("I'm being attacked but my master told me not to fight.");
                    Warmode = false;
                    return;
                }
            }

            StopFlee();

            ForceReacquire();

            if (!IsEnemy(aggressor))
            {
                var pl = Ethics.Player.Find(aggressor, true);

                if (pl?.IsShielded == true)
                {
                    pl.FinishShield();
                }
            }

            if (aggressor.ChangingCombatant && (m_Controlled || _summoned) &&
                (ct == OrderType.Come || !Core.ML && ct == OrderType.Stay || ct is OrderType.Stop or OrderType.None or OrderType.Follow))
            {
                ControlTarget = aggressor;
                ControlOrder = OrderType.Attack;
            }
            else if (Combatant == null && !BardPacified)
            {
                Warmode = true;
                Combatant = aggressor;
            }
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m is BaseCreature creature && !creature.Controlled)
            {
                return !Alive || !creature.Alive || IsDeadBondedPet || creature.IsDeadBondedPet ||
                       Hidden && AccessLevel > AccessLevel.Player;
            }

            if (Region.IsPartOf<SafeZone>() && m is PlayerMobile pm &&
                (pm.DuelContext?.Started != true || pm.DuelContext.Finished ||
                 pm.DuelPlayer?.Eliminated != false))
            {
                return true;
            }

            return base.OnMoveOver(m);
        }

        public virtual void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (Commandable)
            {
                AIObject?.GetContextMenuEntries(from, list);
            }

            if (m_bTamable && !m_Controlled && from.Alive)
            {
                list.Add(new TameEntry(from, this));
            }

            AddCustomContextEntries(from, list);

            /**if (CanTeach && from.Alive)
            {
                var ourSkills = Skills;
                var theirSkills = from.Skills;

                for (var i = 0; i < ourSkills.Length && i < theirSkills.Length; ++i)
                {
                    var skill = ourSkills[i];
                    var theirSkill = theirSkills[i];

                    if (skill?.Base >= 60.0 && CheckTeach(skill.SkillName, from))
                    {
                        var toTeach = skill.BaseFixedPoint / 3;

                        if (toTeach > 420)
                        {
                            toTeach = 420;
                        }

                        list.Add(new TeachEntry((SkillName)i, this, from, toTeach > theirSkill.BaseFixedPoint));
                    }
                }
            }**/
        }

        public override bool HandlesOnSpeech(Mobile from) =>
            (SpeechType?.Flags & IHSFlags.OnSpeech) != 0 && from.InRange(this, 3) ||
            AIObject?.HandlesOnSpeech(from) == true && from.InRange(this, RangePerception);

        public override void OnSpeech(SpeechEventArgs e)
        {
            var speechType = SpeechType;

            if (speechType?.OnSpeech(this, e.Mobile, e.Speech) == true)
            {
                e.Handled = true;
            }
            else if (!e.Handled && AIObject != null && e.Mobile.InRange(this, RangePerception))
            {
                AIObject.OnSpeech(e);
            }
        }

        public override bool IsHarmfulCriminal(Mobile target) =>
            (!Controlled || target != m_ControlMaster) && (!Summoned || target != m_SummonMaster) &&
            (target is not BaseCreature { InitialInnocent: true } creature || creature.Controlled) &&
            (target is not PlayerMobile mobile || mobile.PermaFlags.Count <= 0) && base.IsHarmfulCriminal(target);

        public override void CriminalAction(bool message)
        {
            base.CriminalAction(message);

            if (Controlled || Summoned)
            {
                if (m_ControlMaster?.Player == true)
                {
                    m_ControlMaster.CriminalAction(false);
                }
                else if (m_SummonMaster?.Player == true)
                {
                    m_SummonMaster.CriminalAction(false);
                }
            }
        }

        public override void DoHarmful(Mobile target, bool indirect = false)
        {
            base.DoHarmful(target, indirect);

            if (target == this || target == m_ControlMaster || target == m_SummonMaster || !Controlled && !Summoned)
            {
                return;
            }

            var list = Aggressors;

            for (var i = 0; i < list.Count; ++i)
            {
                var ai = list[i];

                if (ai.Attacker == target)
                {
                    return;
                }
            }

            list = Aggressed;

            for (var i = 0; i < list.Count; ++i)
            {
                var ai = list[i];

                if (ai.Defender == target)
                {
                    if (m_ControlMaster?.Player == true && m_ControlMaster.CanBeHarmful(target, false))
                    {
                        m_ControlMaster.DoHarmful(target, true);
                    }
                    else if (m_SummonMaster?.Player == true && m_SummonMaster.CanBeHarmful(target, false))
                    {
                        m_SummonMaster.DoHarmful(target, true);
                    }

                    return;
                }
            }
        }

        public void ReleaseGuardDupeLock()
        {
            m_NoDupeGuards = null;
        }

        public void ReleaseGuardLock()
        {
            EndAction<GuardedRegion>();
        }

        public virtual bool CheckIdle()
        {
            if (Combatant != null)
            {
                return false; // in combat.. not idling
            }

            if (m_IdleReleaseTime > DateTime.MinValue)
            {
                // idling...
                if (Core.Now >= m_IdleReleaseTime)
                {
                    m_IdleReleaseTime = DateTime.MinValue;
                    return false; // idle is over
                }

                return true; // still idling
            }

            if (Utility.Random(100) < 95)
            {
                return false; // not idling, but don't want to enter idle state
            }

            var idleSeconds = Utility.RandomMinMax(NPCSpeeds.MinIdleSeconds, NPCSpeeds.MaxIdleSeconds);
            m_IdleReleaseTime = Core.Now + TimeSpan.FromSeconds(idleSeconds);

            if (Body.IsHuman)
            {
                switch (Utility.Random(2))
                {
                    case 0:
                        {
                            CheckedAnimate(5, 5, 1, true, true, 1);
                            break;
                        }
                    case 1:
                        {
                            CheckedAnimate(6, 5, 1, true, false, 1);
                            break;
                        }
                }
            }
            else if (Body.IsAnimal)
            {
                switch (Utility.Random(3))
                {
                    case 0:
                        {
                            CheckedAnimate(3, 3, 1, true, false, 1);
                            break;
                        }
                    case 1:
                        {
                            CheckedAnimate(9, 5, 1, true, false, 1);
                            break;
                        }
                    case 2:
                        {
                            CheckedAnimate(10, 5, 1, true, false, 1);
                            break;
                        }
                }
            }
            else if (Body.IsMonster)
            {
                switch (Utility.Random(2))
                {
                    case 0:
                        {
                            CheckedAnimate(17, 5, 1, true, false, 1);
                            break;
                        }
                    case 1:
                        {
                            CheckedAnimate(18, 5, 1, true, false, 1);
                            break;
                        }
                }
            }

            PlaySound(GetIdleSound());
            return true; // entered idle state
        }

        /*
          this way, due to the huge number of locations this will have to be changed
          Perhaps we can change this in the future when fixing game play is not the
          major issue.
        */

        public virtual void CheckedAnimate(int action, int frameCount, int repeatCount, bool forward, bool repeat, int delay)
        {
            if (!Mounted)
            {
                Animate(action, frameCount, repeatCount, forward, repeat, delay);
            }
        }

        private void CheckAIActive()
        {
            var map = Map;

            if (PlayerRangeSensitive && AIObject != null && map?.GetSector(Location).Active == true)
            {
                AIObject.Activate();
            }
        }

        public override void OnCombatantChange()
        {
            base.OnCombatantChange();

            Warmode = Combatant?.Deleted == false && Combatant.Alive;

            if (CanFly && Warmode)
            {
                Flying = false;
            }
            StartBoss();
        }

        protected override void OnMapChange(Map oldMap)
        {
            CheckAIActive();

            base.OnMapChange(oldMap);
        }

        protected override void OnLocationChange(Point3D oldLocation)
        {
            CheckAIActive();

            base.OnLocationChange(oldLocation);
        }

        public virtual void ForceReacquire()
        {
            NextReacquireTime = Core.TickCount;
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (AcquireOnApproach && !Controlled && !Summoned && FightMode != FightMode.Aggressor)
            {
                if (InRange(m.Location, AcquireOnApproachRange) && !InRange(oldLocation, AcquireOnApproachRange) &&
                    CanBeHarmful(m) && IsEnemy(m))
                {
                    Combatant = FocusMob = m;
                    AIObject?.MoveTo(m, true, 1);
                    DoHarmful(m);
                }
            }
            else if (ReacquireOnMovement)
            {
                ForceReacquire();
            }

            var speechType = SpeechType;

            speechType?.OnMovement(this, m, oldLocation);

            /* Begin notice sound */
            if ((!m.Hidden || m.AccessLevel == AccessLevel.Player) && m.Player && FightMode != FightMode.Aggressor &&
                FightMode != FightMode.None && Combatant == null && !Controlled && !Summoned &&
                InRange(m.Location, 18) && !InRange(oldLocation, 18))
            {
                if (Body.IsMonster)
                {
                    Animate(11, 5, 1, true, false, 1);
                }

                PlaySound(GetAngerSound());
            }
            /* End notice sound */

            if (MLQuestSystem.Enabled && CanShout && m is PlayerMobile mobile)
            {
                CheckShout(mobile, oldLocation);
            }

            if (m_NoDupeGuards == m)
            {
                return;
            }

            if (!Body.IsHuman || Kills >= 5 || AlwaysMurderer || AlwaysAttackable || m.Kills < 5 ||
                !m.InRange(Location, 12) || !m.Alive)
            {
                return;
            }

            var guardedRegion = Region.GetRegion<GuardedRegion>();

            if (guardedRegion?.IsDisabled() == false && guardedRegion.IsGuardCandidate(m) && BeginAction<GuardedRegion>())
            {
                Say(1013037 + Utility.Random(16));
                guardedRegion.CallGuards(Location);

                Timer.StartTimer(TimeSpan.FromSeconds(5.0), ReleaseGuardLock);

                m_NoDupeGuards = m;
                Timer.StartTimer(ReleaseGuardDupeLock);
            }
        }
        public void StartBoss()
        {
            if (IsBoss)
            {
                if (m_NumberOfMinions <= 0)
                {
                    m_NumberOfMinions = Utility.Random(4, 6);
                }
                if (Minions.Count == 0)
                {
                    MonsterBuff.SpawnMinions(this);
                }
                InitBossTimer();
            }
        }

        public override void Attack(Mobile m)
        {
            StartBoss();
            base.Attack(m);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel >= AccessLevel.GameMaster && !Body.IsHuman)
            {
                var pack = Backpack;

                pack?.DisplayTo(from);
            }

            if (DeathAdderCharmable && from.CanBeHarmful(this, false))
            {
                if (SummonFamiliarSpell.Table.TryGetValue(from, out var bc) && (bc as DeathAdder)?.Deleted == false)
                {
                    from.SendAsciiMessage("You charm the snake.  Select a target to attack.");
                    from.Target = new DeathAdderCharmTarget(this);
                }
            }

            if (MLQuestSystem.Enabled && CanGiveMLQuest && from is PlayerMobile mobile)
            {
                MLQuestSystem.OnDoubleClick(this, mobile);
            }

            base.OnDoubleClick(from);
        }

        public override void AddNameProperties(IPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1114057, $"Level: {Level.ToString()}");          // ~1_val~
            list.Add(1114057, $"Alignment: {Deity.GetCreatureAlignment(GetType()).ToString()}");   // ~1_val~
            list.Add(1114057, $"Level range: {(Level - 5 < 1 ? 1 : Level - 5).ToString()} - {MaxLevel.ToString()}");   // ~1_val~
            list.Add(1114057, $"XP: {FinalExperience().ToString()}"); // ~1_val~
            list.Add(1114057, $"Mana: {Mana.ToString()}/{ManaMax.ToString()}"); // ~1_val~
            list.Add(1114057, $"Hits: {Hits.ToString()}/{HitsMax.ToString()}"); // ~1_val~
            list.Add(1114057, $"Stamina: {Stam.ToString()}/{StamMax.ToString()}"); // ~1_val~

            if (IsSoulFeeder)
            {
                list.Add(1114057, "Soul Feeder");          // ~1_val~
            }
            if (IsBoss)
            {
                list.Add(1114057, "Boss");      // ~1_val~
            }
            else if (IsMinion)
            {
                list.Add(1114057, "Minion");               // ~1_val~
            }

            if (IsMagicResistant)
            {
                list.Add(1114057, "Magic Resistant");                      // ~1_val~
            }

            if (IsReflective)
            {
                list.Add(1114057, "Reflective");        // ~1_val~
            }

            if (IsRegenerative)
            {
                list.Add(1114057, "Regenerative");               // ~1_val~
            }

            if (IsIllusionist)
            {
                list.Add(1114057, "Illusionist");            // ~1_val~
            }

            if (IsCorruptor)
            {
                list.Add(1114057, "Corruptor");           // ~1_val~
            }
            else if (IsCorrupted)
            {
                list.Add(1114057, "Corrupted");             // ~1_val~
            }

            if (IsEthereal)
            {
                list.Add(1114057, "Ethereal");            // ~1_val~
            }

            if (IsBurning)
            {
                list.Add(1114057, "Burning");            // ~1_val~
            }
            else if (IsFrozen)
            {
                list.Add(1114057, "Frozen");            // ~1_val~
            }
            else if (IsElectrified)
            {
                list.Add(1114057, "Electrified");                  // ~1_val~
            }
            else if (IsToxic)
            {
                list.Add(1114057, "Toxic");       // ~1_val~
            }

            if (MLQuestSystem.Enabled && CanGiveMLQuest)
            {
                list.Add(1072269); // Quest Giver
            }

            if (Core.ML)
            {
                if (DisplayWeight)
                {
                    list.Add(TotalWeight == 1 ? 1072788 : 1072789, TotalWeight); // Weight: ~1_WEIGHT~ stones
                }

                if (m_ControlOrder == OrderType.Guard)
                {
                    list.Add(1080078); // guarding
                }
            }

            if (Summoned && !(IsAnimatedDead || IsNecroFamiliar || this is Clone))
            {
                list.Add(1049646); // (summoned)
            }
            else if (Controlled && Commandable)
            {
                // Intentional difference (showing ONLY bonded when bonded instead of bonded & tame)
                if (IsBonded)
                {
                    list.Add(1049608); // (bonded)
                }
                else
                {
                    list.Add(502006); // (tame)
                }
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            if (Controlled && Commandable)
            {
                int number;

                if (Summoned)
                {
                    number = 1049646; // (summoned)
                }
                else if (IsBonded)
                {
                    number = 1049608; // (bonded)
                }
                else
                {
                    number = 502006; // (tame)
                }

                PrivateOverheadMessage(MessageType.Regular, 0x3B2, number, from.NetState);
            }

            base.OnSingleClick(from);
        }

        public override bool OnBeforeDeath()
        {
            TriggerAbility(MonsterAbilityTrigger.Death, null);

            var treasureLevel = TreasureMapLevel;

            if (treasureLevel == 1 && Map == Map.Trammel && TreasureMap.IsInHavenIsland(this))
            {
                var killer = LastKiller;

                if (killer is BaseCreature bc)
                {
                    killer = bc.GetMaster();
                }

                if (killer is PlayerMobile mobile && mobile.Young)
                {
                    treasureLevel = 0;
                }
            }

            if (!Summoned && !NoKillAwards && !IsBonded)
            {
                if (treasureLevel >= 0)
                {
                    if (m_Paragon && Paragon.ChestChance > Utility.RandomDouble())
                    {
                        PackItem(new ParagonChest(Name, treasureLevel));
                    }
                    else if ((Map == Map.Felucca || Map == Map.Trammel) && Utility.RandomDouble() < TreasureMap.LootChance)
                    {
                        PackItem(new TreasureMap(treasureLevel, Map));
                    }
                }

                if (m_Paragon && Paragon.ChocolateIngredientChance > Utility.RandomDouble())
                {
                    switch (Utility.Random(4))
                    {
                        case 0:
                            {
                                PackItem(new CocoaButter());
                                break;
                            }
                        case 1:
                            {
                                PackItem(new CocoaLiquor());
                                break;
                            }
                        case 2:
                            {
                                PackItem(new SackOfSugar());
                                break;
                            }
                        case 3:
                            {
                                PackItem(new Vanilla());
                                break;
                            }
                    }
                }
            }
            if (Utility.Random(4200 - (int)DynamicExperienceValue()) < 1 && DynamicExperienceValue() >= 1200) {
                RuneWord runeWord = new RuneWord();
                PackItem(runeWord);
            }
            if (Utility.Random(2200 - (int)DynamicExperienceValue()) < 1 && MonsterBuff.CanDropShards(this))
            {
                if (IsFrozen)
                {
                    FrozenShard shard = new FrozenShard();
                    PackItem(shard);
                }
                else if (IsBurning)
                {
                    BurningShard shard = new BurningShard();
                    PackItem(shard);
                }
                else if (IsElectrified)
                {
                    ElectrifiedShard shard = new ElectrifiedShard();
                    PackItem(shard);
                }
                else if (IsToxic)
                {
                    ToxicShard shard = new ToxicShard();
                    PackItem(shard);
                }
            }

            if (!Summoned && !NoKillAwards && !m_HasGeneratedLoot)
            {
                m_HasGeneratedLoot = true;
                GenerateLoot(false);
            }

            if (!NoKillAwards && Region.IsPartOf("Doom"))
            {
                var bones = TheSummoningQuest.GetDaemonBonesFor(this);

                if (bones > 0)
                {
                    PackItem(new DaemonBone(bones));
                }
            }

            if (IsAnimatedDead)
            {
                Effects.SendLocationEffect(Location, Map, 0x3728, 13, 1, 0x461, 4);
            }

            var speechType = SpeechType;
            speechType?.OnDeath(this);
            ReceivedHonorContext?.OnTargetKilled();

            return base.OnBeforeDeath();
        }

        public int ComputeBonusDamage(List<DamageEntry> list, Mobile m)
        {
            var bonus = 0;

            for (var i = list.Count - 1; i >= 0; --i)
            {
                var de = list[i];

                if (de.Damager == m || de.Damager is not BaseCreature bc)
                {
                    continue;
                }

                if (bc.GetMaster() == m)
                {
                    bonus += de.DamageGiven;
                }
            }

            return bonus;
        }

        public Mobile GetMaster()
        {
            if (Controlled && ControlMaster != null)
            {
                return ControlMaster;
            }

            if (Summoned && SummonMaster != null)
            {
                return SummonMaster;
            }

            return null;
        }

        public virtual bool IsMonster => !Controlled || (GetMaster() as BaseCreature)?.IsMonster == true;

        public bool InActivePVPCombat() =>
            ControlOrder != OrderType.Follow &&
            Combatant is PlayerMobile ||
            Combatant is BaseCreature { Controlled: true } bc && bc.GetMaster() is PlayerMobile;

        public static List<DamageStore> GetLootingRights(List<DamageEntry> damageEntries, int hitsMax)
        {
            var rights = new List<DamageStore>();
            DamageStore firstDamager = null;

            for (var i = damageEntries.Count - 1; i >= 0; --i)
            {
                if (i >= damageEntries.Count)
                {
                    continue;
                }

                var de = damageEntries[i];

                if (de.HasExpired)
                {
                    damageEntries.RemoveAt(i);
                    continue;
                }

                var damage = de.DamageGiven;

                var respList = de.Responsible;

                for (var j = 0; j < respList?.Count; ++j)
                {
                    var subEntry = respList[j];
                    var master = subEntry.Damager;

                    if (master?.Deleted != false || !master.Player)
                    {
                        continue;
                    }

                    var needNewSubEntry = true;

                    for (var k = 0; needNewSubEntry && k < rights.Count; ++k)
                    {
                        var ds = rights[k];

                        if (ds.m_Mobile == master)
                        {
                            ds.m_Damage += subEntry.DamageGiven;
                            needNewSubEntry = false;
                            firstDamager = ds;
                        }
                    }

                    if (needNewSubEntry)
                    {
                        var ds = new DamageStore(master, subEntry.DamageGiven);
                        rights.Add(ds);
                        firstDamager = ds;
                    }

                    damage -= subEntry.DamageGiven;
                }

                var m = de.Damager;

                if (m is not { Deleted: false, Player: true })
                {
                    continue;
                }

                if (damage <= 0)
                {
                    continue;
                }

                var needNewEntry = true;

                for (var j = 0; needNewEntry && j < rights.Count; ++j)
                {
                    var ds = rights[j];

                    if (ds.m_Mobile == m)
                    {
                        ds.m_Damage += damage;
                        needNewEntry = false;
                        firstDamager = ds;
                    }
                }

                if (needNewEntry)
                {
                    var ds = new DamageStore(m, damage);
                    rights.Add(ds);
                    firstDamager = ds;
                }
            }

            // Handle damage rights per Five on Friday: https://www.uoguide.com/Five_on_Friday_-_January_19,_2007
            if (rights.Count > 0)
            {
                if (firstDamager != null)
                {
                    firstDamager.m_Damage = (int)(firstDamager.m_Damage * 1.25);
                }

                if (rights.Count > 1)
                {
                    rights.Sort(); // Sort by damage
                }

                var topDamage = rights[0].m_Damage;

                int minDamage = hitsMax switch
                {
                    >= 3000 => topDamage / 16,
                    >= 1000 => topDamage / 8,
                    >= 200  => topDamage / 4,
                    _       => topDamage / 2
                };

                for (var i = 0; i < rights.Count; ++i)
                {
                    var ds = rights[i];

                    ds.m_HasRight = ds.m_Damage >= minDamage;
                }
            }

            return rights;
        }

        public virtual void OnKilledBy(Mobile mob)
        {
            if (GivesMLMinorArtifact)
            {
                if (MondainsLegacy.CheckArtifactChance(mob, this))
                {
                    MondainsLegacy.GiveArtifactTo(mob);
                }
            }
            else if (m_Paragon)
            {
                if (Paragon.CheckArtifactChance(mob, this))
                {
                    Paragon.GiveArtifactTo(mob);
                }
            }
        }

        public int FinalExperience()
        {
            int baseXp = (int)DynamicExperienceValue() + ExperienceValue;
            int scaleXp = ExperienceScale(baseXp);
            if (ExperienceValue == 0)
            {
                scaleXp = 0;
            }

            // int challengeRating = baseXp / 100;
            // scaleXp += (challengeRating + challengeRating / 7) * Level;
            return scaleXp;
        }

        public double ExperienceCompareScale(double xpScale, double xpFixed, int percent)
        {
            xpScale += AOS.Scale((int)xpScale, percent) > xpFixed ? AOS.Scale((int)xpScale, percent) : xpFixed;
            return xpScale;
        }

        public int ExperienceScale(int xp)
        {
            double xpScale = 0;
            int tier = 1;
            while (xp > 0)
            {
                double tierXp = xp;
                if (xp > 500)
                {
                    tierXp = 500;
                }

                xpScale += tier switch
                {
                    1 => tierXp / 9,
                    2 => tierXp / 6,
                    3 => tierXp / 2.5,
                    4 => tierXp / 1.65,
                    5 => tierXp,
                    6 => tierXp / 0.6,
                    7 => tierXp / 0.45,
                    _ => tierXp / 0.25
                };
                xp -= 500;
                tier++;
            }


            if (IsParagon)
            {
                xpScale = ExperienceCompareScale(xpScale, 200, 30);
            }

            if (IsHeroic)
            {
                xpScale = ExperienceCompareScale(xpScale, 125, 15);
            }

            if (IsVeteran)
            {
                xpScale = ExperienceCompareScale(xpScale, 33, 5);
            }

            if (IsReflective)
            {
                xpScale = ExperienceCompareScale(xpScale, 100, 15);
            }

            if (IsRegenerative)
            {
                xpScale = ExperienceCompareScale(xpScale, 100, 15);
            }

            if (IsMagicResistant)
            {
                xpScale = ExperienceCompareScale(xpScale, 100, 15);
            }

            if (IsCorruptor)
            {
                xpScale = ExperienceCompareScale(xpScale, 125, 15);
            } else if (IsCorrupted)
            {
                xpScale = ExperienceCompareScale(xpScale, 25, 3);
            }

            if (IsEthereal)
            {
                xpScale = ExperienceCompareScale(xpScale, 75, 10);
            }

            if (IsIllusionist)
            {
                xpScale = ExperienceCompareScale(xpScale, 75, 10);
            }

            if (IsBoss)
            {
                xpScale = ExperienceCompareScale(xpScale, 125, 20);
            }

            if (IsMinion)
            {
                xpScale = ExperienceCompareScale(xpScale, 25, 3);
            }

            if (IsSoulFeeder)
            {
                xpScale = ExperienceCompareScale(xpScale, 50, 7);
            }

            if (IsBurning || IsFrozen || IsElectrified || IsToxic)
            {
                xpScale = ExperienceCompareScale(xpScale, 75, 10);
            }

            return (int)xpScale;
        }


        public double DynamicExperienceValue()
        {
            var xp = (RawStr > RawDex && RawStr > RawInt ? RawStr * 1.8 : RawStr) + (RawDex > RawStr && RawDex > RawInt ? RawDex * 1.8 : RawDex) + (RawInt > RawStr && RawInt > RawDex ? RawInt * 1.8 : RawInt);

            xp += (m_PhysicalResistance > 45 ? m_PhysicalResistance * 1.6 : m_PhysicalResistance);
            xp += (m_FireResistance > 45 ? m_FireResistance * 1.6 : m_FireResistance);
            xp += (m_ColdResistance > 45 ? m_ColdResistance * 1.6 : m_ColdResistance);
            xp += (m_PoisonResistance > 45 ? m_PoisonResistance * 1.6 : m_PoisonResistance);
            xp += (m_EnergyResistance > 45 ? m_EnergyResistance * 1.6 : m_EnergyResistance);

            xp += SkillsTotal / 10.0;

            xp += m_DamageMax * 4.5;
            xp += m_DamageMin * 4.5;

            xp += VirtualArmor * 2.5;

            xp += Fame / 100.00;

            if (BaseInstrument.IsMageryCreature(this))
            {
                xp += Skills.Magery.Base;
            }

            if (Skills.Necromancy.Base > 5.0)
            {
                xp += Skills.Necromancy.Base;
            }

            if (Skills.Tactics.Base >= 90.0)
            {
                xp += Skills.Tactics.Base;
            }

            if (BaseInstrument.IsFireBreathingCreature(this))
            {
                xp += AOS.Scale(HitsMax, 33);
            }

            if (this is VampireBat || this is VampireBatFamiliar)
            {
                xp += 100;
            }

            xp += BaseInstrument.GetPoisonLevel(this) * 33;

            if (BaseInstrument.IsPoisonImmune(this) && xp >= 1200)
            {
                xp += PoisonImmune.Level * 33;
            }

            if (xp > 5000)
            {
                xp = 5000;
            }

            return xp;
        }

        public override void OnDeath(Container c)
        {
            MeerMage.StopEffect(this, false);
            if (IsBonded)
            {
                Effects.PlaySound(this, GetDeathSound());

                Warmode = false;

                Poison = null;
                Combatant = null;

                Hits = 0;
                Stam = 0;
                Mana = 0;

                IsDeadPet = true;
                ControlTarget = ControlMaster;
                ControlOrder = OrderType.Follow;

                ProcessDeltaQueue();
                SendIncomingPacket();
                SendIncomingPacket();

                // TODO: This can be done in Parallel if there are lots of them.
                var aggressors = Aggressors;

                for (var i = 0; i < aggressors.Count; ++i)
                {
                    var info = aggressors[i];

                    if (info.Attacker.Combatant == this)
                    {
                        info.Attacker.Combatant = null;
                    }
                }

                var aggressed = Aggressed;

                for (var i = 0; i < aggressed.Count; ++i)
                {
                    var info = aggressed[i];

                    if (info.Defender.Combatant == this)
                    {
                        info.Defender.Combatant = null;
                    }
                }

                var owner = ControlMaster;

                if (owner?.Deleted != false || owner.Map != Map || !owner.InRange(this, 12) || !CanSee(owner) ||
                    !InLOS(owner))
                {
                    if (OwnerAbandonTime == DateTime.MinValue)
                    {
                        OwnerAbandonTime = Core.Now;
                    }
                }
                else
                {
                    OwnerAbandonTime = DateTime.MinValue;
                }

                GiftOfLifeSpell.HandleDeath(this);

                CheckStatTimers();
                return;
            }

            if (!Summoned && !NoKillAwards)
            {
                var totalFame = Fame / 100;
                var totalKarma = -Karma / 100;

                if (Map == Map.Felucca)
                {
                    totalFame += totalFame / 10 * 3;
                    totalKarma += totalKarma / 10 * 3;
                }

                var list = GetLootingRights(DamageEntries, HitsMax);
                var titles = new List<Mobile>();
                var damage = new List<int>();
                var fame = new List<int>();
                var karma = new List<int>();

                var givenQuestKill = false;
                var givenFactionKill = false;
                var givenToTKill = false;

                for (var i = 0; i < list.Count; ++i)
                {
                    var ds = list[i];

                    if (!ds.m_HasRight)
                    {
                        continue;
                    }

                    var party = Engines.PartySystem.Party.Get(ds.m_Mobile);

                    if (party != null)
                    {
                        var divedFame = totalFame / party.Members.Count;
                        var divedKarma = totalKarma / party.Members.Count;

                        for (var j = 0; j < party.Members.Count; ++j)
                        {
                            var info = party.Members[j];

                            if (info?.Mobile != null)
                            {
                                var index = titles.IndexOf(info.Mobile);

                                if (index == -1)
                                {
                                    titles.Add(info.Mobile);
                                    fame.Add(divedFame);
                                    karma.Add(divedKarma);
                                }
                                else
                                {
                                    fame[index] += divedFame;
                                    karma[index] += divedKarma;
                                }
                            }
                        }
                    }
                    else
                    {
                        titles.Add(ds.m_Mobile);
                        fame.Add(totalFame);
                        karma.Add(totalKarma);
                    }

                    damage.Add(ds.m_Damage);

                    OnKilledBy(ds.m_Mobile);

                    if (!givenFactionKill)
                    {
                        givenFactionKill = true;
                        Faction.HandleDeath(this, ds.m_Mobile);
                    }

                    var region = ds.m_Mobile.Region;

                    if (!givenToTKill && (Map == Map.Tokuno || region.IsPartOf("Yomotsu Mines") ||
                                          region.IsPartOf("Fan Dancer's Dojo")))
                    {
                        givenToTKill = true;
                        TreasuresOfTokuno.HandleKill(this, ds.m_Mobile);
                    }

                    if (ds.m_Mobile is PlayerMobile pm)
                    {
                        if (MLQuestSystem.Enabled)
                        {
                            MLQuestSystem.HandleKill(pm, this);
                        }

                        if (givenQuestKill)
                        {
                            continue;
                        }

                        var qs = pm.Quest;

                        if (qs != null)
                        {
                            qs.OnKill(this, c);
                            givenQuestKill = true;
                        }
                    }
                }

                int scaleXp = FinalExperience();
                for (var i = 0; i < titles.Count; ++i)
                {
                    int percentKill = (damage[i] / HitsMax) * 100;
                    if (percentKill > 100)
                    {
                        percentKill = 100;
                    }
                    int contributedXp = AOS.Scale(scaleXp, percentKill);
                    int deityXp = contributedXp;
                    Titles.AwardFame(titles[i], fame[i], true);
                    Titles.AwardKarma(titles[i], karma[i], true);
                    if (titles[i] is PlayerMobile)
                    {
                        PlayerMobile player = (PlayerMobile)titles[i];
                        if (Level >= player.Level - 2)
                        {
                            if (Level > player.Level + 5)
                            {
                                contributedXp += (Level - player.Level) * Level;
                            }
                            else if (Level < player.Level)
                            {
                                contributedXp /= player.Level + 1 - Level;
                            }
                            BaseTalent fastLearner = player.GetTalent(typeof(FastLearner));
                            if (fastLearner != null)
                            {
                                contributedXp += AOS.Scale(contributedXp, fastLearner.Level * 10);
                            }
                            BaseTalent wisdom = player.GetTalent(typeof(Wisdom));
                            if (wisdom != null)
                            {
                                contributedXp += AOS.Scale(contributedXp, wisdom.Level * 30);
                            }
                            if (player.Ranger())
                            {
                                contributedXp /= 2; // 50% ranger 50% normal gain
                                if (!player.MaxLevel())
                                {
                                    player.RangerExperience += contributedXp;
                                }
                            }
                            if (!player.MaxLevel())
                            {
                                player.NonCraftExperience += contributedXp;
                            }
                            player.SendMessage($"You slay {Name} and gain {contributedXp.ToString()} experience.");
                        }
                        else
                        {
                            deityXp -= (player.Level - Level) * 2;
                            if (deityXp < 0)
                            {
                                deityXp = 0;
                            }
                        }
                        Deity.PointsFromExperience(player, this, deityXp);
                    }
                }
            }

            if (LastKiller is PlayerMobile playerThatKilled)
            {
                foreach (KeyValuePair<Type, BaseTalent> entry in playerThatKilled.Talents)
                {
                    if (entry.Value.HasKillEffect)
                    {
                        entry.Value.CheckKillEffect(this, LastKiller);
                    }
                }
            } else if (LastKiller is BaseCreature creatureThatKilled)
            {
                if (creatureThatKilled.IsSoulFeeder)
                {
                    MonsterBuff.AddFeederStats(creatureThatKilled);
                } else if (!creatureThatKilled.Controlled && IsEnemy(creatureThatKilled) && creatureThatKilled.DynamicExperienceValue() < DynamicExperienceValue() && creatureThatKilled.Level <= Level)
                {
                    MonsterBuff.LevelUp(creatureThatKilled);
                }
            }
            MonsterBuff.CheckElementalAoe(this);
            base.OnDeath(c);

            if (c is Corpse corpse)
            {
                corpse.PreviousLife = GetType();
            }
            if (DeleteCorpseOnDeath)
            {
                c.Delete();
            }
        }

        public override void OnDelete()
        {
            var m = m_ControlMaster;
            SetControlMaster(null);

            SummonMaster = null;
            ReceivedHonorContext?.Cancel();
            base.OnDelete();
            m?.InvalidateProperties();
        }

        public override bool CanBeHarmful(Mobile target, bool message, bool ignoreOurBlessedness)
        {
            if (target is BaseFactionGuard)
            {
                return false;
            }

            if (target is BaseCreature creature && creature.IsInvulnerable || target is PlayerVendor or TownCrier)
            {
                if (message)
                {
                    if (target.Title == null)
                    {
                        SendMessage($"{target.Name} cannot be harmed.");
                    }
                    else
                    {
                        SendMessage($"{target.Name} {target.Title} cannot be harmed.");
                    }
                }

                return false;
            }

            return base.CanBeHarmful(target, message, ignoreOurBlessedness);
        }

        public override bool CanBeRenamedBy(Mobile from) =>
            Controlled && from == ControlMaster && !from.Region.IsPartOf<JailRegion>() ||
            base.CanBeRenamedBy(from);

        public bool SetControlMaster(Mobile m)
        {
            if (m == null)
            {
                ControlMaster = null;
                Controlled = false;
                ControlTarget = null;
                ControlOrder = OrderType.None;
            }
            else
            {
                if (Spawner?.UnlinkOnTaming == true)
                {
                    Spawner.Remove(this);
                    Spawner = null;
                }

                if (m.Followers + ControlSlots > m.FollowersMax)
                {
                    m.SendLocalizedMessage(1049607); // You have too many followers to control that creature.
                    return false;
                }

                CurrentWayPoint = null; // so tamed animals don't try to go back

                Home = Point3D.Zero;

                ControlMaster = m;
                Controlled = true;
                ControlTarget = null;
                ControlOrder = OrderType.Come;


                if (m_DeleteTimer != null)
                {
                    m_DeleteTimer.Stop();
                    m_DeleteTimer = null;
                }
            }

            Guild = null;
            ResetSpeeds();

            Delta(MobileDelta.Noto);

            InvalidateProperties();

            return true;
        }

        public override void OnRegionChange(Region Old, Region New)
        {
            base.OnRegionChange(Old, New);

            if (Controlled && Spawner?.UnlinkOnTaming == false && New?.AcceptsSpawnsFrom(Spawner.Region) != true)
            {
                Spawner.Remove(this);
                Spawner = null;
            }
        }

        public static bool Summon(BaseCreature creature, Mobile caster, Point3D p, int sound, TimeSpan duration) =>
            Summon(creature, true, caster, p, sound, duration);

        public static bool Summon(
            BaseCreature creature, bool controlled, Mobile caster, Point3D p, int sound,
            TimeSpan duration
        )
        {
            if (caster.Followers + creature.ControlSlots > caster.FollowersMax)
            {
                caster.SendLocalizedMessage(1049645); // You have too many followers to summon that creature.
                creature.Delete();
                return false;
            }

            Summoning = true;

            if (controlled)
            {
                creature.SetControlMaster(caster);
            }

            creature.RangeHome = 10;
            creature.Summoned = true;

            creature.SummonMaster = caster;

            var pack = creature.Backpack;

            if (pack != null)
            {
                for (var i = pack.Items.Count - 1; i >= 0; --i)
                {
                    if (i >= pack.Items.Count)
                    {
                        continue;
                    }

                    pack.Items[i].Delete();
                }
            }

            if (caster is PlayerMobile player)
            {
                BaseTalent summonerCommand = player.GetTalent(typeof(SummonerCommand));
                if (summonerCommand is not null)
                {
                    creature = (BaseCreature)summonerCommand.ScaleMobile(creature);
                }
                player.Leadership();
                if (Deity.AlignmentCheck(creature, player.Alignment, true))
                {
                    int penalty = Utility.RandomMinMax(1, 2);
                    Deity.RewardPoints(player, new []{penalty * -1 }, new []{ player.Alignment });
                    player.SendMessage("You have summoned a creature that is a sworn enemy to your alignment. Your deity loyalty has suffered a penalty.");
                }
            }

            new UnsummonTimer(creature, duration).Start();            creature.SummonEnd = Core.Now + duration;

            creature.MoveToWorld(p, caster.Map);

            Effects.PlaySound(p, creature.Map, sound);

            Summoning = false;

            return true;
        }

        public virtual void OnThink()
        {
            var tc = Core.TickCount;

            if (EnableRummaging && CanRummageCorpses && !Summoned && !Controlled && tc - m_NextRummageTime >= 0)
            {
                double min, max;

                if (Utility.RandomDouble() < ChanceToRummage && Rummage())
                {
                    min = MinutesToNextRummageMin;
                    max = MinutesToNextRummageMax;
                }
                else
                {
                    min = MinutesToNextChanceMin;
                    max = MinutesToNextChanceMax;
                }

                var delay = min + Utility.RandomDouble() * (max - min);
                m_NextRummageTime = tc + (int)TimeSpan.FromMinutes(delay).TotalMilliseconds;
            }

            // Fire breath, etc.
            TriggerAbility(MonsterAbilityTrigger.Think, Combatant);

            if ((CanHeal || CanHealOwner) && Alive && !IsHealing && !BardPacified)
            {
                var owner = ControlMaster;

                if (owner != null && CanHealOwner && tc - m_NextHealOwnerTime >= 0 && CanBeBeneficial(owner, true, true) &&
                    owner.Map == Map && InRange(owner, HealStartRange) && InLOS(owner) &&
                    owner.Hits < HealOwnerTrigger * owner.HitsMax)
                {
                    HealStart(owner);

                    m_NextHealOwnerTime = tc + (int)TimeSpan.FromSeconds(HealOwnerInterval).TotalMilliseconds;
                }
                else if (CanHeal && tc - m_NextHealTime >= 0 && CanBeBeneficial(this) &&
                         (Hits < HealTrigger * HitsMax || Poisoned))
                {
                    HealStart(this);

                    m_NextHealTime = tc + (int)TimeSpan.FromSeconds(HealInterval).TotalMilliseconds;
                }
            }

            if (ReturnsToHome && IsSpawnerBound() && !InRange(Home, RangeHome))
            {
                if (Combatant == null && Warmode == false && Utility.RandomDouble() < .10) /* some throttling */
                {
                    m_FailedReturnHome = !Move(GetDirectionTo(Home.X, Home.Y)) ? m_FailedReturnHome + 1 : 0;

                    if (m_FailedReturnHome > 5)
                    {
                        SetLocation(Home, true);

                        m_FailedReturnHome = 0;
                    }
                }
            }
            else
            {
                m_FailedReturnHome = 0;
            }

            if (HasAura && tc - m_NextAura >= 0)
            {
                AuraDamage();
                m_NextAura = tc + (int)AuraInterval.TotalMilliseconds;
            }
        }

        public virtual bool Rummage()
        {
            if (Backpack == null)
            {
                return false;
            }

            var eable = GetItemsInRange<Corpse>(2);
            Corpse toRummage = null;
            foreach (var c in eable)
            {
                if (c.Items.Count > 0)
                {
                    toRummage = c;
                    break;
                }
            }

            eable.Free();

            if (toRummage == null)
            {
                return false;
            }

            var items = toRummage.Items;

            for (var i = 0; i < items.Count; ++i)
            {
                var item = items.RandomElement();

                Lift(item, item.Amount, out var rejected, out var _);

                if (!rejected && Drop(this, new Point3D(-1, -1, 0)))
                {
                    // *rummages through a corpse and takes an item*
                    PublicOverheadMessage(MessageType.Emote, 0x3B2, 1008086);
                    // TODO: Instancing of Rummaged stuff.
                    return true;
                }
            }

            return false;
        }

        public void Pacify(Mobile master, DateTime endtime)
        {
            BardPacified = true;
            BardEndTime = endtime;
            Timer.StartTimer(TimeSpan.FromSeconds(Utility.Random(40)), Unpacify, out _peacemakingTimer);
        }

        public override Mobile GetDamageMaster(Mobile damagee)
        {
            if (BardProvoked && damagee == BardTarget)
            {
                return BardMaster;
            }

            if (m_Controlled && m_ControlMaster != null)
            {
                return m_ControlMaster;
            }

            if (_summoned && m_SummonMaster != null)
            {
                return m_SummonMaster;
            }

            return base.GetDamageMaster(damagee);
        }

        public void Unprovoke()
        {
            if (BardMaster != null && BardMaster.Alive)
            {
                if (Combatant is BaseCreature combatant)
                {
                    combatant.BardTarget = BardMaster;
                    combatant.BardProvoked = false;
                    combatant.Combatant = BardMaster;
                }
                BardTarget = BardMaster;
                BardProvoked = false;
                Combatant = BardMaster;
            }
        }
        public void Provoke(Mobile master, Mobile target, bool bSuccess)
        {
            BardProvoked = true;
            double seconds = Utility.RandomMinMax(35, 45);
            if (!Core.ML)
            {
                PublicOverheadMessage(MessageType.Emote, EmoteHue, false, "*looks furious*");
            }

            if (bSuccess)
            {
                PlaySound(GetIdleSound());

                BardMaster = master;
                BardTarget = target;
                Combatant = target;
                BardEndTime = Core.Now + TimeSpan.FromSeconds(seconds);

                if (target is BaseCreature t)
                {
                    if (t.Unprovokable || t.IsParagon || t.IsHeroic || BaseInstrument.GetBaseDifficulty(t, true) >= 160.0)
                    {
                        return;
                    }

                    t.BardProvoked = true;

                    t.BardMaster = master;
                    t.BardTarget = this;
                    t.Combatant = this;
                    t.BardEndTime = Core.Now + TimeSpan.FromSeconds(seconds);

                    Timer.StartTimer(TimeSpan.FromSeconds(seconds - 10), Unprovoke, out _provocationTimer);
                }
            }
            else
            {
                PlaySound(GetAngerSound());

                BardMaster = master;
                BardTarget = target;
            }
        }

        public bool FindMyName(string str, bool bWithAll)
        {
            var name = Name;

            if (name == null || str.Length < name.Length)
            {
                return false;
            }

            var wordsString = str.Split(' ');
            var wordsName = name.Split(' ');

            for (var j = 0; j < wordsName.Length; j++)
            {
                var wordName = wordsName[j];

                var bFound = false;
                for (var i = 0; i < wordsString.Length; i++)
                {
                    var word = wordsString[i];

                    if (word.InsensitiveEquals(wordName))
                    {
                        bFound = true;
                    }

                    if (bWithAll && word.InsensitiveEquals("all"))
                    {
                        return true;
                    }
                }

                if (!bFound)
                {
                    return false;
                }
            }

            return true;
        }

        public static void TeleportPets(Mobile master, Point3D loc, Map map, bool onlyBonded = false)
        {
            if (master is PlayerMobile pm)
            {
                for (var i = 0; i < pm.AllFollowers.Count; i++)
                {
                    var m = pm.AllFollowers[i];
                    if (m.Map == master.Map && master.InRange(m, 3) && m is BaseCreature
                            { Controlled: true, ControlOrder: OrderType.Guard or OrderType.Follow or OrderType.Come } pet &&
                        pet.ControlMaster == master && (!onlyBonded || pet.IsBonded))
                    {
                        m.MoveToWorld(loc, map);
                    }
                }

                return;
            }

            using var queue = PooledRefQueue<Mobile>.Create();
            var eable = master.GetMobilesInRange(3);
            foreach (var m in eable)
            {
                if (m is BaseCreature
                        { Controlled: true, ControlOrder: OrderType.Guard or OrderType.Follow or OrderType.Come } pet &&
                    pet.ControlMaster == master && (!onlyBonded || pet.IsBonded))
                {
                    queue.Enqueue(pet);
                }
            }
            eable.Free();

            while (queue.Count > 0)
            {
                queue.Dequeue().MoveToWorld(loc, map);
            }
        }

        public virtual void ResurrectPet()
        {
            if (!IsDeadPet)
            {
                return;
            }

            OnBeforeResurrect();

            Poison = null;

            Warmode = false;

            Hits = 10;
            Stam = StamMax;
            Mana = 0;

            ProcessDeltaQueue();

            IsDeadPet = false;

            Span<byte> buffer = stackalloc byte[OutgoingMobilePackets.BondedStatusPacketLength];
            OutgoingMobilePackets.CreateBondedStatus(buffer, Serial, false);
            Effects.SendPacket(Location, Map, buffer);

            SendIncomingPacket();
            SendIncomingPacket();

            OnAfterResurrect();

            var owner = ControlMaster;

            if (owner?.Deleted == false && owner.Map == Map && owner.InRange(this, 12) && CanSee(owner) && InLOS(owner))
            {
                OwnerAbandonTime = DateTime.MinValue;
            }
            else if (OwnerAbandonTime == DateTime.MinValue)
            {
                OwnerAbandonTime = Core.Now;
            }

            CheckStatTimers();
        }

        public override bool CanBeDamaged()
        {
            if (IsDeadPet || IsInvulnerable)
            {
                return false;
            }

            return base.CanBeDamaged();
        }

        private bool IsSpawnerBound() =>
            Map != null && Map != Map.Internal &&
            FightMode != FightMode.None && RangeHome >= 0 &&
            !Controlled && !Summoned && (Spawner as Spawner)?.Map == Map;

        public override void OnSectorDeactivate()
        {
            if (!Deleted && ReturnsToHome && IsSpawnerBound() && !InRange(Home, RangeHome + 5))
            {
                Timer.StartTimer(TimeSpan.FromSeconds(Utility.Random(45) + 15), GoHome_Callback);

                m_ReturnQueued = true;
            }
            else if (PlayerRangeSensitive)
            {
                AIObject?.Deactivate();
            }

            base.OnSectorDeactivate();
        }

        public void GoHome_Callback()
        {
            if (m_ReturnQueued && IsSpawnerBound() && !Map.GetSector(X, Y).Active)
            {
                SetLocation(Home, true);

                if (!Map.GetSector(X, Y).Active)
                {
                    AIObject?.Deactivate();
                }
            }

            m_ReturnQueued = false;
        }

        public override void OnSectorActivate()
        {
            if (PlayerRangeSensitive)
            {
                AIObject?.Activate();
            }

            base.OnSectorActivate();
        }

        protected virtual List<MLQuest> ConstructQuestList() => null;

        private void CheckShout(PlayerMobile pm, Point3D oldLocation)
        {
            if (m_MLNextShout > Core.Now || pm.Hidden || !pm.Alive)
            {
                return;
            }

            var shoutRange = ShoutRange;

            if (!InRange(pm.Location, shoutRange) || InRange(oldLocation, shoutRange) || !CanSee(pm) || !InLOS(pm))
            {
                return;
            }

            var context = MLQuestSystem.GetContext(pm);

            if (context?.IsFull == true)
            {
                return;
            }

            var quest = MLQuestSystem.RandomStarterQuest(this, pm, context);

            if (quest?.Activated != true || context?.IsDoingQuest(quest) == true)
            {
                return;
            }

            Shout(pm);
            m_MLNextShout = Core.Now + ShoutDelay;
        }

        public virtual void Shout(PlayerMobile pm)
        {
        }

        public static void Configure()
        {
            BondingEnabled = ServerConfiguration.GetSetting("taming.enableBonding", Core.LBR);
        }

        public void BeginDeleteTimer()
        {
            if (this is not BaseEscortable && !Summoned && !Deleted && !IsStabled)
            {
                StopDeleteTimer();
                m_DeleteTimer = new DeleteTimer(this, TimeSpan.FromDays(3.0));
                m_DeleteTimer.Start();
            }
        }

        public void StopDeleteTimer()
        {
            if (m_DeleteTimer != null)
            {
                m_DeleteTimer.Stop();
                m_DeleteTimer = null;
            }
        }

        public void SpillAcid(int amount)
        {
            SpillAcid(null, amount);
        }

        public void SpillAcid(Mobile target, int amount)
        {
            if (target != null && target.Map == null || Map == null)
            {
                return;
            }

            for (var i = 0; i < amount; ++i)
            {
                Point3D loc;
                var map = Map;

                if (target != null && amount == 1)
                {
                    loc = target.Location;
                    map = target.Map;
                }
                else
                {
                    loc = map.GetRandomNearbyLocation(Location);
                }

                var acid = NewHarmfulItem();
                acid.MoveToWorld(loc, map);
            }
        }

        /*
          Solen Style, override me for other mobiles/items:
          kappa+acidslime, grizzles+whatever, etc.
        */

        public virtual Item NewHarmfulItem() => new PoolOfAcid(TimeSpan.FromSeconds(10), 30, 30);

        public virtual void StopFlee()
        {
            EndFleeTime = DateTime.MinValue;
        }

        public virtual bool CheckFlee()
        {
            if (EndFleeTime == DateTime.MinValue)
            {
                return false;
            }

            if (Core.Now >= EndFleeTime)
            {
                StopFlee();
                return false;
            }

            return true;
        }

        public virtual void BeginFlee(TimeSpan maxDuration)
        {
            EndFleeTime = Core.Now + maxDuration;
        }

        public virtual bool IsPetFriend(Mobile m) => Friends?.Contains(m) == true;

        public virtual void AddPetFriend(Mobile m)
        {
            Friends ??= new List<Mobile>();

            Friends.Add(m);
        }

        public virtual void RemovePetFriend(Mobile m) => Friends?.Remove(m);

        public virtual bool IsFriend(Mobile m) => (m is PlayerMobile player && Deity.AlignmentCheck(this, player.CombatAlignment, false))
                                                  || (!IsOpposing(m) && m is BaseCreature c && m_Team == c.m_Team
                                                      && (_summoned || m_Controlled) == (c._summoned || c.m_Controlled));


        public virtual Allegiance GetFactionAllegiance(Mobile mob)
        {
            if (mob == null || mob.Map != Faction.Facet || FactionAllegiance == null)
            {
                return Allegiance.None;
            }

            var fac = Faction.Find(mob, true);

            if (fac == null)
            {
                return Allegiance.None;
            }

            return fac == FactionAllegiance ? Allegiance.Ally : Allegiance.Enemy;
        }

        public virtual Allegiance GetEthicAllegiance(Mobile mob)
        {
            if (mob == null || mob.Map != Faction.Facet || EthicAllegiance == null)
            {
                return Allegiance.None;
            }

            var ethic = Ethic.Find(mob, true);

            if (ethic == null)
            {
                return Allegiance.None;
            }

            return ethic == EthicAllegiance ? Allegiance.Ally : Allegiance.Enemy;
        }

        public virtual void AlterDamageScalarFrom(Mobile caster, ref double scalar)
        {
            TriggerAbilityAlterDamageScalar(MonsterAbilityTrigger.TakeSpellDamage, caster, ref scalar);
        }

        public virtual void AlterDamageScalarTo(Mobile target, ref double scalar)
        {
            TriggerAbilityAlterDamageScalar(MonsterAbilityTrigger.GiveSpellDamage, target, ref scalar);
            AlterDamageToByLevel(target, ref scalar);
            AlterScalarFromBuffs(false, ref scalar);
        }

        public virtual void AlterSpellDamageFrom(Mobile from, ref int damage)
        {
            TriggerAbilityAlterDamage(MonsterAbilityTrigger.TakeSpellDamage, from, ref damage);
            AlterDamageFromByLevel(from, ref damage);
            AlterDamageFromBuffs(true, ref damage);
        }

        public virtual void AlterSpellDamageTo(Mobile to, ref int damage)
        {
            TriggerAbilityAlterDamage(MonsterAbilityTrigger.GiveSpellDamage, to, ref damage);
            // AlterDamageToByLevel(to, ref damage);
        }

        public virtual void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            TriggerAbilityAlterDamage(MonsterAbilityTrigger.TakeMeleeDamage, from, ref damage);
            AlterDamageFromByLevel(from, ref damage);
            AlterDamageFromBuffs(true, ref damage);
        }

        public virtual void AlterMeleeDamageTo(Mobile to, ref int damage)
        {
            TriggerAbilityAlterDamage(MonsterAbilityTrigger.GiveMeleeDamage, to, ref damage);
            if (
                !((BaseInstrument.IsMageryCreature(this) || Skills.Necromancy.Base > 5.0) && to.Skills.MagicResist.Base < 70 || Mana <= 10)
                || Skills.Tactics.Base >= 90)
            {
                AlterDamageToByLevel(to, ref damage);
            }
            AlterDamageFromBuffs(false, ref damage);
        }

        public void AlterScalarFromBuffs(bool defending, ref double scalar)
        {
            if (IsHeroic || IsBoss)
            {
                scalar += (defending) ? -0.2 : 0.2;
            }
            if (IsVeteran || IsCorruptor)
            {
                scalar += (defending) ? -0.1 : 0.1;
            }
        }

        public void AlterDamageFromBuffs(bool defending, ref int scalar)
        {
            if (IsHeroic || IsBoss)
            {
                scalar += (defending) ? -2 : 2;
            }
            if (IsVeteran || IsCorruptor)
            {
                scalar += (defending) ? -1 : 1;
            }
        }

        public void AlterDamageToByLevel(Mobile to, ref int damage)
        {
            damage = to switch
            {
                PlayerMobile player   => CalculateDamageFromLevels(player.Level, Level, damage, false),
                BaseCreature creature => CalculateDamageFromLevels(creature.Level, Level, damage, false),
                _                     => damage
            };
        }
        public void AlterDamageToByLevel(Mobile to, ref double damage)
        {
            damage = to switch
            {
                PlayerMobile player   => CalculateDamageFromLevels(player.Level, Level, damage, false),
                BaseCreature creature => CalculateDamageFromLevels(creature.Level, Level, damage, false),
                _                     => damage
            };
        }
        public void AlterDamageFromByLevel(Mobile from, ref int damage)
        {
            damage = from switch
            {
                PlayerMobile player   => CalculateDamageFromLevels(Level, player.Level , damage, true),
                BaseCreature creature => CalculateDamageFromLevels(Level, creature.Level, damage, false),
                _                     => damage
            };
        }
        public void AlterDamageFromByLevel(Mobile from, ref double damage)
        {
            damage = from switch
            {
                PlayerMobile player   => CalculateDamageFromLevels(Level, player.Level, damage, true),
                BaseCreature creature => CalculateDamageFromLevels(Level, creature.Level, damage, false),
                _                     => damage
            };
        }

        public bool CheckCriticalStrike(int defenderLevel, int attackerLevel) => attackerLevel > defenderLevel + 5 && Utility.Random(135) < attackerLevel - defenderLevel;

        public int CalculateDamageFromLevels(int defenderLevel, int attackerLevel, int damage, bool attackingPlayer)
        {

            if (attackerLevel > defenderLevel + 2 && !attackingPlayer)
            {
                damage += attackerLevel - defenderLevel + 1;
                if (CheckCriticalStrike(defenderLevel, attackerLevel))
                {
                    BaseTalent.CriticalStrike(ref damage);
                }
            } else if (attackerLevel < defenderLevel - 2)
            {
                if (damage > 5)
                {
                    damage -= defenderLevel - attackerLevel - 1;
                }
            }

            if (!attackingPlayer)
            {
                if (IsHeroic || IsBoss)
                {
                    damage += 2;
                }
                if (IsVeteran)
                {
                    damage += 1;
                }
            }

            if (damage <= 0)
            {
                damage = 1;
            }

            return damage;
        }

        public double CalculateDamageFromLevels(int defenderLevel, int attackerLevel, double damage, bool attackingPlayer)
        {
            if (attackerLevel > defenderLevel + 2 && !attackingPlayer)
            {
                damage += (attackerLevel - defenderLevel) / 50.00;
                if (CheckCriticalStrike(defenderLevel, attackerLevel))
                {
                    BaseTalent.CriticalStrike(ref damage);
                }
            } else if (attackerLevel < defenderLevel - 2)
            {
                damage -= (defenderLevel - attackerLevel) / 100.00;
            }

            if (!attackingPlayer)
            {
                if (IsHeroic || IsBoss)
                {
                    damage += 0.2;
                }
                if (IsVeteran)
                {
                    damage += 0.1;
                }
            }

            if (damage <= 0)
            {
                damage = 0.1;
            }

            return damage;
        }

        public virtual bool CheckFoodPreference(Item f) =>
            CheckFoodPreference(f, FoodType.Eggs, m_Eggs) ||
            CheckFoodPreference(f, FoodType.Fish, m_Fish) ||
            CheckFoodPreference(f, FoodType.GrainsAndHay, m_GrainsAndHay) ||
            CheckFoodPreference(f, FoodType.Meat, m_Meat) ||
            CheckFoodPreference(f, FoodType.FruitsAndVegies, m_FruitsAndVegies) ||
            CheckFoodPreference(f, FoodType.Gold, m_Gold);

        public virtual bool CheckFoodPreference(Item fed, FoodType type, Type[] types)
        {
            if ((FavoriteFood & type) == 0)
            {
                return false;
            }

            var fedType = fed.GetType();
            var contains = false;

            for (var i = 0; !contains && i < types.Length; ++i)
            {
                contains = fedType == types[i];
            }

            return contains;
        }

        public virtual bool CheckFeed(Mobile from, Item dropped)
        {
            if (!IsDeadPet && Controlled && (ControlMaster == from || IsPetFriend(from)))
            {
                var f = dropped;

                if (CheckFoodPreference(f))
                {
                    var amount = f.Amount;

                    if (amount > 0)
                    {
                        int stamGain = f switch
                        {
                            Gold => amount - 50,
                            _    => amount * 15 - 50
                        };

                        if (stamGain > 0)
                        {
                            Stam += stamGain;
                        }

                        if (Core.SE)
                        {
                            if (m_Loyalty < MaxLoyalty)
                            {
                                m_Loyalty = MaxLoyalty;
                            }
                        }
                        else
                        {
                            for (var i = 0; i < amount; ++i)
                            {
                                if (m_Loyalty < MaxLoyalty && Utility.RandomBool())
                                {
                                    m_Loyalty += 10;
                                }
                            }
                        }

                        /* if (happier )*/
                        // looks like in OSI pets say they are happier even if they are at maximum loyalty
                        SayTo(from, 502060); // Your pet looks happier.

                        if (Body.IsAnimal)
                        {
                            Animate(3, 5, 1, true, false, 0);
                        }
                        else if (Body.IsMonster)
                        {
                            Animate(17, 5, 1, true, false, 0);
                        }

                        if (IsBondable && !IsBonded)
                        {
                            var master = m_ControlMaster;

                            if (master != null && master == from) // So friends can't start the bonding process
                            {
                                if (MinTameSkill <= 29.1 || master.Skills.AnimalTaming.Base >= MinTameSkill ||
                                    OverrideBondingReqs() ||
                                    Core.ML && master.Skills.AnimalTaming.Value >= MinTameSkill)
                                {
                                    if (BondingBegin == DateTime.MinValue)
                                    {
                                        BondingBegin = Core.Now;
                                    }
                                    else if (BondingBegin + BondingDelay <= Core.Now)
                                    {
                                        IsBonded = true;
                                        BondingBegin = DateTime.MinValue;
                                        from.SendLocalizedMessage(1049666); // Your pet has bonded with you!
                                    }
                                }
                                else if (Core.ML)
                                {
                                    // Your pet cannot form a bond with you until your animal taming ability has risen.
                                    from.SendLocalizedMessage(1075268);
                                }
                            }
                        }

                        dropped.Delete();
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual void OnActionWander()
        {
        }

        public virtual void OnActionCombat()
        {
        }

        public virtual void OnActionGuard()
        {
        }

        public virtual void OnActionFlee()
        {
        }

        public virtual void OnActionInteract()
        {
        }

        public virtual void OnActionBackoff()
        {
        }

        public virtual bool CheckTeach(SkillName skill, Mobile from)
        {
            if (!CanTeach)
            {
                return false;
            }

            if (skill == SkillName.Stealth && from.Skills.Hiding.Base < Stealth.HidingRequirement)
            {
                return false;
            }

            if (skill == SkillName.RemoveTrap && (from.Skills.Lockpicking.Base < 50.0 ||
                                                  from.Skills.DetectHidden.Base < 50.0))
            {
                return false;
            }

            return Core.AOS || skill != SkillName.Focus && skill != SkillName.Chivalry && skill != SkillName.Necromancy;
        }

        public virtual TeachResult CheckTeachSkills(
            SkillName skill, Mobile m, int maxPointsToLearn, ref int pointsToLearn,
            bool doTeach
        )
        {
            if (!CheckTeach(skill, m) || !m.CheckAlive())
            {
                return TeachResult.Failure;
            }

            var ourSkill = Skills[skill];
            var theirSkill = m.Skills[skill];

            if (ourSkill == null || theirSkill == null)
            {
                return TeachResult.Failure;
            }

            var baseToSet = ourSkill.BaseFixedPoint / 3;

            if (baseToSet > 420)
            {
                baseToSet = 420;
            }
            else if (baseToSet < 200)
            {
                return TeachResult.Failure;
            }

            if (baseToSet > theirSkill.CapFixedPoint)
            {
                baseToSet = theirSkill.CapFixedPoint;
            }

            pointsToLearn = baseToSet - theirSkill.BaseFixedPoint;

            if (maxPointsToLearn > 0 && pointsToLearn > maxPointsToLearn)
            {
                pointsToLearn = maxPointsToLearn;
                baseToSet = theirSkill.BaseFixedPoint + pointsToLearn;
            }

            if (pointsToLearn < 0)
            {
                return TeachResult.KnowsMoreThanMe;
            }

            if (pointsToLearn == 0)
            {
                return TeachResult.KnowsWhatIKnow;
            }

            if (theirSkill.Lock != SkillLock.Up)
            {
                return TeachResult.SkillNotRaisable;
            }

            var freePoints = Math.Max(m.Skills.Cap - m.Skills.Total, 0);
            var freeablePoints = 0;

            for (var i = 0; freePoints + freeablePoints < pointsToLearn && i < m.Skills.Length; ++i)
            {
                var sk = m.Skills[i];

                if (sk == theirSkill || sk.Lock != SkillLock.Down)
                {
                    continue;
                }

                freeablePoints += sk.BaseFixedPoint;
            }

            if (freePoints + freeablePoints == 0)
            {
                return TeachResult.NotEnoughFreePoints;
            }

            if (freePoints + freeablePoints < pointsToLearn)
            {
                pointsToLearn = freePoints + freeablePoints;
                baseToSet = theirSkill.BaseFixedPoint + pointsToLearn;
            }

            if (doTeach)
            {
                var need = pointsToLearn - freePoints;

                for (var i = 0; need > 0 && i < m.Skills.Length; ++i)
                {
                    var sk = m.Skills[i];

                    if (sk == theirSkill || sk.Lock != SkillLock.Down)
                    {
                        continue;
                    }

                    if (sk.BaseFixedPoint < need)
                    {
                        need -= sk.BaseFixedPoint;
                        sk.BaseFixedPoint = 0;
                    }
                    else
                    {
                        sk.BaseFixedPoint -= need;
                        need = 0;
                    }
                }

                /* Sanity check */
                if (baseToSet > theirSkill.CapFixedPoint ||
                    m.Skills.Total - theirSkill.BaseFixedPoint + baseToSet > m.Skills.Cap)
                {
                    return TeachResult.NotEnoughFreePoints;
                }

                theirSkill.BaseFixedPoint = baseToSet;
            }

            return TeachResult.Success;
        }

        public virtual bool CheckTeachingMatch(Mobile m)
        {
            if (m_Teaching == (SkillName)(-1))
            {
                return false;
            }

            if (m is PlayerMobile mobile)
            {
                return mobile.Learning == m_Teaching;
            }

            return true;
        }

        public virtual bool Teach(SkillName skill, Mobile m, int maxPointsToLearn, bool doTeach)
        {
            var pointsToLearn = 0;
            var res = CheckTeachSkills(skill, m, maxPointsToLearn, ref pointsToLearn, doTeach);

            switch (res)
            {
                case TeachResult.KnowsMoreThanMe:
                    {
                        Say(501508); // I cannot teach thee, for thou knowest more than I!
                        break;
                    }
                case TeachResult.KnowsWhatIKnow:
                    {
                        Say(501509); // I cannot teach thee, for thou knowest all I can teach!
                        break;
                    }
                case TeachResult.NotEnoughFreePoints:
                case TeachResult.SkillNotRaisable:
                    {
                        // Make sure this skill is marked to raise. If you are near the skill cap (700 points) you may need to lose some points in another skill first.
                        m.SendLocalizedMessage(501510, "", 0x22);
                        break;
                    }
                case TeachResult.Success:
                    {
                        if (doTeach)
                        {
                            Say(501539);                    // Let me show thee something of how this is done.
                            m.SendLocalizedMessage(501540); // Your skill level increases.

                            m_Teaching = (SkillName)(-1);

                            if (m is PlayerMobile mobile)
                            {
                                mobile.Learning = (SkillName)(-1);
                            }
                        }
                        else
                        {
                            // I will teach thee all I know, if paid the amount in full.  The price is:
                            Say(1019077, AffixType.Append, $" {pointsToLearn}", "");
                            Say(1043108); // For less I shall teach thee less.

                            m_Teaching = skill;

                            if (m is PlayerMobile mobile)
                            {
                                mobile.Learning = skill;
                            }
                        }

                        return true;
                    }
            }

            return false;
        }

        public void SetSpeed(double active, double passive)
        {
            ActiveSpeed = active;
            PassiveSpeed = passive;
            CurrentSpeed = PassiveSpeed;
        }

        public void SetDamage(int val)
        {
            m_DamageMin = val;
            m_DamageMax = val;
        }

        public void SetDamage(int min, int max)
        {
            m_DamageMin = min;
            m_DamageMax = max;
        }

        public void SetHits(int val)
        {
            if (val < 1000 && !Core.AOS)
            {
                val = val * 100 / 60;
            }

            HitsMaxSeed = val;
            Hits = HitsMax;
        }

        public void SetHits(int min, int max)
        {
            if (min < 1000 && !Core.AOS)
            {
                min = min * 100 / 60;
                max = max * 100 / 60;
            }

            HitsMaxSeed = Utility.RandomMinMax(min, max);
            Hits = HitsMax;
        }

        public void SetStam(int val)
        {
            StamMaxSeed = val;
            Stam = StamMax;
        }

        public void SetStam(int min, int max)
        {
            StamMaxSeed = Utility.RandomMinMax(min, max);
            Stam = StamMax;
        }

        public void SetMana(int val)
        {
            ManaMaxSeed = val;
            Mana = ManaMax;
        }

        public void SetMana(int min, int max)
        {
            ManaMaxSeed = Utility.RandomMinMax(min, max);
            Mana = ManaMax;
        }

        public void SetStr(int val)
        {
            RawStr = val;
            Hits = HitsMax;
        }

        public void SetStr(int min, int max)
        {
            RawStr = Utility.RandomMinMax(min, max);
            Hits = HitsMax;
        }

        public void SetDex(int val)
        {
            RawDex = val;
            Stam = StamMax;
        }

        public void SetDex(int min, int max)
        {
            RawDex = Utility.RandomMinMax(min, max);
            Stam = StamMax;
        }

        public void SetInt(int val)
        {
            RawInt = val;
            Mana = ManaMax;
        }

        public void SetInt(int min, int max)
        {
            RawInt = Utility.RandomMinMax(min, max);
            Mana = ManaMax;
        }

        public void SetDamageType(ResistanceType type, int min, int max)
        {
            SetDamageType(type, Utility.RandomMinMax(min, max));
        }

        public void SetDamageType(ResistanceType type, int val)
        {
            switch (type)
            {
                case ResistanceType.Physical:
                    {
                        PhysicalDamage = val;
                        break;
                    }
                case ResistanceType.Fire:
                    {
                        FireDamage = val;
                        break;
                    }
                case ResistanceType.Cold:
                    {
                        ColdDamage = val;
                        break;
                    }
                case ResistanceType.Poison:
                    {
                        PoisonDamage = val;
                        break;
                    }
                case ResistanceType.Energy:
                    {
                        EnergyDamage = val;
                        break;
                    }
            }
        }

        public void SetResistance(ResistanceType type, int min, int max)
        {
            SetResistance(type, Utility.RandomMinMax(min, max));
        }

        public void SetResistance(ResistanceType type, int val)
        {
            switch (type)
            {
                case ResistanceType.Physical:
                    {
                        m_PhysicalResistance = val;
                        break;
                    }
                case ResistanceType.Fire:
                    {
                        m_FireResistance = val;
                        break;
                    }
                case ResistanceType.Cold:
                    {
                        m_ColdResistance = val;
                        break;
                    }
                case ResistanceType.Poison:
                    {
                        m_PoisonResistance = val;
                        break;
                    }
                case ResistanceType.Energy:
                    {
                        m_EnergyResistance = val;
                        break;
                    }
            }

            UpdateResistances();
        }

        public void SetSkill(SkillName name, double val)
        {
            ExperienceValue += (int)val;
            Skills[name].BaseFixedPoint = (int)(val * 10);

            if (Skills[name].Base > Skills[name].Cap)
            {
                if (Core.SE)
                {
                    SkillsCap += Skills[name].BaseFixedPoint - Skills[name].CapFixedPoint;
                }

                Skills[name].Cap = Skills[name].Base;
            }
        }

        public void SetSkill(SkillName name, double min, double max)
        {
            var minFixed = (int)(min * 10);
            var maxFixed = (int)(max * 10);

            ExperienceValue += (int)max;

            Skills[name].BaseFixedPoint = Utility.RandomMinMax(minFixed, maxFixed);

            if (Skills[name].Base > Skills[name].Cap)
            {
                if (Core.SE)
                {
                    SkillsCap += Skills[name].BaseFixedPoint - Skills[name].CapFixedPoint;
                }

                Skills[name].Cap = Skills[name].Base;
            }
        }

        public void SetFameLevel(int level)
        {
            Fame = level switch
            {
                1 => Utility.RandomMinMax(0, 1249),
                2 => Utility.RandomMinMax(1250, 2499),
                3 => Utility.RandomMinMax(2500, 4999),
                4 => Utility.RandomMinMax(5000, 9999),
                5 => Utility.RandomMinMax(10000, 10000),
                _ => Fame
            };
        }

        public void SetKarmaLevel(int level)
        {
            Karma = level switch
            {
                0 => -Utility.RandomMinMax(0, 624),
                1 => -Utility.RandomMinMax(625, 1249),
                2 => -Utility.RandomMinMax(1250, 2499),
                3 => -Utility.RandomMinMax(2500, 4999),
                4 => -Utility.RandomMinMax(5000, 9999),
                5 => -Utility.RandomMinMax(10000, 10000),
                _ => Karma
            };
        }

        public void PackArcaneScroll(int min, int max)
        {
            PackArcaneScroll(Utility.RandomMinMax(min, max));
        }

        public void PackArcaneScroll(int amount)
        {
            for (var i = 0; i < amount; ++i)
            {
                PackArcaneScroll();
            }
        }

        public void PackArcaneScroll()
        {
            if (!Core.ML)
            {
                return;
            }

            PackItem(Loot.Construct(Loot.ArcanistScrollTypes));
        }

        public void PackPotion()
        {
            PackItem(Loot.RandomPotion());
        }

        public void PackArcanceScroll(double chance)
        {
            if (!Core.ML || chance <= Utility.RandomDouble())
            {
                return;
            }

            PackItem(Loot.Construct(Loot.ArcanistScrollTypes));
        }

        public void PackNecroScroll(int index)
        {
            if (!Core.AOS || Utility.RandomDouble() < 0.95)
            {
                return;
            }

            PackItem(Loot.Construct(Loot.NecromancyScrollTypes, index));
        }

        public void PackScroll(int minCircle, int maxCircle)
        {
            PackScroll(Utility.RandomMinMax(minCircle, maxCircle));
        }

        public void PackScroll(int circle)
        {
            var min = (circle - 1) * 8;

            PackItem(Loot.RandomScroll(min, min + 7, SpellbookType.Regular));
        }

        public void PackMagicItems(int minLevel, int maxLevel, double armorChance = 0.30, double weaponChance = 0.15)
        {
            if (!PackArmor(minLevel, maxLevel, armorChance))
            {
                PackWeapon(minLevel, maxLevel, weaponChance);
            }
        }

        // If this needs to be serialized, recommend creating a hash or registry id. Don't serialize strings.
        public virtual SpeedLevel SpeedClass => SpeedLevel.None;

        public virtual void GetSpeeds(out double activeSpeed, out double passiveSpeed)
        {
            NPCSpeeds.GetSpeeds(this, out activeSpeed, out passiveSpeed);
        }

        public void ResetSpeeds(bool currentUseActive = false)
        {
            GetSpeeds(out var activeSpeed, out var passiveSpeed);

            ActiveSpeed = activeSpeed;
            PassiveSpeed = passiveSpeed;
            CurrentSpeed = currentUseActive ? activeSpeed : passiveSpeed;
        }

        public virtual void DropBackpack()
        {
            if (Backpack?.Items.Count > 0)
            {
                Backpack b = new CreatureBackpack(Name);

                var list = new List<Item>(Backpack.Items);
                foreach (var item in list)
                {
                    b.DropItem(item);
                }

                var house = BaseHouse.FindHouseAt(this);
                if (house != null)
                {
                    b.MoveToWorld(house.BanLocation, house.Map);
                }
                else
                {
                    b.MoveToWorld(Location, Map);
                }
            }
        }

        public virtual void GenerateLoot(bool spawning)
        {
            m_Spawning = spawning;

            if (!spawning)
            {
                m_KillersLuck = LootPack.GetLuckChanceForKiller(this);
            }

            GenerateLoot();

            if (LastKiller is PlayerMobile killer)
            {
                BaseTalent keenEye = killer.GetTalent(typeof(KeenEye));
                if (keenEye != null && AI != AIType.AI_Animal && Utility.Random(100) < keenEye.ModifySpellMultiplier())
                {
                    string message = "** You find something interesting in their pack **";
                    int random = Utility.RandomMinMax(1, 3);
                    if (Utility.Random(500) <= keenEye.ModifySpellMultiplier())
                    {
                        message = "** You find something powerful in their pack **";
                        switch (random)
                        {
                            case 1:
                                ForceRandomLoot(LootPack.Rich);
                                break;
                            case 2:
                                ForceRandomLoot(LootPack.FilthyRich);
                                break;
                            case 3:
                                ForceRandomLoot(LootPack.UltraRich);
                                break;
                        }
                    }
                    else
                    {
                        switch (random)
                        {
                            case 1:
                                ForceRandomLoot(LootPack.Poor);
                                break;
                            case 2:
                                ForceRandomLoot(LootPack.Meager);
                                break;
                            case 3:
                                ForceRandomLoot(LootPack.Average);
                                break;
                        }
                    }
                    killer.NonlocalOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        false,
                        message
                    );
                }
            }

            if (m_Paragon)
            {
                if (Fame < 1250)
                {
                    AddLoot(LootPack.Meager);
                }
                else if (Fame < 2500)
                {
                    AddLoot(LootPack.Average);
                }
                else if (Fame < 5000)
                {
                    AddLoot(LootPack.Rich);
                }
                else if (Fame < 10000)
                {
                    AddLoot(LootPack.FilthyRich);
                }
                else
                {
                    AddLoot(LootPack.UltraRich);
                }
            }

            m_Spawning = false;
            m_KillersLuck = 0;
        }

        public virtual void GenerateLoot()
        {
        }

        public virtual void ForceRandomLoot(LootPack pack, int amount = 1, int itemAmount = 1)
        {
            var backpack = Backpack ?? new Backpack { Movable = false };
            AddItem(backpack);

            for (var i = 0; i < amount; ++i)
            {
                pack.ForceGenerate(this, backpack, pack.RandomEntry(), itemAmount);
            }
        }

        public virtual void AddLoot(LootPack pack, int amount)
        {
            for (var i = 0; i < amount; ++i)
            {
                AddLoot(pack);
            }
        }

        public virtual void AddLoot(LootPack pack)
        {
            if (Summoned)
            {
                return;
            }

            var backpack = Backpack ?? new Backpack { Movable = false };
            AddItem(backpack);

            pack.Generate(this, backpack, m_Spawning, m_KillersLuck);
        }

        public bool PackArmor(int minLevel, int maxLevel) => PackArmor(minLevel, maxLevel, 1.0);

        public bool PackArmor(int minLevel, int maxLevel, double chance)
        {
            if (chance <= Utility.RandomDouble())
            {
                return false;
            }

            minLevel = Math.Clamp(minLevel, 0, 5);
            maxLevel = Math.Clamp(maxLevel, 0, 5);

            if (Core.AOS)
            {
                var item = Loot.RandomArmorOrShieldOrJewelry();

                if (item == null)
                {
                    return false;
                }

                GetRandomAOSStats(minLevel, maxLevel, out var attributeCount, out var min, out var max);

                if (item is BaseArmor armor)
                {
                    BaseRunicTool.ApplyAttributesTo(armor, attributeCount, min, max);
                }
                else if (item is BaseJewel jewel)
                {
                    BaseRunicTool.ApplyAttributesTo(jewel, attributeCount, min, max);
                }

                PackItem(item);
            }
            else
            {
                var armor = Loot.RandomArmorOrShield();

                if (armor == null)
                {
                    return false;
                }

                armor.ProtectionLevel = (ArmorProtectionLevel)RandomMinMaxScaled(minLevel, maxLevel);
                armor.Durability = (ArmorDurabilityLevel)RandomMinMaxScaled(minLevel, maxLevel);

                PackItem(armor);
            }

            return true;
        }

        public static void GetRandomAOSStats(int minLevel, int maxLevel, out int attributeCount, out int min, out int max)
        {
            var v = RandomMinMaxScaled(minLevel, maxLevel);

            if (v >= 5)
            {
                attributeCount = Utility.RandomMinMax(2, 6);
                min = 20;
                max = 70;
            }
            else if (v == 4)
            {
                attributeCount = Utility.RandomMinMax(2, 4);
                min = 20;
                max = 50;
            }
            else if (v == 3)
            {
                attributeCount = Utility.RandomMinMax(2, 3);
                min = 20;
                max = 40;
            }
            else if (v == 2)
            {
                attributeCount = Utility.RandomMinMax(1, 2);
                min = 10;
                max = 30;
            }
            else
            {
                attributeCount = 1;
                min = 10;
                max = 20;
            }
        }

        public static int RandomMinMaxScaled(int min, int max)
        {
            if (min == max)
            {
                return min;
            }

            if (min > max)
            {
                (min, max) = (max, min);
            }

            /* Example:
             *    min: 1
             *    max: 5
             *  count: 5
             *
             * total = (5*5) + (4*4) + (3*3) + (2*2) + (1*1) = 25 + 16 + 9 + 4 + 1 = 55
             *
             * chance for min+0 : 25/55 : 45.45%
             * chance for min+1 : 16/55 : 29.09%
             * chance for min+2 :  9/55 : 16.36%
             * chance for min+3 :  4/55 :  7.27%
             * chance for min+4 :  1/55 :  1.81%
             */

            var count = max - min + 1;
            int total = 0, toAdd = count;

            for (var i = 0; i < count; ++i, --toAdd)
            {
                total += toAdd * toAdd;
            }

            var rand = Utility.Random(total);
            toAdd = count;

            var val = min;

            for (var i = 0; i < count; ++i, --toAdd, ++val)
            {
                rand -= toAdd * toAdd;

                if (rand < 0)
                {
                    break;
                }
            }

            return val;
        }

        public bool PackSlayer(double chance = 0.05)
        {
            if (chance <= Utility.RandomDouble())
            {
                return false;
            }

            if (Utility.RandomBool())
            {
                var instrument = Loot.RandomInstrument();

                if (instrument != null)
                {
                    instrument.Slayer = SlayerGroup.GetLootSlayerType(GetType());
                    PackItem(instrument);
                }
            }
            else if (!Core.AOS)
            {
                var weapon = Loot.RandomWeapon();

                if (weapon != null)
                {
                    weapon.Slayer = SlayerGroup.GetLootSlayerType(GetType());
                    PackItem(weapon);
                }
            }

            return true;
        }

        public bool PackWeapon(int minLevel, int maxLevel, double chance = 1.0)
        {
            if (chance <= Utility.RandomDouble())
            {
                return false;
            }

            minLevel = Math.Clamp(minLevel, 0, 5);
            maxLevel = Math.Clamp(maxLevel, 0, 5);

            if (Core.AOS)
            {
                var item = Loot.RandomWeaponOrJewelry();

                if (item == null)
                {
                    return false;
                }

                GetRandomAOSStats(minLevel, maxLevel, out var attributeCount, out var min, out var max);

                if (item is BaseWeapon weapon)
                {
                    BaseRunicTool.ApplyAttributesTo(weapon, attributeCount, min, max);
                }
                else if (item is BaseJewel jewel)
                {
                    BaseRunicTool.ApplyAttributesTo(jewel, attributeCount, min, max);
                }

                PackItem(item);
            }
            else
            {
                var weapon = Loot.RandomWeapon();

                if (weapon == null)
                {
                    return false;
                }

                if (Utility.RandomDouble() < 0.05)
                {
                    weapon.Slayer = SlayerName.Silver;
                }

                weapon.DamageLevel = (WeaponDamageLevel)RandomMinMaxScaled(minLevel, maxLevel);
                weapon.AccuracyLevel = (WeaponAccuracyLevel)RandomMinMaxScaled(minLevel, maxLevel);
                weapon.DurabilityLevel = (WeaponDurabilityLevel)RandomMinMaxScaled(minLevel, maxLevel);

                PackItem(weapon);
            }

            return true;
        }

        public void PackGold(int amount)
        {
            if (amount > 0)
            {
                PackItem(new Gold(amount));
            }
        }

        public void PackGold(int min, int max)
        {
            PackGold(Utility.RandomMinMax(min, max));
        }

        public void PackStatue(int min, int max)
        {
            PackStatue(Utility.RandomMinMax(min, max));
        }

        public void PackStatue(int amount)
        {
            for (var i = 0; i < amount; ++i)
            {
                PackStatue();
            }
        }

        public void PackStatue()
        {
            PackItem(Loot.RandomStatue());
        }

        public void PackGem(int min, int max)
        {
            PackGem(Utility.RandomMinMax(min, max));
        }

        public void PackGem(int amount = 1)
        {
            if (amount <= 0)
            {
                return;
            }

            var gem = Loot.RandomGem();

            gem.Amount = amount;

            PackItem(gem);
        }

        public void PackNecroReg(int min, int max)
        {
            PackNecroReg(Utility.RandomMinMax(min, max));
        }

        public void PackNecroReg(int amount)
        {
            for (var i = 0; i < amount; ++i)
            {
                PackNecroReg();
            }
        }

        public void PackNecroReg()
        {
            if (!Core.AOS)
            {
                return;
            }

            PackItem(Loot.RandomNecromancyReagent());
        }

        public void PackReg(int min, int max)
        {
            PackReg(Utility.RandomMinMax(min, max));
        }

        public void PackReg(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            var reg = Loot.RandomReagent();

            reg.Amount = amount;

            PackItem(reg);
        }

        public void PackItem(Item item)
        {
            if (item == null)
            {
                return;
            }

            if (Summoned)
            {
                item.Delete();
                return;
            }

            var pack = Backpack ?? new Backpack { Movable = false };
            AddItem(pack);

            if (!item.Stackable || !pack.TryDropItem(this, item, false)) // try stack
            {
                pack.DropItem(item); // failed, drop it anyway
            }
        }

        public virtual void HealStart(Mobile patient)
        {
            var onSelf = patient == this;

            // DoBeneficial( patient );

            RevealingAction();

            if (!onSelf)
            {
                patient.RevealingAction();
                patient.SendLocalizedMessage(1008078, false, Name); // : Attempting to heal you.
            }

            var seconds = (onSelf ? HealDelay : HealOwnerDelay) + (patient.Alive ? 0.0 : 5.0);

            Timer.StartTimer(TimeSpan.FromSeconds(seconds), () => Heal(patient), out _healTimerToken);
        }

        public virtual void Heal(Mobile patient)
        {
            if (!Alive || Map == Map.Internal || !CanBeBeneficial(patient, true, true) || patient.Map != Map ||
                !InRange(patient, HealEndRange))
            {
                StopHeal();
                return;
            }

            var onSelf = patient == this;

            if (!patient.Alive)
            {
            }
            else if (patient.Poisoned)
            {
                var poisonLevel = patient.Poison.Level;

                var healing = Skills.Healing.Value;
                var anatomy = Skills.Anatomy.Value;
                var chance = (healing - 30.0) / 50.0 - poisonLevel * 0.1;

                if (healing >= 60.0 && anatomy >= 60.0 && chance > Utility.RandomDouble())
                {
                    if (patient.CurePoison(this))
                    {
                        patient.SendLocalizedMessage(1010059); // You have been cured of all poisons.

                        CheckSkill(SkillName.Healing, 0.0, 60.0 + poisonLevel * 10.0); // TODO: Verify formula
                        CheckSkill(SkillName.Anatomy, 0.0, 100.0);
                    }
                }
            }
            else if (BleedAttack.IsBleeding(patient))
            {
                patient.SendLocalizedMessage(1060167); // The bleeding wounds have healed, you are no longer bleeding!
                BleedAttack.EndBleed(patient, false);
            }
            else
            {
                var healing = Skills.Healing.Value;
                var anatomy = Skills.Anatomy.Value;
                var chance = (healing + 10.0) / 100.0;

                if (chance > Utility.RandomDouble())
                {
                    var min = anatomy / 10.0 + healing / 6.0 + 4.0;
                    var max = anatomy / 8.0 + healing / 3.0 + 4.0;

                    if (onSelf)
                    {
                        max += 10;
                    }

                    var toHeal = min + Utility.RandomDouble() * (max - min);

                    toHeal *= HealScalar;

                    patient.Heal((int)toHeal);

                    CheckSkill(SkillName.Healing, 0.0, 90.0);
                    CheckSkill(SkillName.Anatomy, 0.0, 100.0);
                }
            }

            HealEffect(patient);

            StopHeal();

            if (onSelf && HealFully && Hits >= HealTrigger * HitsMax && Hits < HitsMax ||
                !onSelf && HealOwnerFully && patient.Hits >= HealOwnerTrigger * patient.HitsMax &&
                patient.Hits < patient.HitsMax)
            {
                HealStart(patient);
            }
        }

        public virtual void StopHeal()
        {
            _healTimerToken.Cancel();
        }

        public virtual void HealEffect(Mobile patient)
        {
            patient.PlaySound(HealSound);
        }

        public virtual void AuraDamage()
        {
            if (!Alive || IsDeadBondedPet)
            {
                return;
            }

            var eable = GetMobilesInRange(AuraRange);
            using var queue = PooledRefQueue<Mobile>.Create();
            foreach (var m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && (Core.AOS || InLOS(m)) &&
                    (m is BaseCreature bc && (bc.Controlled || bc.Summoned || bc.Team != Team) || m.Player))
                {
                    queue.Enqueue(m);
                }
            }
            eable.Free();

            while (queue.Count > 0)
            {
                var m = queue.Dequeue();

                AOS.Damage(
                    m,
                    this,
                    AuraBaseDamage,
                    AuraPhysicalDamage,
                    AuraFireDamage,
                    AuraColdDamage,
                    AuraPoisonDamage,
                    AuraEnergyDamage,
                    AuraChaosDamage
                );
                AuraEffect(m);
            }
        }

        public virtual void AuraEffect(Mobile m)
        {
        }

        private class TameEntry : ContextMenuEntry
        {
            private readonly BaseCreature m_Mobile;

            public TameEntry(Mobile from, BaseCreature creature) : base(6130, 6)
            {
                m_Mobile = creature;

                Enabled = Enabled && (from.Female ? creature.AllowFemaleTamer : creature.AllowMaleTamer);
            }

            public override void OnClick()
            {
                if (!Owner.From.CheckAlive())
                {
                    return;
                }

                Owner.From.TargetLocked = true;
                AnimalTaming.DisableMessage = true;

                if (Owner.From.UseSkill(SkillName.AnimalTaming))
                {
                    Owner.From.Target.Invoke(Owner.From, m_Mobile);
                }

                AnimalTaming.DisableMessage = false;
                Owner.From.TargetLocked = false;
            }
        }

        private class DeathAdderCharmTarget : Target
        {
            private readonly BaseCreature m_Charmed;

            public DeathAdderCharmTarget(BaseCreature charmed) : base(-1, false, TargetFlags.Harmful) => m_Charmed = charmed;

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!m_Charmed.DeathAdderCharmable || m_Charmed.Combatant != null || !from.CanBeHarmful(m_Charmed, false))
                {
                    return;
                }

                if (!(SummonFamiliarSpell.Table.TryGetValue(from, out var bc) && (bc as DeathAdder)?.Deleted == false))
                {
                    return;
                }

                if (!(targeted is Mobile targ && from.CanBeHarmful(targ, false)))
                {
                    return;
                }

                from.RevealingAction();
                from.DoHarmful(targ, true);

                m_Charmed.Combatant = targ;

                if (m_Charmed.AIObject != null)
                {
                    m_Charmed.AIObject.Action = ActionType.Combat;
                }
            }
        }

        private class DeleteTimer : Timer
        {
            private readonly Mobile m;

            public DeleteTimer(Mobile creature, TimeSpan delay) : base(delay)
            {
                m = creature;
            }

            protected override void OnTick()
            {
                m.Delete();
            }
        }
    }

    public class LoyaltyTimer : Timer
    {
        private static readonly TimeSpan InternalDelay = TimeSpan.FromMinutes(5.0);

        private DateTime m_NextHourlyCheck;

        public LoyaltyTimer() : base(InternalDelay, InternalDelay) =>
            m_NextHourlyCheck = Core.Now + TimeSpan.FromHours(1.0);

        public static void Initialize()
        {
            new LoyaltyTimer().Start();
        }

        protected override void OnTick()
        {
            if (Core.Now < m_NextHourlyCheck)
            {
                return;
            }

            m_NextHourlyCheck = Core.Now + TimeSpan.FromHours(1.0);

            using var toRelease = PooledRefQueue<BaseCreature>.Create();

            // added array for wild creatures in house regions to be removed
            using var toRemove = PooledRefQueue<Mobile>.Create();

            foreach (var m in World.Mobiles.Values)
            {
                if (m is not BaseCreature c)
                {
                    continue;
                }

                if (c is BaseMount mount && mount.Rider != null)
                {
                    mount.OwnerAbandonTime = DateTime.MinValue;
                    continue;
                }

                if (c.IsDeadPet)
                {
                    var owner = c.ControlMaster;

                    if (!c.IsStabled && (owner?.Deleted != false || owner.Map != c.Map ||
                                         !owner.InRange(c, 12) || !c.CanSee(owner) || !c.InLOS(owner)))
                    {
                        if (c.OwnerAbandonTime == DateTime.MinValue)
                        {
                            c.OwnerAbandonTime = Core.Now;
                        }
                        else if (c.OwnerAbandonTime + c.BondingAbandonDelay <= Core.Now)
                        {
                            toRemove.Enqueue(c);
                        }
                    }
                    else
                    {
                        c.OwnerAbandonTime = DateTime.MinValue;
                    }
                }
                else if (c.Controlled && c.Commandable)
                {
                    c.OwnerAbandonTime = DateTime.MinValue;

                    if (c.Map != Map.Internal)
                    {
                        c.Loyalty -= BaseCreature.MaxLoyalty / 10;

                        if (c.Loyalty < BaseCreature.MaxLoyalty / 10)
                        {
                            c.Say(1043270, c.Name); // * ~1_NAME~ looks around desperately *
                            c.PlaySound(c.GetIdleSound());
                        }

                        if (c.Loyalty <= 0)
                        {
                            toRelease.Enqueue(c);
                        }
                    }
                }

                // added lines to check if a wild creature in a house region has to be removed or not
                if (!c.Controlled && !c.IsStabled && (c.Region.IsPartOf<HouseRegion>() && c.CanBeDamaged() ||
                                                      c.RemoveIfUntamed && c.Spawner == null))
                {
                    c.RemoveStep++;

                    if (c.RemoveStep >= 20)
                    {
                        toRemove.Enqueue(c);
                    }
                }
                else
                {
                    c.RemoveStep = 0;
                }
            }

            while (toRelease.Count > 0)
            {
                var c = toRelease.Dequeue();

                c.Say(1043255, c.Name); // ~1_NAME~ appears to have decided that is better off without a master!
                c.Loyalty = BaseCreature.MaxLoyalty; // Wonderfully Happy
                c.IsBonded = false;
                c.BondingBegin = DateTime.MinValue;
                c.OwnerAbandonTime = DateTime.MinValue;
                c.ControlTarget = null;
                // This will prevent no release of creatures left alone with AI disabled (and consequent bug of Followers)
                c.AIObject.DoOrderRelease();
                c.DropBackpack();
            }

            while (toRemove.Count > 0)
            {
                toRemove.Dequeue().Delete();
            }
        }
    }
}
