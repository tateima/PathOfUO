using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Juggernaut : BaseCreature
    {
        [Constructible]
        public Juggernaut() : base(AIType.AI_Melee)
        {
            Body = 768;

            LevelRange = [15, 20];
            StrPerLevel = [2, 5];
            IntPerLevel = [2, 3];
            DexPerLevel = [3, 10];
            ResistancePerLevel = [1, 3];

            SetStr(120, 185);
            SetDex(50, 75);
            SetInt(35, 50);
            SetHits(105, 125);
            SetDamage(5, 10);
            //
            // SetStr(301, 400);
            // SetDex(51, 70);
            // SetInt(51, 100);
            //
            // SetHits(181, 240);
            //
            // SetDamage(12, 19);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire, 5, 25);
            SetResistance(ResistanceType.Cold, 5, 25);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.Anatomy, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 70;

            if (Utility.RandomDouble() < 0.1)
            {
                PackItem(new PowerCrystal());
            }

            if (Utility.RandomDouble() < 0.4)
            {
                PackItem(new ClockworkAssembly());
            }
        }

        public override string CorpseName => "a juggernaut corpse";

        public override string DefaultName => "a blackthorn juggernaut";

        public override bool AlwaysMurderer => true;
        public override bool BardImmune => !Core.AOS;
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int Meat => 1;
        public override int TreasureMapLevel => 5;

        private static MonsterAbility[] _abilities = { MonsterAbilities.ColossalBlow };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

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

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 1);
        }

        public override int GetDeathSound() => 0x423;

        public override int GetAttackSound() => 0x23B;

        public override int GetHurtSound() => 0x140;
    }
}
