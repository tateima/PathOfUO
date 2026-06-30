using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class ValoriteElemental : BaseCreature
    {
        [Constructible]
        public ValoriteElemental(int oreAmount = 2) : base(AIType.AI_Melee)
        {
            Body = 112;
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
            SetResistance(ResistanceType.Cold, 5, 25);
            SetResistance(ResistanceType.Poison, 1, 5);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 38;

            PackItem(new ValoriteOre(oreAmount)
            {
                ItemID = 0x19B9
            });
        }

        public override string CorpseName => "an ore elemental corpse";
        public override string DefaultName => "a valorite elemental";

        public override bool AutoDispel => true;
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 1;

        private static MonsterAbility[] _abilities = { MonsterAbilities.PoisonGasCounter, MonsterAbilities.DestroyEquipment };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 4);
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from is BaseCreature bc && (bc.Controlled || bc.BardTarget == this))
            {
                damage = 0; // Immune to pets and provoked creatures
            }
            else
            {
                damage /= 2; // 50% melee damage
            }
        }

        public override void CheckReflect(Mobile caster, ref bool reflect)
        {
            reflect = true; // Every spell is reflected back to the caster
        }
    }
}
