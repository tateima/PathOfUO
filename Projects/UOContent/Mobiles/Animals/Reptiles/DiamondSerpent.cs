using Server.Ethics;
using Server.Factions;
using Server.Items;

namespace Server.Mobiles
{
    [TypeAlias("Server.Mobiles.Diamondserpant")]
    public class DiamondSerpent : SilverSerpent
    {
        [Constructible]
        public DiamondSerpent()
        {
            Body = 92;
            BaseSoundID = 219;

            SetStr(200, 360);
            SetDex(200, 300);

            SetHits(136, 246);

            SetDamage(8, 24);

            SetDamageType(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Physical, 75, 95);
            SetSkill(SkillName.Poisoning, 70.1, 80.0);
            Hue = Utility.RandomBlueHue();

            Tamable = true;
            MinTameSkill = 84.3;
        }

        public DiamondSerpent(Serial serial) : base(serial)
        {
        }
        public override bool CanCannibalise(Mobile target) => base.CanCannibalise(target) || target is SilverSerpent;
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a diamond serpent corpse";
        public override string DefaultName => "a diamond serpent";
        public override Poison HitPoison => Poison.Greater;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            Backpack?.DropItem(new Diamond(Utility.RandomMinMax(1, 3)));
            Backpack?.DropItem(new BlueDiamond(Utility.RandomMinMax(1, 3)));
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

            if (BaseSoundID == -1)
            {
                BaseSoundID = 219;
            }
        }
    }
}
