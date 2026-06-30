using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class VampireBat : BaseCreature
    {
        [Constructible]
        public VampireBat() : base(AIType.AI_Melee)
        {
            Body = 317;
            BaseSoundID = 0x270;
            LevelRange = [3, 17];
            StrPerLevel = [3, 4];
            IntPerLevel = [2, 4];
            DexPerLevel = [3, 4];
            ResistancePerLevel = [1, 2];
            SetStr(31, 70);
            SetDex(21, 55);
            SetInt(16, 30);

            SetHits(55, 66);

            SetDamage(1, 6);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Poison, 20);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 10, 30);

            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.Wrestling, 45.1, 50.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 14;
        }

        public override string CorpseName => "a vampire bat corpse";
        public override string DefaultName => "a vampire bat";

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
        }

        public override int GetIdleSound() => 0x29B;
    }
}
