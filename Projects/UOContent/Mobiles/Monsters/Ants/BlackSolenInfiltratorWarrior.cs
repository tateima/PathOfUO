using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class BlackSolenInfiltratorWarrior : BaseCreature
    {
        [Constructible]
        public BlackSolenInfiltratorWarrior() : base(AIType.AI_Melee)
        {
            Body = 806;
            BaseSoundID = 959;
            Hue = 0x453;

            LevelRange = [17, 27];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [4, 9];
            ResistancePerLevel = [1, 2];
            SetStr(56, 85);
            SetDex(50, 99);
            SetInt(21, 45);

            SetHits(62, 99);
            SetMana(0);

            SetDamage(2, 8);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Poison, 20);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 5, 20);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 10, 25);

            SetSkill(SkillName.MagicResist, 45.0, 50.0);
            SetSkill(SkillName.Tactics, 45.0, 50.0);
            SetSkill(SkillName.Wrestling, 45.0, 50.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 40;

            SolenHelper.PackPicnicBasket(this);

            PackItem(new ZoogiFungus(Utility.RandomDouble() < 0.05 ? 13 : 3));
        }

        public override string CorpseName => "a solen infiltrator corpse";
        public override string DefaultName => "a black solen infiltrator";

        public override int GetAngerSound() => 0xB5;

        public override int GetIdleSound() => 0xB5;

        public override int GetAttackSound() => 0x289;

        public override int GetHurtSound() => 0xBC;

        public override int GetDeathSound() => 0xE4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 4));
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
