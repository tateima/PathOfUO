using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class MasterTheophilus : EvilMageLord
    {
        [Constructible]
        public MasterTheophilus()
        {
            IsParagon = true;

            Title = "the necromancer";
            Hue = 0;

            LevelRange = [55, 75];
            StrPerLevel = [1, 3];
            IntPerLevel = [4, 9];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [2, 3];

            SetStr(65, 90);
            SetDex(35, 55);
            SetInt(110, 195);
            SetHits(100, 130);

            SetDamage(4, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.Wrestling, 55.9, 65.1);
            SetSkill(SkillName.Tactics, 55.9, 65.1);
            SetSkill(SkillName.MagicResist, 55.9, 65.1);
            SetSkill(SkillName.Magery, 55.9, 65.1);
            SetSkill(SkillName.EvalInt, 55.9, 65.1);
            SetSkill(SkillName.Necromancy, 55.9, 65.1);
            SetSkill(SkillName.SpiritSpeak, 55.9, 65.1);
            SetSkill(SkillName.Meditation, 55.9, 65.1);

            Fame = 18000;
            Karma = -18000;

            AddItem(new Shoes(0x537));
            AddItem(new Robe(0x452));

            for (var i = 0; i < 2; ++i)
            {
                if (Utility.RandomBool())
                {
                    PackNecroScroll(Utility.RandomMinMax(5, 9));
                }
                else
                {
                    PackScroll(4, 7);
                }
            }

            PackReg(7);
            PackReg(7);
            PackReg(8);
        }

        public override string CorpseName => "a Master Theophilus corpse";
        public override string DefaultName => "Master Theophilus";

        public override bool GivesMLMinorArtifact => true;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.ParalyzingBlow;
    }
}
