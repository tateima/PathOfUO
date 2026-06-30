using ModernUO.Serialization;
using System;
using Server.Engines.CannedEvil;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Twaulo : BaseChampion
    {
        [Constructible]
        public Twaulo()
            : base(AIType.AI_Melee)
        {
            Title = "of the Glade";
            Body = 101;
            BaseSoundID = 679;
            Hue = 0x455;

            LevelRange = [75, 90];
            StrPerLevel = [1, 3];
            IntPerLevel = [1, 4];
            DexPerLevel = [1, 7];
            ResistancePerLevel = [1, 2];

            SetStr(60, 70);
            SetDex(72, 90);
            SetInt(50, 70);

            SetHits(180, 280);

            SetDamage(3, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Fire, 5, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.EvalInt, 0);    // Per Stratics?!?
            SetSkill(SkillName.Magery, 0);     // Per Stratics?!?
            SetSkill(SkillName.Meditation, 0); // Per Stratics?!?
            SetSkill(SkillName.Anatomy, 53.0, 63.5);
            SetSkill(SkillName.Archery, 53.0, 63.5);
            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Tactics, 53.0, 63.5);
            SetSkill(SkillName.Wrestling, 53.0, 63.5);

            Fame = 50000;
            Karma = 50000;

            VirtualArmor = 50;

            AddItem(new Bow());
            PackItem(new Arrow(Utility.RandomMinMax(500, 700)));
        }

        public override string CorpseName => "a corpse of Twaulo";
        public override ChampionSkullType SkullType => ChampionSkullType.Pain;

        public override Type[] UniqueList => new[] { typeof(Quell) };
        public override Type[] SharedList => new[] { typeof(TheMostKnowledgePerson), typeof(OblivionsNeedle) };
        public override Type[] DecorativeList => new[] { typeof(Pier), typeof(MonsterStatuette) };

        public override MonsterStatuetteType[] StatueTypes => new[] { MonsterStatuetteType.DreadHorn };

        public override string DefaultName => "Twaulo";

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override bool Unprovokable => true;
        public override Poison PoisonImmune => Poison.Regular;
        public override int TreasureMapLevel => 5;
        public override int Meat => 1;
        public override int Hides => 8;
        public override HideType HideType => HideType.Spined;

        private static MonsterAbility[] _abilities = { MonsterAbilities.SummonPixiesCounter };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems);
        }

        public override void OnGaveMeleeAttack(Mobile defender, int damage)
        {
            base.OnGaveMeleeAttack(defender, damage);

            defender.Damage(Utility.Random(20, 10), this);
            defender.Stam -= Utility.Random(20, 10);
            defender.Mana -= Utility.Random(20, 10);
        }
    }
}
