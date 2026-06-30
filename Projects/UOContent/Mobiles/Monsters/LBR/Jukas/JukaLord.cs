using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class JukaLord : BaseCreature
    {
        [Constructible]
        public JukaLord() : base(AIType.AI_Archer, FightMode.Closest, 10, 3)
        {
            Body = 766;

            LevelRange = [63, 78];
            StrPerLevel = [2, 5];
            IntPerLevel = [2, 5];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [2, 3];

            SetStr(86, 105);
            SetDex(90, 120);
            SetInt(72, 90);

            SetHits(190, 230);
            SetDamage(5, 11);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 30);
            SetResistance(ResistanceType.Fire, 15, 20);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 10, 30);

            SetSkill(SkillName.Anatomy, 53.0, 63.5);
            SetSkill(SkillName.Archery, 53.0, 63.5);
            SetSkill(SkillName.Healing, 53.0, 63.5);
            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Swords, 53.0, 63.5);
            SetSkill(SkillName.Tactics, 53.0, 63.5);
            SetSkill(SkillName.Wrestling, 53.0, 63.5);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 28;

            var pack = new Backpack();

            pack.DropItem(new Arrow(Utility.RandomMinMax(25, 35)));
            pack.DropItem(new Arrow(Utility.RandomMinMax(25, 35)));
            pack.DropItem(new Bandage(Utility.RandomMinMax(5, 15)));
            pack.DropItem(new Bandage(Utility.RandomMinMax(5, 15)));
            pack.DropItem(Loot.RandomGem());
            pack.DropItem(new ArcaneGem());

            PackItem(pack);

            AddItem(new JukaBow());

            // TODO: Bandage self
        }

        public override string CorpseName => "a jukan corpse";
        public override string DefaultName => "a juka lord";

        public override bool AlwaysMurderer => true;
        public override bool BardImmune => !Core.AOS;
        public override bool CanRummageCorpses => true;
        public override int Meat => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (!willKill && amount > 5 && from?.Player == true && Utility.Random(100) < 5)
            {
                switch (Utility.Random(4))
                {
                    case 0:
                        {
                            Say(true, $"{from.Name}!!  You will have to do better than that!");
                            break;
                        }
                    case 1:
                        {
                            Say(true, $"{from.Name}!!  Prepare to meet your doom!");
                            break;
                        }
                    case 2:
                        {
                            Say(true, $"{from.Name}!!  My armies will crush you!");
                            break;
                        }
                    default:
                        {
                            Say(true, $"{from.Name}!!  You will pay for that!");
                            break;
                        }
                }
            }

            base.OnDamage(amount, from, willKill);
        }

        public override int GetIdleSound() => 0x262;

        public override int GetAngerSound() => 0x263;

        public override int GetHurtSound() => 0x1D0;

        public override int GetDeathSound() => 0x28D;
    }
}
