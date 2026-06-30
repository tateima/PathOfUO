using ModernUO.Serialization;
using System;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class JukaWarrior : BaseCreature
    {
        [Constructible]
        public JukaWarrior() : base(AIType.AI_Melee)
        {
            Body = 764;
            LevelRange = [54, 74];
            StrPerLevel = [2, 6];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 2];

            SetStr(86, 105);
            SetDex(50, 80);
            SetInt(12, 70);

            SetHits(155, 200);

            SetDamage(5, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 10, 15);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.Anatomy, 53.0, 63.5);
            SetSkill(SkillName.Fencing, 53.0, 63.5);
            SetSkill(SkillName.Macing, 53.0, 63.5);
            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Swords, 53.0, 63.5);
            SetSkill(SkillName.Tactics, 53.0, 63.5);
            SetSkill(SkillName.Wrestling, 53.0, 63.5);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 22;

            if (Utility.RandomDouble() < 0.1)
            {
                PackItem(new ArcaneGem());
            }
        }

        public override string CorpseName => "a jukan corpse";
        public override string DefaultName => "a juka warrior";

        public override bool AlwaysMurderer => true;
        public override bool CanRummageCorpses => true;
        public override int Meat => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.Gems, 1);
        }

        public override int GetIdleSound() => 0x1AC;

        public override int GetAngerSound() => 0x1CD;

        public override int GetHurtSound() => 0x1D0;

        public override int GetDeathSound() => 0x28D;

        public override void OnGaveMeleeAttack(Mobile defender, int damage)
        {
            base.OnGaveMeleeAttack(defender, damage);

            if (Utility.RandomDouble() < 0.80)
            {
                return;
            }

            switch (Utility.Random(3))
            {
                case 0:
                    {
                        defender.SendLocalizedMessage(1004014); // You have been stunned!
                        defender.Freeze(TimeSpan.FromSeconds(4.0));
                        break;
                    }
                case 1:
                    {
                        defender.SendLocalizedMessage(1072221); // You have been hit by a paralyzing blow!
                        defender.Freeze(TimeSpan.FromSeconds(3.0));
                        break;
                    }
                case 2:
                    {
                        AOS.Damage(defender, this, Utility.Random(10, 5), 100, 0, 0, 0, 0);
                        defender.SendAsciiMessage("You have been hit by a critical strike!");
                        break;
                    }
            }
        }
    }
}
