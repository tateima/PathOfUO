using ModernUO.Serialization;
using System;
using Server.Engines.Plants;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class MLDryad : BaseCreature
    {
        private DateTime m_NextPeace;

        private DateTime m_NextUndress;

        [Constructible]
        public MLDryad() : base(AIType.AI_Mage, FightMode.Aggressor)
        {
            Body = 266;
            BaseSoundID = 0x57B;

            LevelRange = [10, 75];
            StrPerLevel = [1, 4];
            IntPerLevel = [2, 7];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(56, 75);
            SetDex(75, 95);
            SetInt(122, 140);

            SetHits(60, 106);

            SetDamage(2, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 50);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.Meditation, 45.4, 50.1);
            SetSkill(SkillName.EvalInt, 45.4, 50.1);
            SetSkill(SkillName.Magery, 45.4, 50.1);
            SetSkill(SkillName.Anatomy, 45.4, 50.1);
            SetSkill(SkillName.MagicResist, 45.4, 50.1);
            SetSkill(SkillName.Tactics, 45.4, 50.1);
            SetSkill(SkillName.Wrestling, 45.4, 50.1);

            Fame = 5000;
            Karma = 5000;

            VirtualArmor = 28; // Don't know what it should be

            if (Core.ML && Utility.RandomDouble() < .60)
            {
                PackItem(Seed.RandomPeculiarSeed(1));
            }

            PackArcanceScroll(0.05);
        }

        public override string CorpseName => "a dryad's corpse";
        public override bool InitialInnocent => true;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override string DefaultName => "a dryad";

        public override int Meat => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.MlRich);
        }

        public override void OnThink()
        {
            base.OnThink();

            AreaPeace();
            AreaUndress();
        }

        public void AreaPeace()
        {
            if (Combatant == null || Deleted || !Alive || m_NextPeace > Core.Now || Utility.RandomDouble() < 0.9)
            {
                return;
            }

            var duration = TimeSpan.FromSeconds(Utility.RandomMinMax(20, 80));

            foreach (var m in GetMobilesInRange(RangePerception))
            {
                if (m is PlayerMobile pm && IsValidTarget(pm))
                {
                    pm.PeacedUntil = Core.Now + duration;
                    m.SendLocalizedMessage(1072065); // You gaze upon the dryad's beauty, and forget to continue battling!
                    m.FixedParticles(0x376A, 1, 20, 0x7F5, EffectLayer.Waist);
                    m.Combatant = null;
                }
            }

            m_NextPeace = Core.Now + TimeSpan.FromSeconds(10);
            PlaySound(0x1D3);
        }

        public bool IsValidTarget(PlayerMobile m) =>
            m?.PeacedUntil < Core.Now && !m.Hidden && m.AccessLevel == AccessLevel.Player &&
            CanBeHarmful(m);

        public void AreaUndress()
        {
            if (Combatant == null || Deleted || !Alive || m_NextUndress > Core.Now || Utility.RandomDouble() >= 0.005)
            {
                return;
            }

            foreach (var m in GetMobilesInRange(RangePerception))
            {
                if (m?.Player == true && !m.Female && !m.Hidden && m.AccessLevel == AccessLevel.Player &&
                    CanBeHarmful(m))
                {
                    UndressItem(m, Layer.OuterTorso);
                    UndressItem(m, Layer.InnerTorso);
                    UndressItem(m, Layer.MiddleTorso);
                    UndressItem(m, Layer.Pants);
                    UndressItem(m, Layer.Shirt);

                    // The dryad's beauty makes your blood race. Your clothing is too confining.
                    m.SendLocalizedMessage(1072197);
                }
            }

            m_NextUndress = Core.Now + TimeSpan.FromMinutes(1);
        }

        public static void UndressItem(Mobile m, Layer layer)
        {
            var item = m.FindItemOnLayer(layer);

            if (item?.Movable == true)
            {
                m.PlaceInBackpack(item);
            }
        }
    }
}
