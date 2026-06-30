using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Malefic : DreadSpider
    {
        [Constructible]
        public Malefic()
        {
            IsParagon = true;
            Hue = 0x455;

            LevelRange = [45, 65];
            StrPerLevel = [2, 7];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 9];
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
            SetSkill(SkillName.Meditation, 0);
            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 50.1, 55.0);
            SetSkill(SkillName.Wrestling, 45.1, 50);

            Fame = 21000;
            Karma = -21000;

            /*
            // TODO: uncomment once added
            if (Utility.RandomDouble() < 0.1)
              PackItem( new ParrotItem() );
            */
        }

        public override string CorpseName => "a Malefic corpse";
        public override string DefaultName => "Malefic";

        public override bool GivesMLMinorArtifact => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.Dismount;
    }
}
