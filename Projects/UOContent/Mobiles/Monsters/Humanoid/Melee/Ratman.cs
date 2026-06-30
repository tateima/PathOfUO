using ModernUO.Serialization;
using Server.Misc;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Ratman : BaseCreature
    {
        [Constructible]
        public Ratman() : base(AIType.AI_Melee)
        {
            Name = NameList.RandomName("ratman");
            Body = 42;
            BaseSoundID = 437;

            LevelRange = [4, 9];
            StrPerLevel = [2, 3];
            IntPerLevel = [2, 3];
            DexPerLevel = [3, 7];
            ResistancePerLevel = [1, 2];
            SetStr(15, 20);
            SetDex(29, 40);
            SetInt(14, 19);

            SetHits(35, 60);

            SetDamage(3, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Fire, 1, 10);
            SetResistance(ResistanceType.Cold, 1, 10);
            SetResistance(ResistanceType.Poison, 1, 10);
            SetResistance(ResistanceType.Energy, 1, 10);

            SetSkill(SkillName.MagicResist, 35.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 28;
        }

        public override string CorpseName => "a ratman's corpse";
        public override InhumanSpeech SpeechType => InhumanSpeech.Ratman;

        public override bool CanRummageCorpses => true;
        public override int Hides => 8;
        public override HideType HideType => HideType.Spined;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
            // TODO: weapon, misc
        }
    }
}
