using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class ShadowWisp : BaseCreature
    {
        [Constructible]
        public ShadowWisp() : base(AIType.AI_Mage, FightMode.Aggressor)
        {
            Body = 165;
            BaseSoundID = 466;

            LevelRange = [10, 50];
            StrPerLevel = [1, 2];
            IntPerLevel = [2, 9];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(36, 65);
            SetDex(70, 130);
            SetInt(82, 140);

            SetHits(50, 76);

            SetDamage(3, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 15, 30);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 500;

            VirtualArmor = 18;

            AddItem(new LightSource());

            PackItem(
                Utility.Random(10) switch
                {
                    0 => new LeftArm(),
                    1 => new RightArm(),
                    2 => new Torso(),
                    3 => new Bone(),
                    4 => new RibCage(),
                    5 => new RibCage(),
                    _ => new BonePile() // 6-9
                }
            );
        }

        public override string CorpseName => "a wisp corpse";
        public override string DefaultName => "a shadow wisp";

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight, OppositionGroup.CelestialsAndDaemons };

    }
}
