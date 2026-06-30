using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class OrcBrute : BaseCreature
    {
        [Constructible]
        public OrcBrute() : base(AIType.AI_Melee)
        {
            Body = 189;
            BaseSoundID = 0x45A;
            LevelRange = [6, 11];
            StrPerLevel = [4, 5];
            IntPerLevel = [2, 3];
            DexPerLevel = [7, 10];
            ResistancePerLevel = [1, 2];

            SetStr(25, 35);
            SetDex(19, 35);
            SetInt(10, 15);

            SetHits(30, 66);

            SetDamage(6, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 11, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.Macing, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 50;

            PackItem(new ShadowIronOre(25)
            {
                ItemID = 0x19B9
            });
            PackItem(new IronIngot(10));

            if (Utility.RandomDouble() < 0.05)
            {
                PackItem(new OrcishKinMask());
            }

            if (Utility.RandomDouble() < 0.2)
            {
                PackItem(new BolaBall());
            }
        }

        public override string CorpseName => "an orcish corpse";
        public override string DefaultName => "an orc brute";

        public override bool BardImmune => !Core.AOS;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int Meat => 2;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.SavagesAndOrcs };

        public override bool CanRummageCorpses => true;
        public override bool AutoDispel => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
        }

        public override bool IsEnemy(Mobile m) =>
            (!m.Player || m.FindItemOnLayer<OrcishKinMask>(Layer.Helm) == null) && base.IsEnemy(m);

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            if (aggressor.FindItemOnLayer(Layer.Helm) is OrcishKinMask item)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                item.Delete();
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
            }
        }

        public override void OnDamagedBySpell(Mobile caster, int damage)
        {
            if (caster == this)
            {
                return;
            }

            SpawnOrcLord(caster);
        }

        public void SpawnOrcLord(Mobile target)
        {
            var map = target.Map;

            if (map == null || map == Map.Internal)
            {
                return;
            }

            var count = 0;
            foreach (var m in GetMobilesInRange<OrcishLord>(10))
            {
                if (++count == 10)
                {
                    return;
                }
            }

            var location = map.GetRandomNearbyLocation(target.Location);
            var orc = new SpawnedOrcishLord
            {
                Team = Team,
                Home = location,
                RangeHome = 10,
                Combatant = target
            };

            orc.MoveToWorld(location, map);
        }
    }
}
