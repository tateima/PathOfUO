using ModernUO.Serialization;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class OrcishMage : BaseCreature
    {
        public bool IsShaman { get; set; }
        [Constructible]
        public OrcishMage() : base(AIType.AI_Mage)
        {
            Body = 140;
            BaseSoundID = 0x45A;

            LevelRange = [5, 12];
            StrPerLevel = [1, 3];
            IntPerLevel = [2, 5];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 2];
            SetStr(20, 40);
            SetDex(19, 30);
            SetInt(50, 90);
            SetHits(50, 86);
            SetDamage(2, 4);
            SetDamageType(ResistanceType.Physical, 100);
            SetSkill(SkillName.Magery, 40.1, 50.0);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 30;

            PackReg(6);

            if (Utility.RandomDouble() < 0.05)
            {
                PackItem(new OrcishKinMask());
            }
        }

        public override string CorpseName => IsShaman ? "a dark orc corpse" : "a glowing orc corpse";
        public override InhumanSpeech SpeechType => InhumanSpeech.Orc;

        public override string DefaultName => IsShaman ? "an orcish shaman" : "an orcish mage";

        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 1;
        public override int Meat => 1;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.SavagesAndOrcs };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.LowScrolls);
        }

        public override bool IsEnemy(Mobile m) =>
            (!m.Player || m.FindItemOnLayer<OrcishKinMask>(Layer.Helm) == null) && base.IsEnemy(m);

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            if (aggressor.FindItemOnLayer(Layer.Helm) is OrcishKinMask item)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                item.Delete();
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
            }
        }
    }
}
