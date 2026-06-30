using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Ogre : BaseCreature
    {
        [Constructible]
        public Ogre() : base(AIType.AI_Melee)
        {
            Body = 1;
            BaseSoundID = 427;

            LevelRange = [13, 18];
            StrPerLevel = [1, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 5];
            ResistancePerLevel = [1, 3];

            SetStr(60, 155);
            SetDex(30, 55);
            SetInt(25, 30);
            SetHits(85, 100);
            SetDamage(4, 8);

            // SetStr(166, 195);
            // SetDex(46, 65);
            // SetInt(46, 70);
            //
            // SetHits(100, 117);
            // SetMana(0);

            // SetDamage(9, 11);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 15);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 50.1, 60.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 32;

            PackItem(new Club());
        }

        public override string CorpseName => "an ogre corpse";
        public override string DefaultName => "an ogre";

        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 1;
        public override int Meat => 2;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Potions);
        }
    }
}
