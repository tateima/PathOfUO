using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class GargoyleEnforcer : BaseCreature
    {
        [Constructible]
        public GargoyleEnforcer() : base(AIType.AI_Mage)
        {
            Body = 0x2F2;
            BaseSoundID = 0x174;

            LevelRange = [10, 20];
            StrPerLevel = [3, 7];
            IntPerLevel = [4, 7];
            DexPerLevel = [3, 8];
            ResistancePerLevel = [1, 2];

            SetStr(70, 100);
            SetDex(20, 65);
            SetInt(25, 70);
            SetHits(75, 100);

            SetDamage(2, 5);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 45.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);
            SetSkill(SkillName.Swords, 45.1, 55.0);
            SetSkill(SkillName.Anatomy, 45.1, 55.0);
            SetSkill(SkillName.Magery, 45.1, 55.0);
            SetSkill(SkillName.EvalInt, 45.1, 55.0);
            SetSkill(SkillName.Meditation, 45.1, 55.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 50;

            if (Utility.RandomDouble() < 0.2)
            {
                PackItem(new GargoylesPickaxe());
            }
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a gargoyle corpse";

        public override string DefaultName => "a gargoyle enforcer";

        public override bool CanFly => true;

        public override int Meat => 1;

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.WhirlwindAttack;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);
        }
    }
}
