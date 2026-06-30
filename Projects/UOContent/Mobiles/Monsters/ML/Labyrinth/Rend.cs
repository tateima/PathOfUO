using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Rend : Reptalon
    {
        [Constructible]
        public Rend()
        {
            IsParagon = true;

            Hue = 0x455;

            LevelRange = [75, 80];
            StrPerLevel = [4, 10];
            IntPerLevel = [3, 5];
            DexPerLevel = [3, 5];
            ResistancePerLevel = [2, 4];

            SetStr(166, 205);
            SetDex(50, 80);
            SetInt(82, 140);

            SetHits(260, 320);

            SetDamageType(ResistanceType.Physical, 100);
            SetDamageType(ResistanceType.Poison, 0);
            SetDamageType(ResistanceType.Energy, 0);

            SetResistance(ResistanceType.Physical, 5, 35);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 15, 20);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Wrestling, 55.0, 65.5);
            SetSkill(SkillName.Tactics, 55.0, 65.5);
            SetSkill(SkillName.MagicResist, 55.0, 65.5);
            SetSkill(SkillName.Anatomy, 55.0, 65.5);

            Fame = 21000;
            Karma = -21000;
        }

        public override string CorpseName => "a Rend corpse";
        public override string DefaultName => "Rend";

        public override bool GivesMLMinorArtifact => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }

        public override WeaponAbility GetWeaponAbility()
        {
            if (Utility.RandomBool())
            {
                return WeaponAbility.ParalyzingBlow;
            }

            return WeaponAbility.BleedAttack;
        }
    }
}
