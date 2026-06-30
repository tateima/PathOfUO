using ModernUO.Serialization;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class OrcCaptain : BaseCreature
    {
        [Constructible]
        public OrcCaptain() : base(AIType.AI_Melee)
        {
            Name = NameList.RandomName("orc");
            Body = 7;
            BaseSoundID = 0x45A;

            LevelRange = [8, 13];
            StrPerLevel = [4, 5];
            IntPerLevel = [2, 3];
            DexPerLevel = [7, 10];
            ResistancePerLevel = [1, 2];

            SetStr(25, 45);
            SetDex(19, 45);
            SetInt(10, 15);

            SetHits(30, 66);

            SetDamage(6, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 1, 3);
            SetResistance(ResistanceType.Energy, 1, 3);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Swords, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 34;

            // TODO: Skull?
            PackItem(
                Utility.Random(7) switch
                {
                    0 => new Arrow(),
                    1 => new Lockpick(),
                    2 => new Shaft(),
                    3 => new Ribs(),
                    4 => new Bandage(),
                    5 => new BeverageBottle(BeverageType.Wine),
                    _ => new Jug(BeverageType.Cider) // 6
                }
            );

            if (Core.AOS)
            {
                PackItem(Loot.RandomNecromancyReagent());
            }
        }

        public override string CorpseName => "an orcish corpse";
        public override InhumanSpeech SpeechType => InhumanSpeech.Orc;

        public override bool CanRummageCorpses => true;
        public override int Meat => 1;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.SavagesAndOrcs };

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // TODO: Check drop rate
            if (Utility.RandomDouble() < 0.05)
            {
                c.DropItem(new StoutWhip());
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager, 2);
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
