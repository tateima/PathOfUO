using ModernUO.Serialization;
using System;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class CorporealBrume : BaseCreature
    {
        [Constructible]
        public CorporealBrume()
            : base(AIType.AI_Melee)
        {
            Body = 0x104; // TODO: Verify
            BaseSoundID = 0x56B;

            LevelRange = [45, 65];
            StrPerLevel = [3, 6];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(86, 135);
            SetDex(50, 90);
            SetInt(52, 70);

            SetHits(200, 230);

            SetDamage(1, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 15, 10);

            SetSkill(SkillName.Wrestling, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Anatomy, 50.0, 60.5);

            Fame = 12000;
            Karma = -12000;
        }

        public override string CorpseName => "a corporeal brume corpse";
        public override string DefaultName => "a corporeal brume";

        // TODO: Verify area attack specifics
        public override bool HasAura => Combatant != null;
        public override TimeSpan AuraInterval => TimeSpan.FromSeconds(20);
        public override int AuraRange => 10;

        public override int AuraBaseDamage => Utility.RandomMinMax(25, 35);
        public override int AuraFireDamage => 0;
        public override int AuraColdDamage => 100;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
        }

        public override void AuraEffect(Mobile m)
        {
            m.FixedParticles(0x374A, 10, 15, 5038, 1181, 2, EffectLayer.Head);
            m.PlaySound(0x213);
        }
    }
}
