using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class BlackSolenQueen : BaseCreature
    {
        [SerializableField(0, setter: "private")]
        private bool _burstSac;

        [Constructible]
        public BlackSolenQueen() : base(AIType.AI_Melee)
        {
            Body = 807;
            BaseSoundID = 959;
            Hue = 0x453;

            LevelRange = [27, 35];
            StrPerLevel = [2, 6];
            IntPerLevel = [1, 7];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 3];
            SetStr(56, 75);
            SetDex(50, 99);
            SetInt(60, 95);

            SetHits(52, 89);
            SetMana(0);

            SetDamage(4, 7);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 25);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 15, 20);

            SetSkill(SkillName.MagicResist, 45.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 45;

            SolenHelper.PackPicnicBasket(this);

            PackItem(new ZoogiFungus(Utility.RandomDouble() < 0.95 ? 5 : 25));

            if (Utility.RandomDouble() < 0.05)
            {
                PackItem(new BallOfSummoning());
            }
        }

        public override string CorpseName => "a solen queen corpse";

        public override string DefaultName => "a black solen queen";

        public override int GetAngerSound() => 0x259;

        public override int GetIdleSound() => 0x259;

        public override int GetAttackSound() => 0x195;

        public override int GetHurtSound() => 0x250;

        public override int GetDeathSound() => 0x25B;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
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

            if (!willKill)
            {
                if (!BurstSac)
                {
                    if (Hits < 50)
                    {
                        // The solen's acid sac is burst open!
                        PublicOverheadMessage(MessageType.Regular, 0x3B2, 1080038);
                        BurstSac = true;
                    }
                }
                else if (from != null && from != this && InRange(from, 1))
                {
                    // * The solen's damaged acid sac squirts acid! *
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, 1080060);
                    SpillAcid(from, 1);
                }
            }

            base.OnDamage(amount, from, willKill);
        }

        public override bool OnBeforeDeath()
        {
            SpillAcid(4);

            return base.OnBeforeDeath();
        }
    }
}
