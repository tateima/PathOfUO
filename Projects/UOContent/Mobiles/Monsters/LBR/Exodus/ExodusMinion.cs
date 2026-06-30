using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class ExodusMinion : BaseCreature
    {
        [Constructible]
        public ExodusMinion() : base(AIType.AI_Melee)
        {
            Body = 0x2F5;

            LevelRange = [48, 68];
            StrPerLevel = [4, 5];
            IntPerLevel = [4, 5];
            DexPerLevel = [4, 5];
            ResistancePerLevel = [1, 2];

            SetStr(86, 105);
            SetDex(60, 80);
            SetInt(62, 110);

            SetHits(150, 180);

            SetDamage(5, 10);

            SetResistance(ResistanceType.Physical, 5, 35);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Tactics, 53.0, 63.5);
            SetSkill(SkillName.Wrestling, 53.0, 63.5);

            Fame = 18000;
            Karma = -18000;
            VirtualArmor = 65;

            PackItem(new PowerCrystal());
            PackItem(new ArcaneGem());
            PackItem(new ClockworkAssembly());

            switch (Utility.Random(3))
            {
                case 0:
                    {
                        PackItem(new PowerCrystal());
                        break;
                    }
                case 1:
                    {
                        PackItem(new ArcaneGem());
                        break;
                    }
                case 2:
                    {
                        PackItem(new ClockworkAssembly());
                        break;
                    }
            }
        }

        public override string CorpseName => "a minion's corpse";
        public override bool IsScaredOfScaryThings => false;
        public override bool IsScaryToPets => true;

        public override string DefaultName => "an exodus minion";

        public override bool AutoDispel => true;
        public override bool BardImmune => !Core.AOS;
        public override Poison PoisonImmune => Poison.Lethal;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Rich);
        }

        public override int GetIdleSound() => 0x218;

        public override int GetAngerSound() => 0x26C;

        public override int GetDeathSound() => 0x211;

        public override int GetAttackSound() => 0x232;

        public override int GetHurtSound() => 0x140;

        private static MonsterAbility[] _abilities =
        {
            // OSI changed the ability some time around UOML
            Core.ML ? MonsterAbilities.EnergyBoltCounter : MonsterAbilities.MagicalBarrier
        };

        public override MonsterAbility[] GetMonsterAbilities() => _abilities;
    }
}
