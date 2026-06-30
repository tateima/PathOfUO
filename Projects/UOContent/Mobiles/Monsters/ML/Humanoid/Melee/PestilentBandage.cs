using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class PestilentBandage : BaseCreature
    {
        [Constructible]
        public PestilentBandage() : base(AIType.AI_Melee) // NEED TO CHECK
        {
            Body = 154;
            Hue = 0x515;
            BaseSoundID = 471;

            LevelRange = [55, 65];
            StrPerLevel = [2, 4];
            IntPerLevel = [4, 6];
            DexPerLevel = [5, 10];
            ResistancePerLevel = [2, 3];

            SetStr(50, 135);
            SetDex(60, 85);
            SetInt(35, 50);
            SetHits(65, 85);
            SetDamage(3, 9);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 40);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 10, 25);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Poisoning, 40, 55.0);
            SetSkill(SkillName.Anatomy, 40, 55.0);
            SetSkill(SkillName.MagicResist, 40, 55.0);
            SetSkill(SkillName.Tactics, 40, 55.0);
            SetSkill(SkillName.Wrestling, 40, 55.0);

            Fame = 20000;
            Karma = -20000;

            // VirtualArmor = 28; // Don't know what it should be

            PackItem(new Bandage(5)); // How many?
        }

        public override string CorpseName => "a pestilent bandage corpse";
        // Neither Stratics nor UOGuide have much description
        // beyond being a "Grey Mummy". Body, Sound and
        // Hue are all guessed until they can be verified.
        // Loot and Fame/Karma are also guesses at this point.
        //
        // They also apparently have a Poison Attack, which I've stolen from Yamandons.

        public override string DefaultName => "a pestilent bandage";

        public override Poison HitPoison => Poison.Lethal;
        public override bool CanHeal => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich); // Need to verify
        }
    }
}
