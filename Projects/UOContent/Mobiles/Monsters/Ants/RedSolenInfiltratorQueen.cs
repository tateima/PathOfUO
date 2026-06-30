using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class RedSolenInfiltratorQueen : BaseCreature
    {
        [Constructible]
        public RedSolenInfiltratorQueen() : base(AIType.AI_Melee)
        {
            Body = 783;
            BaseSoundID = 959;

            LevelRange = [29, 39];
            StrPerLevel = [2, 6];
            IntPerLevel = [1, 7];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 3];
            SetStr(66, 85);
            SetDex(60, 109);
            SetInt(70, 105);

            SetHits(52, 89);
            SetMana(0);

            SetDamage(4, 7);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            SetResistance(ResistanceType.Physical, 5, 30);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 15, 30);
            SetResistance(ResistanceType.Energy, 15, 20);

            SetSkill(SkillName.MagicResist, 45.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);

            Fame = 6500;
            Karma = -6500;

            VirtualArmor = 50;

            SolenHelper.PackPicnicBasket(this);

            PackItem(new ZoogiFungus(Utility.RandomDouble() < 0.95 ? 4 : 16));
        }

        public override string CorpseName => "a solen infiltrator corpse";
        public override string DefaultName => "a red solen infiltrator";

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
