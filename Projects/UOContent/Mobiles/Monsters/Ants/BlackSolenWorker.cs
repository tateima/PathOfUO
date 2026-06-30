using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class BlackSolenWorker : BaseCreature
    {
        [Constructible]
        public BlackSolenWorker() : base(AIType.AI_Melee)
        {
            Body = 805;
            BaseSoundID = 959;
            Hue = 0x453;

            LevelRange = [13, 25];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [4, 9];
            ResistancePerLevel = [1, 2];
            SetStr(56, 85);
            SetDex(50, 99);
            SetInt(21, 45);

            SetHits(52, 89);
            SetMana(0);

            SetDamage(2, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 25);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 15, 20);

            SetSkill(SkillName.MagicResist, 45.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 28;

            PackGold(Utility.Random(100, 180));

            SolenHelper.PackPicnicBasket(this);

            PackItem(new ZoogiFungus());
        }

        public override string CorpseName => "a solen worker corpse";
        public override string DefaultName => "a black solen worker";

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
            if (SolenHelper.CheckBlackFriendship(m))
            {
                return false;
            }

            return base.IsEnemy(m);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            SolenHelper.OnBlackDamage(from);

            base.OnDamage(amount, from, willKill);
        }
    }
}
