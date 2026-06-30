using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class KhaldunSummoner : BaseCreature
    {
        [Constructible]
        public KhaldunSummoner() : base(AIType.AI_Mage)
        {
            Body = 0x190;
            Title = "the Summoner";
            LevelRange = [9, 14];
            StrPerLevel = [1, 4];
            IntPerLevel = [4, 8];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 4];

            SetStr(40, 90);
            SetDex(30, 35);
            SetInt(75, 95);
            SetHits(55, 75);
            SetDamage(3, 6);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Cold, 25);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 5, 20);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.Wrestling, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Meditation, 40.1, 50.0);

            VirtualArmor = 36;
            Fame = 10000;
            Karma = -10000;

            var gloves = new LeatherGloves();
            gloves.Hue = 0x66D;
            AddItem(gloves);

            var helm = new BoneHelm();
            helm.Hue = 0x835;
            AddItem(helm);

            var necklace = new Necklace();
            necklace.Hue = 0x66D;
            AddItem(necklace);

            var cloak = new Cloak();
            cloak.Hue = 0x66D;
            AddItem(cloak);

            var kilt = new Kilt();
            kilt.Hue = 0x66D;
            AddItem(kilt);

            var sandals = new Sandals();
            sandals.Hue = 0x66D;
            AddItem(sandals);
        }

        public override bool ClickTitle => false;
        public override bool ShowFameTitle => false;

        public override string DefaultName => "Zealot of Khaldun";

        public override bool AlwaysMurderer => true;
        public override bool Unprovokable => true;

        public override int GetIdleSound() => 0x184;

        public override int GetAngerSound() => 0x286;

        public override int GetDeathSound() => 0x288;

        public override int GetHurtSound() => 0x19F;

        public override bool OnBeforeDeath()
        {
            var rm = new BoneMagi();
            rm.Team = Team;
            rm.Combatant = Combatant;
            rm.NoKillAwards = true;

            if (rm.Backpack == null)
            {
                var pack = new Backpack();
                pack.Movable = false;
                rm.AddItem(pack);
            }

            for (var i = 0; i < 2; i++)
            {
                LootPack.FilthyRich.Generate(this, rm.Backpack, true, LootPack.GetLuckChanceForKiller(this));
                LootPack.FilthyRich.Generate(this, rm.Backpack, false, LootPack.GetLuckChanceForKiller(this));
            }

            Effects.PlaySound(this, GetDeathSound());
            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10, 0x835);
            rm.MoveToWorld(Location, Map);

            Delete();
            return false;
        }
    }
}
