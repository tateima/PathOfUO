using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class CuSidhe : BaseMount
    {
        [Constructible]
        public CuSidhe() : base(277, 0x3E91, AIType.AI_Animal, FightMode.Aggressor)
        {
            var chance = Utility.RandomDouble() * 23301;

            if (chance <= 1)
            {
                Hue = 0x489;
            }
            else if (chance < 50)
            {
                Hue = Utility.RandomList(0x657, 0x515, 0x4B1, 0x481, 0x482, 0x455);
            }
            else if (chance < 500)
            {
                Hue = Utility.RandomList(0x97A, 0x978, 0x901, 0x8AC, 0x5A7, 0x527);
            }
            LevelRange = [10, 30];
            StrPerLevel = [2, 3];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 2];
            SetStr(196, 225);
            SetDex(30, 50);
            SetInt(50, 80);

            SetHits(250, 320);

            SetDamage(3, 7);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Cold, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 25);

            SetSkill(SkillName.Wrestling, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Anatomy, 40.1, 50.0);
            SetSkill(SkillName.Healing, 40.1, 50.0);

            Fame = 5000;  // Guessing here
            Karma = 5000; // Guessing here

            Tamable = true;
            ControlSlots = 4;
            MinTameSkill = 101.1;

            if (Utility.RandomDouble() < 0.2)
            {
                PackItem(new TreasureMap(5, Map.Trammel));
            }

            // if (Utility.RandomDouble() < 0.1)
            // PackItem( new ParrotItem() );

            PackGold(500, 800);

            // TODO 0-2 spellweaving scroll
        }

        public override string CorpseName => "a cu sidhe corpse";
        public override string DefaultName => "a cu sidhe";

        public override bool CanHeal => true;
        public override bool CanHealOwner => true;
        public override FoodType FavoriteFood => FoodType.FruitsAndVeggies;
        public override bool CanAngerOnTame => true;
        public override bool StatLossAfterTame => true;
        public override int Hides => 10;
        public override int Meat => 3;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.AosFilthyRich, 5);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Race != Race.Elf && from == ControlMaster && from.AccessLevel == AccessLevel.Player)
            {
                var pads = from.FindItemOnLayer(Layer.Shoes);

                if (pads is PadsOfTheCuSidhe)
                {
                    from.SendLocalizedMessage(1071981); // Your boots allow you to mount the Cu Sidhe.
                }
                else
                {
                    from.SendLocalizedMessage(1072203); // Only Elves may use this.
                    return;
                }
            }

            base.OnDoubleClick(from);
        }

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.BleedAttack;

        public override int GetIdleSound() => 0x577;

        public override int GetAttackSound() => 0x576;

        public override int GetAngerSound() => 0x578;

        public override int GetHurtSound() => 0x576;

        public override int GetDeathSound() => 0x579;
    }
}
