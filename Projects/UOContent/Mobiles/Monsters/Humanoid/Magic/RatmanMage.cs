using ModernUO.Serialization;
using Server.Misc;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class RatmanMage : BaseCreature
    {
        [Constructible]
        public RatmanMage() : base(AIType.AI_Mage)
        {
            Name = NameList.RandomName("ratman");
            Body = 0x8F;
            BaseSoundID = 437;

            LevelRange = [4, 9];
            StrPerLevel = [1, 3];
            IntPerLevel = [2, 5];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 2];
            SetStr(15, 20);
            SetDex(19, 20);
            SetInt(14, 59);

            SetHits(35, 60);

            SetDamage(2, 4);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Fire, 1, 10);
            SetResistance(ResistanceType.Cold, 1, 10);
            SetResistance(ResistanceType.Poison, 1, 10);
            SetResistance(ResistanceType.Energy, 1, 10);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Necromancy, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 7500;
            Karma = -7500;

            VirtualArmor = 44;

            PackReg(6);

            if (Utility.RandomDouble() < 0.02)
            {
                PackStatue();
            }
        }

        public override string CorpseName => "a glowing ratman corpse";
        public override InhumanSpeech SpeechType => InhumanSpeech.Ratman;

        public override bool CanRummageCorpses => true;
        public override int Meat => 1;
        public override int Hides => 8;
        public override HideType HideType => HideType.Spined;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.LowScrolls);
        }
    }
}
