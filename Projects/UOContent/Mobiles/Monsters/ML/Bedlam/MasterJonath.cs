using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class MasterJonath : BoneMagi
    {
        [Constructible]
        public MasterJonath()
        {
            IsParagon = true;

            Hue = 0x455;

            LevelRange = [50, 60];
            StrPerLevel = [1, 3];
            IntPerLevel = [2, 7];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [2, 3];
            SetStr(40, 80);
            SetDex(10, 45);
            SetInt(90, 175);
            SetHits(75, 90);

            SetDamage(3, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.Wrestling, 50.9, 65.1);
            SetSkill(SkillName.Tactics, 50.9, 65.1);
            SetSkill(SkillName.MagicResist, 50.9, 65.1);
            SetSkill(SkillName.Magery, 50.9, 65.1);
            SetSkill(SkillName.EvalInt, 50.9, 65.1);
            SetSkill(SkillName.Necromancy, 50.9, 65.1);
            SetSkill(SkillName.SpiritSpeak, 50.9, 65.1);

            Fame = 18000;
            Karma = -18000;

            if (Utility.RandomBool())
            {
                PackNecroScroll(Utility.RandomMinMax(5, 9));
            }
            else
            {
                PackScroll(4, 7);
            }

            PackReg(7);
            PackReg(7);
            PackReg(8);
        }

        public override string CorpseName => "a Master Jonath corpse";
        public override string DefaultName => "Master Jonath";

        // TODO: Special move?

        /*
        // TODO: uncomment once added
        public override void OnDeath( Container c )
        {
          base.OnDeath( c );

          if (Utility.RandomDouble() < 0.05)
            c.DropItem( new ParrotItem() );

          if (Utility.RandomDouble() < 0.15)
            c.DropItem( new DisintegratingThesisNotes() );
        }
        */

        public override bool GivesMLMinorArtifact => true;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }
    }
}
