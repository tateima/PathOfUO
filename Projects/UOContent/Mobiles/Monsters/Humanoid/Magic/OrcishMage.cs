using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    public class OrcishMage : BaseCreature
    {
        private bool IsNecromancer { get; set; }
        [Constructible]
        public OrcishMage() : base(AIType.AI_Mage)
        {
            Body = 140;
            BaseSoundID = 0x45A;
            IsNecromancer = Utility.Random(100) < 10;
            int resistanceBuffs = 0;
            int skillBuffs = 0;
            int fameBuff = 0;
            SetDex(91, 115);
            SetStr(116, 150);
            if (IsNecromancer)
            {
                Hue = 0x322;
                SetInt(181, 1055);
                SetHits(170, 190);
                SetDamageType(ResistanceType.Cold, 100);
                SetSkill(SkillName.Necromancy, 90.1, 95.5);
                resistanceBuffs = 20;
                skillBuffs = 10;
                fameBuff = 2000;
                SetDamage(3, 6);
                PackItem(new BagOfNecroReagents());
            }
            else
            {
                SetInt(161, 185);
                SetHits(70, 90);
                SetDamageType(ResistanceType.Physical, 100);
                SetSkill(SkillName.Magery, 60.1, 72.5);
                SetDamage(4, 14);
            }

            SetResistance(ResistanceType.Physical, 25, 35 + resistanceBuffs);
            SetResistance(ResistanceType.Fire, 30, 40 + resistanceBuffs);
            SetResistance(ResistanceType.Cold, 20, 30 + resistanceBuffs);
            SetResistance(ResistanceType.Poison, 30, 40 + resistanceBuffs);
            SetResistance(ResistanceType.Energy, 30, 40 + resistanceBuffs);

            SetSkill(SkillName.EvalInt, 60.1 + skillBuffs, 72.5 + skillBuffs);
            SetSkill(SkillName.MagicResist, 60.1 + skillBuffs, 75.0 + skillBuffs);
            SetSkill(SkillName.Tactics, 50.1 + skillBuffs, 65.0 + skillBuffs);
            SetSkill(SkillName.Wrestling, 40.1 + skillBuffs, 50.0 + skillBuffs);

            Fame = 3000 + fameBuff;
            Karma = -3000 - fameBuff;

            VirtualArmor = 30;

            PackReg(6);

            if (Utility.RandomDouble() < 0.05)
            {
                PackItem(new OrcishKinMask());
            }
        }

        public OrcishMage(Serial serial) : base(serial)
        {
        }

        public override string CorpseName => IsNecromancer ? "a dark orc corpse" : "a glowing orc corpse";
        public override InhumanSpeech SpeechType => InhumanSpeech.Orc;

        public override string DefaultName => IsNecromancer ? "an orcish necromancer" : "an orcish mage";

        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 1;
        public override int Meat => 1;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.LowScrolls);
        }

        public override bool IsEnemy(Mobile m)
        {
            if (m.Player && m.FindItemOnLayer(Layer.Helm) is OrcishKinMask)
            {
                return false;
            }

            return base.IsEnemy(m);
        }

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            var item = aggressor.FindItemOnLayer(Layer.Helm);

            if (item is OrcishKinMask)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                item.Delete();
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
            }
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
        }
    }
}
