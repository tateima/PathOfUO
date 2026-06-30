using ModernUO.Serialization;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class OrcishLord : BaseCreature
    {
        [Constructible]
        public OrcishLord() : base(AIType.AI_Melee)
        {
            Body = 138;
            BaseSoundID = 0x45A;

            LevelRange = [10, 15];
            StrPerLevel = [2, 5];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 2];

            SetStr(25, 45);
            SetDex(19, 45);
            SetInt(10, 15);

            SetHits(30, 66);

            SetDamage(7, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Swords, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 2500;
            Karma = -2500;

            PackItem(
                Utility.Random(5) switch
                {
                    0 => new Lockpick(),
                    1 => new MortarPestle(),
                    2 => new Bottle(),
                    3 => new RawRibs(),
                    _ => new Shovel() // 4
                }
            );

            PackItem(new RingmailChest());

            if (Utility.RandomDouble() < 0.3)
            {
                PackItem(Loot.RandomPossibleReagent());
            }

            if (Utility.RandomDouble() < 0.2)
            {
                PackItem(new BolaBall());
            }
        }

        public override string CorpseName => "an orcish corpse";
        public override InhumanSpeech SpeechType => InhumanSpeech.Orc;

        public override string DefaultName => "an orcish lord";

        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 1;
        public override int Meat => 1;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.SavagesAndOrcs };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.Average);
            // TODO: evil orc helm
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
