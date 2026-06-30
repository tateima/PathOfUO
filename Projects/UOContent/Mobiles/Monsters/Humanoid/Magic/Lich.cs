using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Lich : BaseCreature
    {
        [Constructible]
        public Lich() : base(AIType.AI_Mage)
        {
            Body = 24;
            BaseSoundID = 0x3E9;

            LevelRange = [18, 38];
            StrPerLevel = [1, 3];
            IntPerLevel = [1, 5];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [2, 3];

            SetStr(30, 50);
            SetDex(10, 25);
            SetInt(50, 100);
            SetHits(70, 80);
            SetDamage(1, 5);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Cold, 40);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 5, 20);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.Necromancy, 40.1, 50.0);
            SetSkill(SkillName.SpiritSpeak, 40.1, 50.0);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.Meditation, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 50;
            PackItem(new GnarledStaff());
            PackNecroReg(17, 24);
        }

        public override string CorpseName => "a lich's corpse";
        public override string DefaultName => "a lich";

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override bool CanRummageCorpses => true;
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 3;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, 2);
        }
    }
}
