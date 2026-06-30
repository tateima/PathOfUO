using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class VeriteElemental : BaseCreature
    {
        [Constructible]
        public VeriteElemental(int oreAmount = 2) : base(AIType.AI_Melee)
        {
            Body = 113;
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

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 10, 25);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 35;

            PackItem(new VeriteOre(oreAmount)
            {
                ItemID = 0x19B9
            });
        }

        public override string CorpseName => "an ore elemental corpse";
        public override string DefaultName => "a verite elemental";

        public override bool AutoDispel => true;
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 1;

        private static MonsterAbility[] _abilities = { MonsterAbilities.DestroyEquipment };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 2);
        }
    }
}
