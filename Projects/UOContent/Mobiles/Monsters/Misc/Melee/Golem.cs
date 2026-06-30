using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Golem : BaseCreature
    {
        [Constructible]
        public Golem(bool summoned = false, double scalar = 1.0, int level = 1) : base(AIType.AI_Melee)
        {
            Body = 752;

            if (summoned)
            {
                Hue = 2101;
            }

            if (summoned)
            {
                LevelRange = [level, level];
            }
            else
            {
                LevelRange = [19, 32];
            }

            StrPerLevel = [2, 7];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr((int)(70 * scalar) , (int)(125 * scalar));
            SetDex((int)(30 * scalar), (int)(55* scalar));
            SetInt((int)(25 * scalar), (int)(30* scalar));
            SetHits((int)(85 * scalar), (int)(110* scalar));
            SetDamage((int)(2 * scalar), (int)(5* scalar));

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, (int)(5 * scalar), (int)(25 * scalar));
            SetResistance(ResistanceType.Fire, (int)(5 * scalar), (int)(20 * scalar));

            SetResistance(ResistanceType.Cold, (int)(5 * scalar), (int)(15 * scalar));
            SetResistance(ResistanceType.Poison, (int)(5 * scalar), (int)(15 * scalar));
            SetResistance(ResistanceType.Energy, (int)(10 * scalar), (int)(30 * scalar));

            SetSkill(SkillName.MagicResist, 40.1 * scalar, 50 * scalar);
            SetSkill(SkillName.Tactics, 40.1 * scalar, 50.0 * scalar);
            SetSkill(SkillName.Wrestling, 40.1 * scalar, 50.0 * scalar);

            if (summoned)
            {
                Fame = 10;
                Karma = 10;
            }
            else
            {
                Fame = 3500;
                Karma = -3500;
            }

            if (!summoned)
            {
                PackItem(new IronIngot(Utility.RandomMinMax(13, 21)));

                if (Utility.RandomDouble() < 0.1)
                {
                    PackItem(new PowerCrystal());
                }

                if (Utility.RandomDouble() < 0.15)
                {
                    PackItem(new ClockworkAssembly());
                }

                if (Utility.RandomDouble() < 0.2)
                {
                    PackItem(new ArcaneGem());
                }

                if (Utility.RandomDouble() < 0.25)
                {
                    PackItem(new Gears());
                }
            }

            ControlSlots = 3;
        }

        public override string CorpseName => "a golem corpse";

        public override bool IsScaredOfScaryThings => false;
        public override bool IsScaryToPets => true;

        public override bool IsBondable => false;

        public override FoodType FavoriteFood => FoodType.None;

        public override bool CanBeDistracted => false;

        public override string DefaultName => "a golem";

        public override bool DeleteOnRelease => true;

        public override bool AutoDispel => !Controlled;
        public override bool BleedImmune => true;

        public override bool BardImmune => !Core.AOS || Controlled;
        public override Poison PoisonImmune => Poison.Lethal;

        private static MonsterAbility[] _abilities = { MonsterAbilities.ColossalBlow };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.05)
            {
                if (!IsParagon)
                {
                    if (Utility.RandomDouble() < 0.75)
                    {
                        c.DropItem(DawnsMusicGear.RandomCommon);
                    }
                    else
                    {
                        c.DropItem(DawnsMusicGear.RandomUncommon);
                    }
                }
                else
                {
                    c.DropItem(DawnsMusicGear.RandomRare);
                }
            }
        }

        public override int GetAngerSound() => 541;

        public override int GetIdleSound() => !Controlled ? 542 : base.GetIdleSound();

        public override int GetDeathSound() => !Controlled ? 545 : base.GetDeathSound();

        public override int GetAttackSound() => 562;

        public override int GetHurtSound() => Controlled ? 320 : base.GetHurtSound();

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (Controlled || Summoned)
            {
                var master = ControlMaster ?? SummonMaster;

                if (master?.Player == true && master.Map == Map && master.InRange(Location, 20))
                {
                    if (master.Mana >= amount)
                    {
                        master.Mana -= amount;
                    }
                    else
                    {
                        amount -= master.Mana;
                        master.Mana = 0;
                        master.Damage(amount);
                    }
                }
            }

            base.OnDamage(amount, from, willKill);
        }
    }
}
