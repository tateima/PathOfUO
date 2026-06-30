using Server.Items;
using Server.Pantheon;

namespace Server.Mobiles
{
    public class Celestial : BaseCreature
    {
        [Constructible]
        public Celestial() : base(AIType.AI_Healer)
        {
            Body = 30;
            Hue = Deity.LightHue;

            LevelRange = [30, 60];
            StrPerLevel = [3, 5];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(76, 95);
            SetDex(50, 100);
            SetInt(52, 90);

            SetHits(91, 110);

            SetDamage(3, 7);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 5, 30);
            SetResistance(ResistanceType.Fire, 10, 10);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 5, 30);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.Meditation, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 12500;
            Karma = -12500;

            OverrideDispellable = true;

            VirtualArmor = 70;
            ControlSlots = 4;
            AddItem(new LightSource());
        }

        public Celestial(Serial serial) : base(serial)
        {
        }
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight, OppositionGroup.CelestialsAndDaemons };
        public override string CorpseName => "a celestial corpse";
        public override double DispelDifficulty => 150.5;
        public override double DispelFocus => 45.0;

        public override string DefaultName => "a celestial";

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
        }
    }
}
