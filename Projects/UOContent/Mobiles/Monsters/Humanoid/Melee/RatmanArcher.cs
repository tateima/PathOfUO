using ModernUO.Serialization;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class RatmanArcher : BaseCreature
    {
        [Constructible]
        public RatmanArcher() : base(AIType.AI_Archer)
        {
            Name = NameList.RandomName("ratman");
            Body = 0x8E;
            BaseSoundID = 437;

            LevelRange = [4, 7];
            StrPerLevel = [1, 2];
            IntPerLevel = [2, 3];
            DexPerLevel = [9, 15];
            ResistancePerLevel = [1, 2];
            SetStr(15, 25);
            SetDex(39, 50);
            SetInt(14, 25);

            SetHits(35, 80);

            SetDamage(5, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Fire, 1, 10);
            SetResistance(ResistanceType.Cold, 1, 10);
            SetResistance(ResistanceType.Poison, 1, 10);
            SetResistance(ResistanceType.Energy, 1, 10);

            SetSkill(SkillName.Anatomy, 40.1, 50.0);
            SetSkill(SkillName.Archery, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 6500;
            Karma = -6500;

            VirtualArmor = 56;

            AddItem(new Bow());
            PackItem(new Arrow(Utility.RandomMinMax(50, 70)));
        }

        public override string CorpseName => "a ratman archer corpse";
        public override InhumanSpeech SpeechType => InhumanSpeech.Ratman;

        public override bool CanRummageCorpses => true;
        public override int Hides => 8;
        public override HideType HideType => HideType.Spined;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }
    }
}
