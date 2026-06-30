using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [TypeAlias("Server.Mobiles.ArticOgreLord")]
    [SerializationGenerator(0, false)]
    public partial class ArcticOgreLord : BaseCreature
    {
        [Constructible]
        public ArcticOgreLord() : base(AIType.AI_Melee)
        {
            Body = 135;
            BaseSoundID = 427;

            LevelRange = [24, 29];
            StrPerLevel = [6, 10];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 7];
            ResistancePerLevel = [3, 4];

            SetStr(120, 330);
            // SetStr(767, 945);
            SetDex(20, 45);
            // SetDex(66, 75);
            SetInt(35, 50);
            // SetInt(46, 70);

            SetHits(236, 282);
            // SetHits(476, 552);

            SetDamage(8, 14);
            // SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold, 70);

            SetResistance(ResistanceType.Physical, 15, 35);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 30);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.Wrestling, 50.0, 60.5);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 50;

            PackItem(new Club());
        }
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };
        public override string CorpseName => "a frozen ogre lord's corpse";
        public override string DefaultName => "an arctic ogre lord";

        public override Poison PoisonImmune => Poison.Regular;
        public override int TreasureMapLevel => 3;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
        }
    }
}
