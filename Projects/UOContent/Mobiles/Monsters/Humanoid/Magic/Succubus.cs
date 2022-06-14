namespace Server.Mobiles
{
    public class Succubus : BaseCreature
    {
        [Constructible]
        public Succubus() : base(AIType.AI_Mage)
        {
            Body = 149;
            BaseSoundID = 0x4B0;

            SetStr(488, 620);
            SetDex(121, 170);
            SetInt(498, 657);

            SetHits(312, 353);

            SetDamage(18, 28);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 99.1, 100.0);
            SetSkill(SkillName.Meditation, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 80.1, 90.0);
            SetSkill(SkillName.Wrestling, 80.1, 90.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 80;
        }

        public Succubus(Serial serial) : base(serial)
        {
        }

        public override string CorpseName => "a succubus corpse";
        public override string DefaultName => "a succubus";

        public override int Meat => 1;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.MedScrolls, 2);
        }

        public void DrainLife()
        {
            var eable = GetMobilesInRange(2);

            foreach (var m in eable)
            {
                if (m == this || !CanBeHarmful(m) ||
                    !(m.Player || m is BaseCreature creature &&
                        (creature.Controlled || creature.Summoned || creature.Team != Team)))
                {
                    continue;
                }

                DoHarmful(m);

                m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
                m.PlaySound(0x231);

                // m.SendMessage( "You feel the life drain out of you!" );

                var toDrain = Utility.RandomMinMax(10, 40);

                Hits += toDrain;
                m.Damage(toDrain, this);
            }

            eable.Free();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() <= 0.1)
            {
                DrainLife();
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() <= 0.1)
            {
                DrainLife();
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
