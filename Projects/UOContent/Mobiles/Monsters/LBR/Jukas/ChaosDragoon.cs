using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class ChaosDragoon : BaseCreature
    {
        [Constructible]
        public ChaosDragoon() : base(AIType.AI_Melee)
        {
            Body = 0x190;
            Hue = Race.Human.RandomSkinHue();

            SetSpeed(0.15, 0.4);

            LevelRange = [45, 65];
            StrPerLevel = [2, 3];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 2];

            SetStr(76, 105);
            SetDex(40, 70);
            SetInt(72, 90);

            SetHits(130, 180);

            SetDamage(4, 8);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 5, 35);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.Fencing, 48.0, 58.5);
            SetSkill(SkillName.Healing, 48.0, 58.5);
            SetSkill(SkillName.Macing, 48.0, 58.5);
            SetSkill(SkillName.Anatomy, 48.0, 58.5);
            SetSkill(SkillName.MagicResist, 48.0, 58.5);
            SetSkill(SkillName.Swords, 48.0, 58.5);
            SetSkill(SkillName.Tactics, 48.0, 58.5);

            Fame = 5000;
            Karma = -5000;

            var res = Utility.Random(6) switch
            {
                0 => CraftResource.BlackScales,
                1 => CraftResource.RedScales,
                2 => CraftResource.BlueScales,
                3 => CraftResource.YellowScales,
                4 => CraftResource.GreenScales,
                _ => CraftResource.WhiteScales // 5
            };

            var melee = Utility.Random(3) switch
            {
                0 => (BaseWeapon)new Kryss(),
                1 => new Broadsword(),
                _ => new Katana() // 2
            };

            melee.Movable = false;
            AddItem(melee);

            AddItem(new DragonHelm { Resource = res, Movable = false });
            AddItem(new DragonChest { Resource = res, Movable = false });
            AddItem(new DragonArms { Resource = res, Movable = false });
            AddItem(new DragonGloves { Resource = res, Movable = false });
            AddItem(new DragonLegs { Resource = res, Movable = false });
            AddItem(new ChaosShield { Movable = false });

            AddItem(new Shirt());
            AddItem(new Boots());

            var amount = Utility.RandomMinMax(1, 3);

            AddItem(
                res switch
                {
                    CraftResource.BlackScales  => new BlackScales(amount),
                    CraftResource.RedScales    => new RedScales(amount),
                    CraftResource.BlueScales   => new BlueScales(amount),
                    CraftResource.YellowScales => new YellowScales(amount),
                    CraftResource.GreenScales  => new GreenScales(amount),
                    _                          => new WhiteScales(amount) // CraftResource.WhiteScales
                }
            );

            new SwampDragon().Rider = this;
        }

        public override string CorpseName => "a chaos dragoon corpse";
        public override string DefaultName => "a chaos dragoon";

        public override bool AutoDispel => true;
        public override bool BardImmune => !Core.AOS;
        public override bool CanRummageCorpses => true;
        public override bool AlwaysMurderer => true;
        public override bool ShowFameTitle => false;

        private static MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override int GetIdleSound() => 0x2CE;

        public override int GetDeathSound() => 0x2CC;

        public override int GetHurtSound() => 0x2D1;

        public override int GetAttackSound() => 0x2C8;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            // AddLoot( LootPack.Gems );
        }

        public override bool OnBeforeDeath()
        {
            var mount = Mount;

            if (mount != null)
            {
                mount.Rider = null;
            }

            return base.OnBeforeDeath();
        }

        public override void AlterMeleeDamageTo(Mobile to, ref int damage)
        {
            if (to is Dragon or WhiteWyrm or SwampDragon or Drake or Nightmare or Hiryu or LesserHiryu or Daemon)
            {
                damage *= 3;
            }
            base.AlterMeleeDamageTo(to, ref damage);
        }
    }
}
