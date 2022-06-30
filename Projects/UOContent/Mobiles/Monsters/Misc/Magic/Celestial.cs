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

            SetStr(290, 390);
            SetDex(136, 170);
            SetInt(600, 1200);

            SetHits(256, 359);

            SetDamage(9, 13);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 30, 70);
            SetResistance(ResistanceType.Fire, 30, 70);
            SetResistance(ResistanceType.Cold, 30, 70);
            SetResistance(ResistanceType.Poison, 100, 100);
            SetResistance(ResistanceType.Energy, 30, 70);

            SetSkill(SkillName.EvalInt, 80.1, 95.0);
            SetSkill(SkillName.Magery, 80.1, 95.0);
            SetSkill(SkillName.Meditation, 80.2, 120.0);
            SetSkill(SkillName.MagicResist, 85.2, 115.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.Wrestling, 70.1, 90.0);

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
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };
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
