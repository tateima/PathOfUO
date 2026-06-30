using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class HordeMinion : BaseCreature
    {
        [Constructible]
        public HordeMinion() : base(AIType.AI_Melee)
        {
            Body = 776;
            BaseSoundID = 357;
            LevelRange = [1, 6];
            StrPerLevel = [1, 3];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [1, 2];

            SetStr(16, 40);
            SetDex(31, 60);
            SetInt(11, 25);

            SetHits(10, 24);

            SetDamage(4, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Fire, 1, 5);

            SetSkill(SkillName.MagicResist, 10.0);
            SetSkill(SkillName.Tactics, 15.1, 35.0);
            SetSkill(SkillName.Wrestling, 25.1, 40.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 18;

            AddItem(new LightSource());

            PackItem(new Bone(3));
            // TODO: Body parts
        }

        public override string CorpseName => "a horde minion corpse";
        public override string DefaultName => "a horde minion";

        public override int GetIdleSound() => 338;

        public override int GetAngerSound() => 338;

        public override int GetDeathSound() => 338;

        public override int GetAttackSound() => 406;

        public override int GetHurtSound() => 194;
    }
}
