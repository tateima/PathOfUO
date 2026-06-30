using ModernUO.Serialization;
using Server.Engines.Plants;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Bogling : BaseCreature
    {
        [Constructible]
        public Bogling() : base(AIType.AI_Melee)
        {
            Body = 779;
            BaseSoundID = 422;
            LevelRange = [2, 10];
            StrPerLevel = [1, 2];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 2];
            SetStr(16, 30);
            SetDex(20, 40);
            SetInt(21, 45);

            SetHits(58, 65);

            SetDamage(1, 4);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 450;
            Karma = -450;

            VirtualArmor = 28;

            PackItem(new Log(4));
            PackItem(new Seed());
        }

        public override string CorpseName => "a plant corpse";
        public override string DefaultName => "a bogling";

        public override int Hides => 6;
        public override int Meat => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }
    }
}
