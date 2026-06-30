using ModernUO.Serialization;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Orc : BaseCreature
    {
        [Constructible]
        public Orc() : base(AIType.AI_Melee)
        {
            Name = NameList.RandomName("orc");
            Body = 17;
            BaseSoundID = 0x45A;

            LevelRange = [3, 8];
            StrPerLevel = [1, 3];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 2];
            SetStr(15, 30);
            SetDex(19, 30);
            SetInt(10, 15);

            SetHits(20, 56);

            SetDamage(5, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 28;

            PackItem(
                Utility.Random(20) switch
                {
                    0 => new Scimitar(),
                    1 => new Katana(),
                    2 => new WarMace(),
                    3 => new WarHammer(),
                    4 => new Kryss(),
                    5 => new Pitchfork(),
                    _ => null // 6-19
                }
            );

            PackItem(new ThighBoots());

            PackItem(
                Utility.Random(3) switch
                {
                    0 => new Ribs(),
                    1 => new Shaft(),
                    _ => new Candle() // 2
                }
            );

            if (Utility.RandomDouble() < 0.2)
            {
                PackItem(new BolaBall());
            }
        }

        public override string CorpseName => "an orcish corpse";
        public override InhumanSpeech SpeechType => InhumanSpeech.Orc;

        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 1;
        public override int Meat => 1;
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.SavagesAndOrcs };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
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
