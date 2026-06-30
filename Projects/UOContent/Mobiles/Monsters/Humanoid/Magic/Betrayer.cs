using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Betrayer : BaseCreature
    {
        [Constructible]
        public Betrayer() : base(AIType.AI_Mage)
        {
            Body = 767;

            LevelRange = [45, 65];
            StrPerLevel = [3, 6];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(76, 105);
            SetDex(50, 90);
            SetInt(52, 110);

            SetHits(200, 230);

            SetDamage(1, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.Anatomy, 50.0, 60.5);
            SetSkill(SkillName.EvalInt, 50.0, 60.5);
            SetSkill(SkillName.Magery, 50.0, 60.5);
            SetSkill(SkillName.Meditation, 50.0, 60.5);
            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.Wrestling, 50.0, 60.5);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 65;
            SpeechHue = Utility.RandomDyedHue();

            PackItem(new PowerCrystal());

            if (Utility.RandomDouble() < 0.02)
            {
                PackItem(new BlackthornWelcomeBook());
            }
        }

        public override string CorpseName => "a betrayer corpse";

        public override string DefaultName => "a betrayer";

        public override bool AlwaysMurderer => true;
        public override bool BardImmune => !Core.AOS;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int Meat => 1;
        public override int TreasureMapLevel => 5;

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.05)
            {
                if (!IsParagon)
                {
                    if (Utility.RandomDouble() < 0.75)
                    {
                        c.DropItem(DawnsMusicGear.RandomCommon);
                    }
                    else
                    {
                        c.DropItem(DawnsMusicGear.RandomUncommon);
                    }
                }
                else
                {
                    c.DropItem(DawnsMusicGear.RandomRare);
                }
            }
        }

        public override int GetDeathSound() => 0x423;

        public override int GetAttackSound() => 0x23B;

        public override int GetHurtSound() => 0x140;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 1);
        }

        private static MonsterAbility[] _abilities =
        {
            new MonsterAbilityGroup(
                (1, MonsterAbilities.ColossalBlow),
                (1, MonsterAbilities.PoisonGasAreaAttack)
            )
        };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;
    }
}
