using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class ChaosDaemon : BaseCreature
    {
        [Constructible]
        public ChaosDaemon() : base(AIType.AI_Melee)
        {
            Body = 792;
            BaseSoundID = 0x3E9;
            LevelRange = [20, 30];
            StrPerLevel = [3, 6];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(56, 80);
            SetDex(121, 130);
            SetInt(32, 40);

            SetHits(91, 110);

            SetDamage(1, 8);

            SetDamageType(ResistanceType.Physical, 85);
            SetDamageType(ResistanceType.Fire, 15);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.Wrestling, 50.0, 60.5);

            Fame = 3000;
            Karma = -4000;

            VirtualArmor = 15;
        }

        public override string CorpseName => "a chaos daemon corpse";

        public override string DefaultName => "a chaos daemon";
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override WeaponAbility GetWeaponAbility() => WeaponAbility.CrushingBlow;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
        }
    }
}
