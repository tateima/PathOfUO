using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class LadyJennifyr : SkeletalKnight
{
    [Constructible]
    public LadyJennifyr()
    {
        IsParagon = true;

        Hue = 0x76D;

        LevelRange = [50, 60];
        StrPerLevel = [3, 6];
        IntPerLevel = [1, 2];
        DexPerLevel = [3, 5];
        ResistancePerLevel = [2, 3];

        SetStr(80, 175);
        SetDex(50, 75);
        SetInt(55, 80);
        SetHits(95, 180);

        SetDamage(6, 10);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 5, 30);
        SetResistance(ResistanceType.Fire, 10, 25);
        SetResistance(ResistanceType.Cold, 10, 30);
        SetResistance(ResistanceType.Poison, 10, 20);
        SetResistance(ResistanceType.Energy, 10, 25);

        SetSkill(SkillName.Wrestling, 50.9, 65.1);
        SetSkill(SkillName.Tactics, 50.4, 65.9);
        SetSkill(SkillName.MagicResist, 50.1, 65.5);
        SetSkill(SkillName.Anatomy, 50.0, 65.5);

        Fame = 18000;
        Karma = -18000;
    }

    public override string CorpseName => "a Lady Jennifyr corpse";
    public override string DefaultName => "Lady Jennifyr";

    /*
    // TODO: Uncomment once added
    public override void OnDeath( Container c )
    {
      base.OnDeath( c );

      if (Utility.RandomDouble() < 0.15)
        c.DropItem( new DisintegratingThesisNotes() );

      if (Utility.RandomDouble() < 0.1)
        c.DropItem( new ParrotItem() );
    }
    */

    public override bool GivesMLMinorArtifact => true;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.UltraRich, 3);
    }

    private static readonly MonsterAbility[] _abilities =
    [
        new FanningFire(0.10, -10, 35, 45)
    ];

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;
}
