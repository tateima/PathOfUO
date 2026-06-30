using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class DullCopperElemental : BaseCreature
    {
        [Constructible]
        public DullCopperElemental(int oreAmount = 2) : base(AIType.AI_Melee)
        {
            Body = 110;
            BaseSoundID = 268;

            LevelRange = [25, 50];
            StrPerLevel = [2, 7];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(70, 95);
            SetDex(30, 75);
            SetInt(25, 50);
            SetHits(85, 130);
            SetDamage(2, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 15);
            SetResistance(ResistanceType.Poison, 1, 5);
            SetResistance(ResistanceType.Energy, 1, 5);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 20;

            PackItem(new DullCopperOre(oreAmount)
            {
                ItemID = 0x19B9
            });
        }

        public override string CorpseName => "an ore elemental corpse";
        public override string DefaultName => "a dull copper elemental";

        public override bool AutoDispel => true;
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 1;

        private static MonsterAbility[] _abilities = { MonsterAbilities.DeathExplosion };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems, 2);
        }
    }
}
