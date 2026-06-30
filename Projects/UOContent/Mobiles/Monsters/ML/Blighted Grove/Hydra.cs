using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Hydra : BaseCreature
    {
        [Constructible]
        public Hydra() : base(AIType.AI_Melee)
        {
            Body = 0x109;
            BaseSoundID = 0x16A;

            LevelRange = [65, 75];
            StrPerLevel = [2, 7];
            IntPerLevel = [1, 3];
            DexPerLevel = [1, 4];
            ResistancePerLevel = [1, 2];

            SetStr(116, 165);
            SetDex(50, 80);
            SetInt(62, 110);

            SetHits(150, 220);

            SetDamage(3, 11);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 10);
            SetDamageType(ResistanceType.Cold, 10);
            SetDamageType(ResistanceType.Poison, 10);
            SetDamageType(ResistanceType.Energy, 10);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.Wrestling, 40.4, 50.1);
            SetSkill(SkillName.Tactics, 40.4, 50.1);
            SetSkill(SkillName.MagicResist, 40.4, 50.1);
            SetSkill(SkillName.Anatomy, 40.4, 50.1);

            Fame = 20000;
            Karma = -20000;
        }

        public override string CorpseName => "a hydra corpse";
        public override string DefaultName => "a hydra";
        public override int Hides => 40;
        public override int Meat => 19;
        public override int TreasureMapLevel => 5;

        private static MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new HydraScale());

            /*
            // TODO: uncomment once added
            if (Utility.RandomDouble() < 0.2)
              c.DropItem( new ParrotItem() );

            if (Utility.RandomDouble() < 0.05)
              c.DropItem( new ThorvaldsMedallion() );
            */
        }
    }
}
