using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Grobu : BlackBear
    {
        [Constructible]
        public Grobu()
        {
            IsParagon = true;

            Hue = 0x455;

            AI = AIType.AI_Melee;
            FightMode = FightMode.Closest;

            LevelRange = [55, 65];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 3];
            DexPerLevel = [1, 4];
            ResistancePerLevel = [1, 2];

            SetStr(76, 106);
            SetDex(30, 40);
            SetInt(22, 45);

            SetHits(86, 150);

            SetDamage(2, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 25);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Wrestling, 45.4, 55.0);
            SetSkill(SkillName.Tactics, 45.2, 55.5);
            SetSkill(SkillName.MagicResist, 45.2, 55.7);

            Fame = 1000;
            Karma = 1000;
        }

        public override string CorpseName => "a Grobu corpse";
        public override string DefaultName => "Grobu";

        public override bool GivesMLMinorArtifact => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new GrobusFur());
        }
    }
}
