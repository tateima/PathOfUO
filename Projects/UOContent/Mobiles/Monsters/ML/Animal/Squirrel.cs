using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Squirrel : BaseCreature
    {
        [Constructible]
        public Squirrel() : base(AIType.AI_Animal, FightMode.Aggressor)
        {
            Body = 0x116;

            LevelRange = [2, 4];
            StrPerLevel = [1, 2];
            IntPerLevel = [3, 4];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 2];

            SetStr(6, 10);
            SetDex(16, 28);
            SetInt(6, 14);

            SetHits(4, 6);
            SetMana(0);

            SetDamage(1, 2);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 5, 14);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 5, 20);

            SetSkill(SkillName.MagicResist, 4.0, 30.0);
            SetSkill(SkillName.Tactics, 4.0, 30.0);
            SetSkill(SkillName.Wrestling, 4.0, 30.0);

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -21.3;
        }

        public override string CorpseName => "a squirrel corpse";
        public override string DefaultName => "a squirrell";

        public override int Meat => 1;
        public override FoodType FavoriteFood => FoodType.FruitsAndVeggies;
    }
}
