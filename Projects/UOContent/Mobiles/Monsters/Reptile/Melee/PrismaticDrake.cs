using System;
using Server.Spells;

namespace Server.Mobiles
{
    public class PrismaticDrake : Drake
    {
        [Constructible]
        public PrismaticDrake()
        {
            SetHits(300, 358);
            Tamable = true;
            ControlSlots = 3;
            SetSkill(SkillName.MagicResist,80.0, 100.0);
            Hue = Utility.RandomBrightHue();
        }

        public PrismaticDrake(Serial serial) : base(serial)
        {
        }
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a prismatic drake corpse";
        public override string DefaultName => "a prismatic drake";

        public override void OnDamagedBySpell(Mobile from, int damage)
        {
            MovingParticles(from, 0x379F, 7, 0, false, true, 3043, 4043, 0x211);
            from.PlaySound(0x20A);
            SpellHelper.Damage(TimeSpan.FromSeconds(3), from, this, damage);
            base.OnDamagedBySpell(from, damage);
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
