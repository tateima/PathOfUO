using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Troglodyte : BaseCreature
    {
        [Constructible]
        public Troglodyte() : base(AIType.AI_Melee) // NEED TO CHECK
        {
            Body = 267;
            BaseSoundID = 0x59F;

            LevelRange = [15, 35];
            StrPerLevel = [3, 6];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 7];
            ResistancePerLevel = [1, 3];

            SetStr(70, 145);
            SetDex(40, 65);
            SetInt(15, 20);
            SetHits(95, 120);
            SetDamage(4, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 15);

            SetSkill(SkillName.Anatomy, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);
            SetSkill(SkillName.Healing, 40.1, 50.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 28; // Don't know what it should be

            PackItem(new Bandage(5)); // How many?
            PackItem(new Ribs());
        }

        public override string CorpseName => "a troglodyte corpse";
        public override string DefaultName => "a troglodyte";
        public override bool CanHeal => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich); // Need to verify
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.1)
            {
                c.DropItem(new PrimitiveFetish());
            }
        }
    }
}
