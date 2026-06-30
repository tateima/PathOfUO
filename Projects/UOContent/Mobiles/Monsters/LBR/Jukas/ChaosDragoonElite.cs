using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class ChaosDragoonElite : BaseCreature
    {
        [Constructible]
        public ChaosDragoonElite() : base(AIType.AI_Mage)
        {
            Body = 0x190;
            Hue = Race.Human.RandomSkinHue();

            SetSpeed(0.15, 0.4);

            LevelRange = [63, 78];
            StrPerLevel = [3, 4];
            IntPerLevel = [3, 4];
            DexPerLevel = [3, 4];
            ResistancePerLevel = [2, 3];

            SetStr(96, 125);
            SetDex(70, 90);
            SetInt(82, 110);

            SetHits(230, 280);

            SetDamage(6, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 35);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.Tactics, 53.0, 63.5);
            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Anatomy, 53.0, 63.5);
            SetSkill(SkillName.Magery, 53.0, 63.5);
            SetSkill(SkillName.EvalInt, 53.0, 63.5);
            SetSkill(SkillName.Swords, 53.0, 63.5);
            SetSkill(SkillName.Fencing, 53.0, 63.5);
            SetSkill(SkillName.Macing, 53.0, 63.5);

            Fame = 8000;
            Karma = -8000;

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

            AddItem(new DragonChest { Resource = res, Movable = false });
            AddItem(new DragonLegs { Resource = res, Movable = false });
            AddItem(new DragonArms { Resource = res, Movable = false });
            AddItem(new DragonGloves { Resource = res, Movable = false });
            AddItem(new DragonHelm { Resource = res, Movable = false });
            AddItem(new ChaosShield { Movable = false });

            AddItem(new Boots(0x455));
            AddItem(new Shirt(Utility.RandomMetalHue()));

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

            res = Utility.Random(9) switch
            {
                0 => CraftResource.DullCopper,
                1 => CraftResource.ShadowIron,
                2 => CraftResource.Copper,
                3 => CraftResource.Bronze,
                4 => CraftResource.Gold,
                5 => CraftResource.Agapite,
                6 => CraftResource.Verite,
                7 => CraftResource.Valorite,
                _ => CraftResource.Iron // 8
            };

            var mt = new SwampDragon { HasBarding = true, BardingResource = res };
            mt.BardingHP = mt.BardingMaxHP;
            mt.Rider = this;
        }

        public ChaosDragoonElite(Serial serial)
            : base(serial)
        {
        }

        public override string CorpseName => "a chaos dragoon elite corpse";
        public override string DefaultName => "a chaos dragoon elite";
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
            AddLoot(LootPack.Gems);
        }

        public override bool OnBeforeDeath()
        {
            var mount = Mount;

            if (mount != null)
            {
                if (mount is SwampDragon dragon)
                {
                    dragon.HasBarding = false;
                }

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
        }
    }
}
