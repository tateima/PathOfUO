using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Gibberling : BaseCreature
    {
        [Constructible]
        public Gibberling() : base(AIType.AI_Melee)
        {
            Body = 307;
            BaseSoundID = 422;
            LevelRange = [5, 13];
            StrPerLevel = [2, 3];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 2];

            SetStr(33, 66);
            SetDex(44, 88);
            SetInt(22, 50);

            SetHits(55, 88);

            SetDamage(3, 4);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Fire, 40);
            SetDamageType(ResistanceType.Energy, 60);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 47.6, 52.5);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 27;
        }

        public override string CorpseName => "a gibberling corpse";

        public override string DefaultName => "a gibberling";

        public override int TreasureMapLevel => 1;

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.Dismount;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }
    }
}
