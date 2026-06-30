using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Thrasher : Alligator
    {
        [Constructible]
        public Thrasher()
        {
            IsParagon = true;

            Hue = 0x497;

            LevelRange = [50, 60];
            StrPerLevel = [3, 6];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [1, 2];

            SetStr(136, 155);
            SetDex(30, 50);
            SetInt(42, 75);

            SetHits(140, 200);

            SetDamage(2, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);

            SetSkill(SkillName.Wrestling, 40.4, 50.1);
            SetSkill(SkillName.Tactics, 40.4, 50.1);
            SetSkill(SkillName.MagicResist, 40.4, 50.1);

            // TODO: Fame/Karma
        }

        public override string CorpseName => "a Thrasher corpse";
        public override string DefaultName => "Thrasher";

        public override bool GivesMLMinorArtifact => true;
        public override int Hides => 48;
        public override int Meat => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 4);
        }

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.ArmorIgnore;

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new ThrashersTail());
        }
    }
}
