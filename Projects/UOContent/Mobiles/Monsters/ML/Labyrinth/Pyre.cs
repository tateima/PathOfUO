using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Pyre : Phoenix
    {
        [Constructible]
        public Pyre()
        {
            IsParagon = true;

            Hue = 0x489;

            FightMode = FightMode.Closest;

            LevelRange = [60, 70];
            StrPerLevel = [3, 6];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 5];
            ResistancePerLevel = [2, 3];

            SetStr(70, 145);
            SetDex(100, 135);
            SetInt(55, 70);
            SetHits(95, 160);

            SetDamage(5, 8);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 35);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 10, 25);

            SetSkill(SkillName.Wrestling, 50.9, 65.1);
            SetSkill(SkillName.Tactics, 50.9, 65.1);
            SetSkill(SkillName.MagicResist, 50.9, 65.1);
            SetSkill(SkillName.Poisoning, 50.9, 65.1);
            SetSkill(SkillName.Magery, 50.9, 65.1);
            SetSkill(SkillName.EvalInt, 50.9, 65.1);
            SetSkill(SkillName.Meditation, 50.9, 65.1);

            Fame = 21000;
            Karma = -21000;
        }

        public override string CorpseName => "a Pyre corpse";
        public override string DefaultName => "Pyre";

        public override bool GivesMLMinorArtifact => true;
        public override int TreasureMapLevel => 5;
        public override bool HasAura => true;

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
