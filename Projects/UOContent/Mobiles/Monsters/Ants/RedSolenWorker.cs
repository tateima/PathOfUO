using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class RedSolenWorker : BaseCreature
    {
        [Constructible]
        public RedSolenWorker() : base(AIType.AI_Melee)
        {
            Body = 781;
            BaseSoundID = 959;
            LevelRange = [5, 15];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [4, 9];
            ResistancePerLevel = [1, 2];

            SetStr(36, 65);
            SetDex(50, 99);
            SetInt(21, 45);

            SetHits(32, 49);

            SetDamage(5, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 20);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 7, 15);

            SetSkill(SkillName.MagicResist, 45.0, 50.0);
            SetSkill(SkillName.Tactics, 45.0, 50.0);
            SetSkill(SkillName.Wrestling, 45.0, 50.0);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 28;

            PackGold(Utility.Random(100, 180));

            SolenHelper.PackPicnicBasket(this);

            PackItem(new ZoogiFungus());
        }

        public override string CorpseName => "a solen worker corpse";
        public override string DefaultName => "a red solen worker";

        public override int GetAngerSound() => 0x269;

        public override int GetIdleSound() => 0x269;

        public override int GetAttackSound() => 0x186;

        public override int GetHurtSound() => 0x1BE;

        public override int GetDeathSound() => 0x8E;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 2));
        }

        public override bool IsEnemy(Mobile m)
        {
            if (SolenHelper.CheckRedFriendship(m))
            {
                return false;
            }

            return base.IsEnemy(m);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            SolenHelper.OnRedDamage(from);

            base.OnDamage(amount, from, willKill);
        }
    }
}
