using Server.Items;
using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class FrostTroll : BaseCreature
    {
        [Constructible]
        public FrostTroll() : base(AIType.AI_Melee)
        {
            Body = 55;
            BaseSoundID = 461;

            LevelRange = [15, 22];
            StrPerLevel = [1, 4];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 3];
            ResistancePerLevel = [1, 3];

            SetStr(70, 185);
            SetDex(20, 65);
            SetInt(15, 40);
            SetHits(95, 130);
            SetDamage(4, 12);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Cold, 25);

            SetResistance(ResistanceType.Physical, 15, 35);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 50;

            PackItem(new DoubleAxe()); // TODO: Weapon??
        }

        public override string CorpseName => "a frost troll corpse";
        public override string DefaultName => "a frost troll";

        public override int Meat => 2;
        public override int TreasureMapLevel => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems);
        }
    }
}
