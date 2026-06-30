using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Balron : BaseCreature
    {
        [Constructible]
        public Balron() : base(AIType.AI_Mage)
        {
            Name = NameList.RandomName("balron");
            Body = 40;
            BaseSoundID = 357;
            LevelRange = [60, 80];
            StrPerLevel = [3, 6];
            IntPerLevel = [3, 6];
            DexPerLevel = [3, 6];
            ResistancePerLevel = [2, 4];

            SetStr(126, 175);
            SetDex(70, 100);
            SetInt(82, 140);

            SetHits(230, 280);

            SetDamage(4, 12);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 30);
            SetResistance(ResistanceType.Energy, 10, 30);

            SetSkill(SkillName.Anatomy, 53.0, 63.5);
            SetSkill(SkillName.EvalInt, 53.0, 63.5);
            SetSkill(SkillName.Magery, 53.0, 63.5);
            SetSkill(SkillName.Meditation, 53.0, 63.5);
            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Tactics, 53.0, 63.5);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;

            PackItem(new Longsword());
        }

        public override string CorpseName => "a balron corpse";
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder, OppositionGroup.CelestialsAndDaemons };
        public override bool CanRummageCorpses => true;
        public override Poison PoisonImmune => Poison.Deadly;
        public override int TreasureMapLevel => 5;
        public override int Meat => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, 2);
        }
    }
}
