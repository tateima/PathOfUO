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

        public static OppositionGroup DarknessAndLight { get; } = new(
            new[]
            {
                new[]
                {
                    typeof(MageGuard),
                    typeof(NobleLord),
                    typeof(WarriorGuard),
                    typeof(ArcherGuard),
                    typeof(Samurai),
                    typeof(Ninja),
                    typeof(Wisp),
                    typeof(EtherealWarrior),
                    typeof(Kirin),
                    typeof(LordOaks),
                    typeof(Silvani),
                    typeof(Doppleganger),
                    typeof(Celestial),
                    typeof(Centaur),
                    typeof(Pixie),
                    typeof(Treefellow),
                    typeof(Unicorn),
                    typeof(MLDryad),
                    typeof(Satyr),
                    typeof(Guardian),
                    typeof(HeadlessOne)
                },
                new []{
                    typeof(DarkWisp),
                    typeof(AncientLich),
                    typeof(LichLord),
                    typeof(DarknightCreeper),
                    typeof(RevenantLion),
                    typeof(Revenant),
                    typeof(PatchworkSkeleton),
                    typeof(KhaldunRevenant),
                    typeof(WailingBanshee),
                    typeof(LadyOfTheSnow),
                    typeof(SkeletalDragon),
                    typeof(Bogle),
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
                    typeof(VampireBat),
                    typeof(RestlessSoul),
                    typeof(WailingBanshee),
                    typeof(RottingCorpse),
                    typeof(SpectralArmour),
                    typeof(Lich)
                }
            }
        );
        public static OppositionGroup ChaosAndOrder { get; } = new(
            new[]
            {
                new[]
                {
                    typeof(GreaterDragon),
                    typeof(AncientWyrm),
                    typeof(DeepSeaSerpent),
                    typeof(Dragon),
                    typeof(Leviathan),
                    typeof(SeaSerpent),
                    typeof(CrystalSeaSerpent),
                    typeof(SerpentineDragon),
                    typeof(ShadowWyrm),
                    typeof(WhiteWyrm),
                    typeof(Kraken),
                    typeof(SilverSerpent),
                    typeof(Drake),
                    typeof(OphidianArchmage),
                    typeof(OphidianKnight),
                    typeof(OphidianMage),
                    typeof(OphidianMatriarch),
                    typeof(OphidianWarrior),
                    typeof(Harpy),
                    typeof(StoneHarpy),
                    typeof(Lizardman),
                    typeof(LavaLizard),
                    typeof(Wyvern),
                    typeof(Alligator),
                    typeof(GiantSerpent),
                    typeof(IceSerpent),
                    typeof(IceSnake),
                    typeof(Snake),
                    typeof(LavaSnake)
                },
                new []
                {
                    typeof(BoneDemon),
                    typeof(AbysmalHorror),
                    typeof(CrystalDaemon),
                    typeof(GoreFiend),
                    typeof(MoundOfMaggots),
                    typeof(ArcaneDaemon),
                    typeof(Balron),
                    typeof(ElderGazer),
                    typeof(IceFiend),
                    typeof(ChaosDaemon),
                    typeof(ShadowFiend),
                    typeof(ChaosElemental),
                    typeof(HellHound),
                    typeof(HellSteed),
                    typeof(PredatorHellCat),
                    typeof(HellCat),
                    typeof(DemonKnight),
                    typeof(Devourer),
                    typeof(WandererOfTheVoid),
                    typeof(Daemon),
                    typeof(Gazer),
                    typeof(Gargoyle),
                    typeof(GargoyleDestroyer),
                    typeof(GargoyleEnforcer),
                    typeof(Imp),
                    typeof(Succubus),
                    typeof(EnslavedGargoyle),
                    typeof(StoneGargoyle),
                    typeof(GazerLarva),
                    typeof(SummonedDaemon)
                }
            }
        );

        public static Type[] SeaCreatures { get; } =
        {
            typeof(Dolphin),
            typeof(SeaSerpent),
            typeof(DeepSeaSerpent),
            typeof(SeaHorse),
            typeof(CrystalSeaSerpent),
            typeof(Kraken),
            typeof(WaterElemental)
        };

        public static Type[] LesserUndeadGroup { get; } =
        {
            typeof(Bogle),
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
            typeof(VampireBat),
            typeof(RestlessSoul),
            typeof(WailingBanshee),
            typeof(RottingCorpse),
            typeof(SpectralArmour),
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
            typeof(VeriteElemental),
            typeof(ChaosElemental)
        };

        public static Type[] LesserAbyssalGroup { get; } =
        {
            typeof(HellHound),
            typeof(HellSteed),
            typeof(PredatorHellCat),
            typeof(HellCat),
            typeof(DemonKnight),
            typeof(Devourer),
            typeof(WandererOfTheVoid),
            typeof(Daemon),
            typeof(Gazer),
            typeof(Gargoyle),
            typeof(GargoyleDestroyer),
            typeof(GargoyleEnforcer),
            typeof(Imp),
            typeof(Succubus),
            typeof(EnslavedGargoyle),
            typeof(StoneGargoyle),
            typeof(GazerLarva),
            typeof(SummonedDaemon)
        };
        public static Type[] LesserReptilianGroup { get; } =
        {

            typeof(SilverSerpent),
            typeof(Drake),
            typeof(OphidianArchmage),
            typeof(OphidianKnight),
            typeof(OphidianMage),
            typeof(OphidianMatriarch),
            typeof(OphidianWarrior),
            typeof(Harpy),
            typeof(StoneHarpy),
            typeof(Lizardman),
            typeof(LavaLizard),
            typeof(Wyvern),
            typeof(Alligator),
            typeof(GiantSerpent),
            typeof(IceSerpent),
            typeof(IceSnake),
            typeof(Snake),
            typeof(LavaSnake)
        };

        public static Type[] LesserHumanoidGroup { get; } =
        {
            typeof(MageGuard),
            typeof(NobleLord),
            typeof(WarriorGuard),
            typeof(ArcherGuard),
            typeof(Samurai),
            typeof(Ninja),
            typeof(Centaur),
            typeof(Pixie),
            typeof(Treefellow),
            typeof(Unicorn),
            typeof(Wisp),
            typeof(Satyr),
            typeof(Guardian),
            typeof(HeadlessOne)
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

        public Type[] this[int i] => m_Types[i];
    }
}

