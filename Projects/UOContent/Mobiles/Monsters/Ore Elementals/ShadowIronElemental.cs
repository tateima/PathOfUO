using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class ShadowIronElemental : BaseCreature
    {
        public ShadowIronElemental() : this(2)
        {

        }
        [Constructible]
        public ShadowIronElemental(int oreAmount = 2) : base(AIType.AI_Melee)
        {
            Body = 111;
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

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 1, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 23;

            PackItem(new ShadowIronOre(oreAmount)
            {
                ItemID = 0x19B9
            });
        }

        public override string CorpseName => "an ore elemental corpse";
        public override string DefaultName => "a shadow iron elemental";

        public override bool AutoDispel => true;
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 1;
        public override Poison PoisonImmune => Poison.Deadly;
        public override bool BreathImmune => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems, 2);
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from is BaseCreature bc && (bc.Controlled || bc.BardTarget == this))
            {
                damage = 0; // Immune to pets and provoked creatures
            }
        }

        public override void AlterDamageScalarFrom(Mobile caster, ref double scalar)
        {
            scalar = 0.0; // Immune to magic
        }

        public override void AlterSpellDamageFrom(Mobile from, ref int damage)
        {
            damage = 0;
        }
    }
}
