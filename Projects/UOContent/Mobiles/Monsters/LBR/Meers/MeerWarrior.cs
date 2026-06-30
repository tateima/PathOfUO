using ModernUO.Serialization;
using System;
using Server.Spells;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class MeerWarrior : BaseCreature
    {
        [Constructible]
        public MeerWarrior() : base(AIType.AI_Melee, FightMode.Aggressor)
        {
            Body = 771;
            LevelRange = [30, 50];
            StrPerLevel = [2, 3];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 2];

            SetStr(36, 65);
            SetDex(50, 85);
            SetInt(22, 50);

            SetHits(125, 150);

            SetDamage(3, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 15, 20);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 40.0, 50.5);
            SetSkill(SkillName.Tactics, 40.0, 50.5);
            SetSkill(SkillName.Wrestling, 40.0, 50.5);

            VirtualArmor = 22;

            Fame = 2000;
            Karma = 5000;
        }

        public override string CorpseName => "a meer corpse";
        public override string DefaultName => "a meer warrior";

        public override bool BardImmune => !Core.AOS;
        public override bool CanRummageCorpses => true;

        public override bool InitialInnocent => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (from != null && !willKill && amount > 3 && !InRange(from, 7))
            {
                MovingEffect(from, 0xF51, 10, 0, false, false);
                SpellHelper.Damage(
                    TimeSpan.FromSeconds(1.0),
                    from,
                    this,
                    Utility.RandomMinMax(30, 40) - (Core.AOS ? 0 : 10),
                    100,
                    0,
                    0,
                    0,
                    0
                );
            }

            base.OnDamage(amount, from, willKill);
        }

        public override int GetHurtSound() => 0x156;

        public override int GetDeathSound() => 0x15C;
    }
}
