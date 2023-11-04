using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

[SerializationGenerator(0)]
public partial class DarkGuardian : BaseCreature
{
    [Constructible]
    public DarkGuardian() : base(AIType.AI_Mage)
    {
        Body = 78;
        BaseSoundID = 0x3E9;

        SetStr(125, 150);
        SetDex(100, 120);
        SetInt(200, 235);

        SetHits(150, 180);

        SetDamage(43, 48);

        SetDamageType(ResistanceType.Physical, 10);
        SetDamageType(ResistanceType.Cold, 40);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 20, 45);
        SetResistance(ResistanceType.Cold, 50, 60);
        SetResistance(ResistanceType.Poison, 20, 45);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.EvalInt, 40.1, 50);
        SetSkill(SkillName.Magery, 50.1, 60.0);
        SetSkill(SkillName.Meditation, 85.1, 95.0);
        SetSkill(SkillName.MagicResist, 50.1, 70.0);
        SetSkill(SkillName.Tactics, 50.1, 70.0);

        Fame = 5000;
        Karma = -5000;

        VirtualArmor = 50;
        PackNecroReg(15, 25);
        PackItem(new DaemonBone(30));
    }

    public override string CorpseName => "a dark guardians' corpse";
    public override string DefaultName => "a dark guardian";

    public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

    public override int TreasureMapLevel => 2;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;
    public override bool Unprovokable => true;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
        AddLoot(LootPack.MedScrolls, 2);
    }
}
