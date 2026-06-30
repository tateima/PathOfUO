using Server.Items;
using Server.Pantheon;

namespace Server.Mobiles
{
    public class ChaosElemental : BaseCreature
    {
        [Constructible]
        public ChaosElemental() : base(AIType.AI_Mage)
        {
            Body = 159;
            BaseSoundID = 768;
            Hue = Deity.ChaosHue;

            LevelRange = [25, 42];
            StrPerLevel = [2, 6];
            IntPerLevel = [1, 6];
            DexPerLevel = [2, 6];
            ResistancePerLevel = [1, 3];

            SetStr(60, 125);
            SetDex(30, 65);
            SetInt(25, 70);
            SetHits(85, 140);
            SetDamage(3, 8);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical,  Utility.RandomMinMax(1, 10),  Utility.RandomMinMax(10, 30));
            SetResistance(ResistanceType.Fire,  Utility.RandomMinMax(1, 10),  Utility.RandomMinMax(10, 20));
            SetResistance(ResistanceType.Cold,  Utility.RandomMinMax(1, 10),  Utility.RandomMinMax(10, 20));
            SetResistance(ResistanceType.Poison,  Utility.RandomMinMax(1, 20),  Utility.RandomMinMax(20, 30));
            SetResistance(ResistanceType.Energy,  Utility.RandomMinMax(1, 10),  Utility.RandomMinMax(10, 20));

            SetSkill(SkillName.EvalInt, 45.1, 55.0);
            SetSkill(SkillName.Magery, 45.1, 55.0);
            SetSkill(SkillName.Meditation, 45.1, 55.0);
            SetSkill(SkillName.MagicResist, 45.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);

            Fame = 12500;
            Karma = -12500;

            OverrideDispellable = true;

            VirtualArmor = 70;
            ControlSlots = 4;
            AddItem(new LightSource());
        }

        public ChaosElemental(Serial serial) : base(serial)
        {
        }
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a chaos elemental corpse";
        public override double DispelDifficulty => 150.5;
        public override double DispelFocus => 45.0;

        public override string DefaultName => "a chaos elemental";

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 2;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();

            if (BaseSoundID == 274)
            {
                BaseSoundID = 838;
            }
        }
    }
}
