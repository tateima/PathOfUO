using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Lurg : Troglodyte
    {
        [Constructible]
        public Lurg()
        {
            IsParagon = true;

            Hue = 0x455;
            LevelRange = [55, 65];
            StrPerLevel = [3, 6];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 7];
            ResistancePerLevel = [1, 3];

            SetStr(110, 195);
            SetDex(70, 105);
            SetInt(15, 20);
            SetHits(120, 200);
            SetDamage(4, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 15, 20);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 10, 25);

            SetSkill(SkillName.Wrestling, 55.7, 60.5);
            SetSkill(SkillName.Tactics, 55.3, 60.5);
            SetSkill(SkillName.MagicResist, 55.9, 60.6);
            SetSkill(SkillName.Anatomy, 55.5, 60.0);
            SetSkill(SkillName.Healing, 55.1, 60.0);

            Fame = 10000;
            Karma = -10000;
        }

        public override string CorpseName => "a Lurg corpse";
        public override string DefaultName => "Lurg";
        public override bool GivesMLMinorArtifact => true;
        public override int TreasureMapLevel => 4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.CrushingBlow;
    }
}
