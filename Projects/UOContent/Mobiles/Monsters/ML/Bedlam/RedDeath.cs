using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class RedDeath : SkeletalMount
    {
        [Constructible]
        public RedDeath() : base()
        {
            IsParagon = true;

            Hue = 0x21;
            BaseSoundID = 0x1C3;

            AI = AIType.AI_Melee;
            FightMode = FightMode.Closest;

            LevelRange = [50, 60];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 2];

            SetStr(196, 225);
            SetDex(30, 50);
            SetInt(50, 80);

            SetHits(250, 320);

            SetDamage(3, 7);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 75);
            SetDamageType(ResistanceType.Cold, 0);

            SetResistance(ResistanceType.Physical, 5, 30);
            SetResistance(ResistanceType.Fire, 20, 25);
            SetResistance(ResistanceType.Cold, 0);
            SetResistance(ResistanceType.Poison, 10, 25);
            SetResistance(ResistanceType.Energy, 20, 25);

            SetSkill(SkillName.Wrestling, 50.9, 65.1);
            SetSkill(SkillName.Tactics, 50.9, 65.1);
            SetSkill(SkillName.MagicResist, 50.9, 65.1);
            SetSkill(SkillName.Anatomy, 50.9, 65.1);

            Fame = 28000;
            Karma = -28000;

            if (Utility.RandomBool())
            {
                PackNecroScroll(Utility.RandomMinMax(5, 9));
            }
            else
            {
                PackScroll(4, 7);
            }
        }

        public override string DefaultName => "Red Death";

        public override string CorpseName => "a Red Death corpse";

        public override bool GivesMLMinorArtifact => true;
        public override bool AlwaysMurderer => true;

        private static MonsterAbility[] _abilities = { MonsterAbilities.ChaosBreath };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.WhirlwindAttack;

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new ResolvesBridle());
        }
    }
}
