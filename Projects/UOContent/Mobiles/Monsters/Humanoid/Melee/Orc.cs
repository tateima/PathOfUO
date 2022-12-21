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

            SetStr(96, 120);
            SetDex(81, 105);
            SetInt(36, 60);

            SetHits(58, 72);

            SetDamage(5, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 25, 30);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 50.1, 75.0);
            SetSkill(SkillName.Tactics, 55.1, 80.0);
            SetSkill(SkillName.Wrestling, 50.1, 70.0);

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
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

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
