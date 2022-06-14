using System;
using Server.Buffers;
using Server.Collections;

namespace Server.Mobiles;

public class EnergyVortex : BaseCreature
{
    [Constructible]
    public EnergyVortex() : base(AIType.AI_Melee)
    {
        if (Core.SE && Utility.Random(500) == 0) // Per OSI FoF, it's a 1/500 chance.
        {
            // Llama vortex!
            Body = 0xDC;
            Hue = 0x76;
        }
        else
        {
            Body = 164;
        }

        SetStr(200);
        SetDex(200);
        SetInt(100);

        SetHits(Core.SE ? 140 : 70);
        SetStam(250);
        SetMana(0);

        SetDamage(14, 17);

        SetDamageType(ResistanceType.Physical, 0);
        SetDamageType(ResistanceType.Energy, 100);

        SetResistance(ResistanceType.Physical, 60, 70);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 90, 100);

        SetSkill(SkillName.MagicResist, 99.9);
        SetSkill(SkillName.Tactics, 100.0);
        SetSkill(SkillName.Wrestling, 120.0);

        Fame = 0;
        Karma = 0;

        VirtualArmor = 40;
        ControlSlots = Core.SE ? 2 : 1;
    }

    public EnergyVortex(Serial serial)
        : base(serial)
    {
    }

    public override string CorpseName => "an energy vortex corpse";
    public override bool DeleteCorpseOnDeath => Summoned;
    public override bool AlwaysMurderer => true; // Or Llama vortices will appear gray.

    public override double DispelDifficulty => 80.0;
    public override double DispelFocus => 20.0;

    public override string DefaultName => "an energy vortex";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override double GetFightModeRanking(Mobile m, FightMode acqType, bool bPlayerOnly) =>
        (m.Int + m.Skills.Magery.Value) / Math.Max(GetDistanceToSqrt(m), 1.0);

    public override int GetAngerSound() => 0x15;

    public override int GetAttackSound() => 0x28;

    public override void OnThink()
    {
        if (Core.SE && Summoned)
        {
            var eable = GetMobilesInRange(5);
            using var queue = PooledRefQueue<Mobile>.Create();
            foreach (var m in eable)
            {
                if (m is EnergyVortex or BladeSpirits && ((BaseCreature)m).Summoned)
                {
                    queue.Enqueue(m);
                }
            }
            eable.Free();

            var amount = queue.Count - 6;
            if (amount > 0)
            {
                var mobs = queue.ToPooledArray();
                mobs.Shuffle();

                while (amount > 0)
                {
                    Dispel(mobs[amount--]);
                }

                STArrayPool<Mobile>.Shared.Return(mobs, true);
            }
        }

        base.OnThink();
    }

    public override void Serialize(IGenericWriter writer)
    {
        base.Serialize(writer);

        writer.Write(0); // version
    }

    public override void Deserialize(IGenericReader reader)
    {
        base.Deserialize(reader);

        var version = reader.ReadInt();

        if (BaseSoundID == 263)
        {
            BaseSoundID = 0;
        }
    }
}
