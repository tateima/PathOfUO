using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class DreadSpider : BaseCreature
    {
        [Constructible]
        public DreadSpider() : base(AIType.AI_Mage)
        {
            Body = 11;
            BaseSoundID = 1170;
            LevelRange = [16, 26];
            StrPerLevel = [2, 5];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 2];
            SetStr(60, 75);
            SetDex(25, 65);
            SetInt(5, 11);

            SetHits(35, 70);

            SetDamage(3, 8);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.EvalInt, 45.1, 50.0);
            SetSkill(SkillName.Magery, 45.1, 55.0);
            SetSkill(SkillName.Meditation, 45.1, 50.0);
            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 50.1, 55.0);
            SetSkill(SkillName.Wrestling, 45.1, 50);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 36;
            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 81.1;
            PackItem(new SpidersSilk(8));
        }
        public override bool CanCannibalise(Mobile target) => base.CanCannibalise(target) || target is GiantSpider or FrostSpider;
        public override string CorpseName => "a dread spider corpse";
        public override string DefaultName => "a dread spider";

        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Poison.Lethal;
        public override int TreasureMapLevel => 3;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
        }
    }
}
