using ModernUO.Serialization;
using System;
using Server.Ethics;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class DarkWisp : BaseCreature
    {
        [Constructible]
        public DarkWisp() : base(AIType.AI_Mage, FightMode.Aggressor)
        {
            Body = 165;
            BaseSoundID = 466;

            LevelRange = [10, 50];
            StrPerLevel = [1, 2];
            IntPerLevel = [2, 9];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(36, 65);
            SetDex(70, 130);
            SetInt(82, 140);

            SetHits(50, 76);

            SetDamage(3, 7);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 10, 30);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 40;

            AddItem(new LightSource());
        }

        public override string CorpseName => "a wisp corpse";
        public override InhumanSpeech SpeechType => InhumanSpeech.Wisp;

        public override Ethic EthicAllegiance => Ethic.Evil;

        public override TimeSpan ReacquireDelay => TimeSpan.FromSeconds(1.0);

        public override string DefaultName => "a wisp";

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight, OppositionGroup.CelestialsAndDaemons };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
        }
    }
}
