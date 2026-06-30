using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class ExodusOverseer : BaseCreature
    {
        [Constructible]
        public ExodusOverseer() : base(AIType.AI_Melee)
        {
            Body = 0x2F4;

            LevelRange = [65, 78];
            StrPerLevel = [4, 5];
            IntPerLevel = [4, 5];
            DexPerLevel = [4, 5];
            ResistancePerLevel = [2, 3];

            SetStr(106, 145);
            SetDex(80, 100);
            SetInt(92, 130);

            SetHits(230, 280);

            SetDamage(3, 13);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 15, 35);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Tactics, 53.0, 63.5);
            SetSkill(SkillName.Wrestling, 53.0, 63.5);

            Fame = 10000;
            Karma = -10000;
            VirtualArmor = 50;

            if (Utility.Random(2) == 0)
            {
                PackItem(new PowerCrystal());
            }
            else
            {
                PackItem(new ArcaneGem());
            }
        }

        public override string CorpseName => "an overseer's corpse";

        public override bool IsScaredOfScaryThings => false;
        public override bool IsScaryToPets => true;

        public override string DefaultName => "an exodus overseer";

        public override bool AutoDispel => true;
        public override bool BardImmune => !Core.AOS;
        public override Poison PoisonImmune => Poison.Lethal;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public override int GetIdleSound() => 0xFD;

        public override int GetAngerSound() => 0x26C;

        public override int GetDeathSound() => 0x211;

        public override int GetAttackSound() => 0x23B;

        public override int GetHurtSound() => 0x140;

        private static MonsterAbility[] _abilities =
        {
            // OSI changed the ability some time around UOML
            Core.ML ? MonsterAbilities.EnergyBoltCounter : MonsterAbilities.MagicalBarrier
        };

        public override MonsterAbility[] GetMonsterAbilities() => _abilities;
    }
}
