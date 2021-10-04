using System;
using Server.Mobiles;

namespace Server
{
    public class OppositionGroup
    {
        private readonly Type[][] m_Types;

        public OppositionGroup(Type[][] types) => m_Types = types;

        public static OppositionGroup TerathansAndOphidians { get; } = new(
            new[]
            {
                new[]
                {
                    typeof(TerathanAvenger),
                    typeof(TerathanDrone),
                    typeof(TerathanMatriarch),
                    typeof(TerathanWarrior)
                },
                new[]
                {
                    typeof(OphidianArchmage),
                    typeof(OphidianKnight),
                    typeof(OphidianMage),
                    typeof(OphidianMatriarch),
                    typeof(OphidianWarrior)
                }
            }
        );

        public static OppositionGroup SavagesAndOrcs { get; } = new(
            new[]
            {
                new[]
                {
                    typeof(Orc),
                    typeof(OrcBomber),
                    typeof(OrcBrute),
                    typeof(OrcCaptain),
                    typeof(OrcishLord),
                    typeof(OrcishMage),
                    typeof(SpawnedOrcishLord)
                },
                new[]
                {
                    typeof(Savage),
                    typeof(SavageRider),
                    typeof(SavageRidgeback),
                    typeof(SavageShaman)
                }
            }
        );

        public static OppositionGroup FeyAndUndead { get; } = new(
            new[]
            {
                new[]
                {
                    typeof(Centaur),
                    typeof(EtherealWarrior),
                    typeof(Kirin),
                    typeof(LordOaks),
                    typeof(Pixie),
                    typeof(Silvani),
                    typeof(Unicorn),
                    typeof(Wisp),
                    typeof(Treefellow),
                    typeof(MLDryad),
                    typeof(Satyr)
                },
                UndeadGroup
            }
        );

        public static Type[] UndeadGroup { get; } =
                {
                    typeof(AncientLich),
                    typeof(Bogle),
                    typeof(LichLord),
                    typeof(Shade),
                    typeof(Spectre),
                    typeof(Wraith),
                    typeof(BoneKnight),
                    typeof(Ghoul),
                    typeof(Mummy),
                    typeof(SkeletalKnight),
                    typeof(Skeleton),
                    typeof(Zombie),
                    typeof(BoneMagi),
                    typeof(SkeletalMage),
                    typeof(ShadowKnight),
                    typeof(DarknightCreeper),
                    typeof(RevenantLion),
                    typeof(Revenant),
                    typeof(PatchworkSkeleton),
                    typeof(VampireBat),
                    typeof(RestlessSoul),
                    typeof(KhaldunRevenant),
                    typeof(WailingBanshee),
                    typeof(LadyOfTheSnow),
                    typeof(RottingCorpse),
                    typeof(SpectralArmour),
                    typeof(SkeletalDragon),
                    typeof(Lich)
                };

        public static Type[] ElementalGroup { get; } =
              {
                    typeof(AcidElemental),
                    typeof(AirElemental),
                    typeof(BloodElemental),
                    typeof(SummonedAirElemental),
                    typeof(Efreet),
                    typeof(FireElemental),
                    typeof(SummonedFireElemental),
                    typeof(IceElemental),
                    typeof(PoisonElemental),
                    typeof(WaterElemental),
                    typeof(SummonedWaterElemental),
                    typeof(EarthElemental),
                    typeof(SummonedEarthElemental),
                    typeof(SnowElemental),
                    typeof(AgapiteElemental),
                    typeof(BronzeElemental),
                    typeof(CopperElemental),
                    typeof(DullCopperElemental),
                    typeof(GoldenElemental),
                    typeof(ShadowIronElemental),
                    typeof(ValoriteElemental),
                    typeof(CrystalElemental),
                    typeof(VeriteElemental)
                };
        public static Type[] AbyssalGroup { get; } =
            {
                    typeof(BoneDemon),
                    typeof(HellHound),
                    typeof(HellSteed),
                    typeof(PredatorHellCat),
                    typeof(HellCat),
                    typeof(AbysmalHorror),
                    typeof(CrystalDaemon),
                    typeof(DemonKnight),
                    typeof(GoreFiend),
                    typeof(Devourer),
                    typeof(MoundOfMaggots),
                    typeof(WandererOfTheVoid),
                    typeof(ArcaneDaemon),
                    typeof(Balron),
                    typeof(Daemon),
                    typeof(ElderGazer),
                    typeof(Gazer),
                    typeof(FireGargoyle),
                    typeof(Gargoyle),
                    typeof(GargoyleDestroyer),
                    typeof(GargoyleEnforcer),
                    typeof(IceFiend),
                    typeof(Imp),
                    typeof(Succubus),
                    typeof(ChaosDaemon),
                    typeof(EnslavedGargoyle),
                    typeof(StoneGargoyle),
                    typeof(GazerLarva),
                    typeof(ShadowFiend),
                    typeof(SummonedDaemon)
                };
        public static Type[] ReptilianGroup { get; } =
            {
                    typeof(GreaterDragon),
                    typeof(AncientWyrm),
                    typeof(DeepSeaSerpent),
                    typeof(Dragon),
                    typeof(Drake),
                    typeof(Leviathan),
                    typeof(OphidianArchmage),
                    typeof(OphidianKnight),
                    typeof(OphidianMage),
                    typeof(OphidianMatriarch),
                    typeof(OphidianWarrior),
                    typeof(SeaSerpent),
                    typeof(CrystalSeaSerpent),
                    typeof(SerpentineDragon),
                    typeof(ShadowWyrm),
                    typeof(WhiteWyrm),
                    typeof(Harpy),
                    typeof(StoneHarpy),
                    typeof(Kraken),
                    typeof(Lizardman),
                    typeof(LavaLizard),
                    typeof(Wyvern),
                    typeof(Alligator),
                    typeof(GiantSerpent),
                    typeof(IceSerpent),
                    typeof(IceSnake),
                    typeof(Snake),
                    typeof(LavaSnake),
                    typeof(SilverSerpent)
                };

        public static Type[] HumanoidGroup{ get; } =
            {
                    typeof(Wanderer),
                    typeof(ArcticOgreLord),
                    typeof(OgreLord),
                    typeof(Ogre),
                    typeof(Troll),
                    typeof(Orc),
                    typeof(OrcBomber),
                    typeof(OrcBrute),
                    typeof(OrcCaptain),
                    typeof(OrcishLord),
                    typeof(OrcishMage),
                    typeof(SpawnedOrcishLord),
                    typeof(SummonedOrcBrute),
                    typeof(Brigand),
                    typeof(ElfBrigand),
                    typeof(Cyclops),
                    typeof(Titan),
                    typeof(Doppleganger),
                    typeof(SummonedDoppleganger),
                    typeof(Ettin),
                    typeof(Cursed),
                    typeof(Executioner),
                    typeof(FrostTroll),
                    typeof(Guardian),
                    typeof(HeadlessOne),
                    typeof(Juggernaut),
                    typeof(KhaldunSummoner),
                    typeof(KhaldunZealot),
                    typeof(Ratman),
                    typeof(RatmanArcher),
                    typeof(RatmanMage),
                    typeof(Savage),
                    typeof(SavageRider),
                    typeof(SavageShaman),
                    typeof(EvilHealer),
                    typeof(EvilMage),
                    typeof(EvilMageLord),
                    typeof(EvilWanderingHealer),
                    typeof(Executioner)
                };

        public static Type[] AnimalGroup { get; } =
          {
                    typeof(BlackBear),
                    typeof(BrownBear),
                    typeof(GrizzlyBear),
                    typeof(PolarBear),
                    typeof(Chicken),
                    typeof(Crane),
                    typeof(Eagle),
                    typeof(Phoenix),
                    typeof(DireWolf),
                    typeof(GreyWolf),
                    typeof(TimberWolf),
                    typeof(WhiteWolf),
                    typeof(Bull),
                    typeof(Cow),
                    typeof(Cougar),
                    typeof(Panther),
                    typeof(SnowLeopard),
                    typeof(Boar),
                    typeof(BullFrog),
                    typeof(Dolphin),
                    typeof(Gaman),
                    typeof(GiantToad),
                    typeof(Goat),
                    typeof(Gorilla),
                    typeof(GreatHart),
                    typeof(Hind),
                    typeof(Llama),
                    typeof(MountainGoat),
                    typeof(PackHorse),
                    typeof(PackLlama),
                    typeof(Pig),
                    typeof(Sheep),
                    typeof(Walrus),
                    typeof(Beetle),
                    typeof(DesertOstard),
                    typeof(ForestOstard),
                    typeof(FrenziedOstard),
                    typeof(Horse),
                    typeof(RidableLlama),
                    typeof(SeaHorse),
                    typeof(GiantRat),
                    typeof(JackRabbit),
                    typeof(Rabbit),
                    typeof(Bird),
                    typeof(Cat),
                    typeof(Dog),
                    typeof(Rat)
                };
        public static Type[] ArachnidGroup { get; } =
            {
                    typeof(DreadSpider),
                    typeof(TerathanAvenger),
                    typeof(TerathanDrone),
                    typeof(TerathanMatriarch),
                    typeof(TerathanWarrior),
                    typeof(FrostSpider),
                    typeof(GiantBlackWidow),
                    typeof(GiantSpider)
                };
        public bool IsEnemy(object from, object target)
        {
            var fromGroup = IndexOf(from);
            var targGroup = IndexOf(target);

            return fromGroup != -1 && targGroup != -1 && fromGroup != targGroup;
        }

        public int IndexOf(object obj)
        {
            if (obj == null)
            {
                return -1;
            }

            var type = obj.GetType();

            for (var i = 0; i < m_Types.Length; ++i)
            {
                var group = m_Types[i];

                var contains = false;
                if (group != null)
                {
                    for (var j = 0; !contains && j < group.Length; ++j)
                    {
                        contains = group[j].IsAssignableFrom(type);
                    }
                }
               
                if (contains)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}

