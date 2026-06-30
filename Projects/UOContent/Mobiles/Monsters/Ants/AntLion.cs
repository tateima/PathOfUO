using ModernUO.Serialization;
using Server.Engines.Plants;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class AntLion : BaseCreature
    {
        [Constructible]
        public AntLion() : base(AIType.AI_Melee)
        {
            Body = 787;
            BaseSoundID = 1006;

            LevelRange = [27, 47];
            StrPerLevel = [2, 6];
            IntPerLevel = [1, 7];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 3];
            SetStr(86, 105);
            SetDex(80, 129);
            SetInt(90, 125);

            SetHits(52, 89);

            SetDamage(4, 7);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 45.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 45;

            PackItem(new Bone(3));
            PackItem(new FertileDirt(Utility.RandomMinMax(1, 5)));

            if (Core.ML && Utility.RandomDouble() < .33)
            {
                PackItem(Seed.RandomPeculiarSeed(2));
            }

            var orepile = Utility.Random(4) switch
            {
                0 => (Item)new DullCopperOre(),
                1 => new ShadowIronOre(),
                2 => new CopperOre(),
                _ => new BronzeOre()
            };

            orepile.Amount = Utility.RandomMinMax(1, 10);
            orepile.ItemID = 0x19B9;
            PackItem(orepile);

            // TODO: skeleton
        }
        public override string CorpseName => "an ant lion corpse";
        public override string DefaultName => "an ant lion";

        public override int GetAngerSound() => 0x5A;

        public override int GetIdleSound() => 0x5A;

        public override int GetAttackSound() => 0x164;

        public override int GetHurtSound() => 0x187;

        public override int GetDeathSound() => 0x1BA;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2);
        }
    }
}
