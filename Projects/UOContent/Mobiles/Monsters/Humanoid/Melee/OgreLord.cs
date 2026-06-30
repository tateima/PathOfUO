using ModernUO.Serialization;
using Server.Ethics;
using Server.Factions;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class OgreLord : BaseCreature
    {
        [Constructible]
        public OgreLord() : base(AIType.AI_Melee)
        {
            Body = 83;
            BaseSoundID = 427;

            LevelRange = [21, 26];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 4];
            ResistancePerLevel = [3, 4];

            SetStr(100, 300);
            // SetStr(767, 945);
            SetDex(40, 65);
            // SetDex(66, 75);
            SetInt(35, 50);
            // SetInt(46, 70);

            SetHits(216, 252);
            // SetHits(476, 552);

            SetDamage(6, 12);
            // SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 10);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 50;

            PackItem(new Club());
        }

        public override string CorpseName => "an ogre lords corpse";
        public override Faction FactionAllegiance => Minax.Instance;
        public override Ethic EthicAllegiance => Ethic.Evil;

        public override string DefaultName => "an ogre lord";

        public override bool CanRummageCorpses => true;
        public override Poison PoisonImmune => Poison.Regular;
        public override int TreasureMapLevel => 3;
        public override int Meat => 2;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
        }
    }
}
