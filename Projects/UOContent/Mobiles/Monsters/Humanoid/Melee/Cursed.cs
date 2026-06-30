using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Cursed : BaseCreature
    {
        [Constructible]
        public Cursed() : base(AIType.AI_Melee)
        {
            Title = "the Cursed";

            Hue = Utility.RandomMinMax(0x8596, 0x8599);
            Body = 0x190;
            Name = NameList.RandomName("male");
            BaseSoundID = 471;

            AddItem(new ShortPants(Utility.RandomNeutralHue()));
            AddItem(new Shirt(Utility.RandomNeutralHue()));

            var weapon = Loot.RandomWeapon();
            weapon.Movable = false;
            AddItem(weapon);

            LevelRange = [10, 30];
            StrPerLevel = [1, 3];
            IntPerLevel = [2, 3];
            DexPerLevel = [5, 7];
            ResistancePerLevel = [1, 2];

            SetStr(55, 90);
            SetDex(45, 75);
            SetInt(55, 90);

            SetDamage(2, 8);

            SetResistance(ResistanceType.Physical, 5, 15);
            SetResistance(ResistanceType.Fire, 0, 5);
            SetResistance(ResistanceType.Cold, 1, 5);
            SetResistance(ResistanceType.Poison, 0, 5);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.Fencing, 40.0, 50.5);
            SetSkill(SkillName.Macing, 40.0, 50.5);
            SetSkill(SkillName.MagicResist, 40.0, 50.5);
            SetSkill(SkillName.Swords, 40.0, 50.5);
            SetSkill(SkillName.Tactics, 40.0, 50.5);
            SetSkill(SkillName.Poisoning, 40.0, 50.5);

            Fame = 1000;
            Karma = -2000;
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };
        public override string CorpseName => "an inhuman corpse";
        public override bool ClickTitle => false;
        public override bool ShowFameTitle => false;

        public override bool AlwaysMurderer => true;

        public override int GetAttackSound() => -1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
            // AddLoot( LootPack.Miscellaneous );
        }
    }
}
