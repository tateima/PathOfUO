using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class TerathanDrone : BaseCreature
    {
        [Constructible]
        public TerathanDrone() : base(AIType.AI_Melee)
        {
            Body = 71;
            BaseSoundID = 594;
            LevelRange = [5, 15];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [4, 9];
            ResistancePerLevel = [1, 2];
            SetStr(36, 65);
            SetDex(50, 99);
            SetInt(21, 45);

            SetHits(22, 39);
            SetMana(0);

            SetDamage(1, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 15);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 1, 5);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 1, 5);

            SetSkill(SkillName.Poisoning, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 30.1, 55.0);
            SetSkill(SkillName.Tactics, 30.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 2000;
            Karma = -2000;

            VirtualArmor = 24;

            PackItem(new SpidersSilk(2));
        }

        public override string CorpseName => "a terathan drone corpse";
        public override string DefaultName => "a terathan drone";

        public override int Meat => 4;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.TerathansAndOphidians };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
            // TODO: weapon?
        }
    }
}
