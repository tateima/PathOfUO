using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class ArcherGuard : BaseGuard
{
    [Constructible]
    public ArcherGuard(int maxStrength = 3)
    {
        AiType = AIType.AI_Archer;
        SetHumanoidStrength(maxStrength);
        InitStats(100, 125, 25);
        Title = "the guard";

        SpeechHue = Utility.RandomDyedHue();

        Hue = Race.Human.RandomSkinHue();

        if (Female = Utility.RandomBool())
        {
            Body = 0x191;
            Name = NameList.RandomName("female");
        }
        else
        {
            Body = 0x190;
            Name = NameList.RandomName("male");
        }

        new Horse().Rider = this;

        AddItem(new StuddedChest());
        AddItem(new StuddedArms());
        AddItem(new StuddedGloves());
        AddItem(new StuddedGorget());
        AddItem(new StuddedLegs());
        AddItem(new Boots());
        AddItem(new SkullCap());

        var bow = new Bow();

        bow.Movable = false;
        bow.Crafter = Name;
        bow.Quality = WeaponQuality.Exceptional;

        AddItem(bow);

        Container pack = new Backpack();

        pack.Movable = false;

        var arrows = new Arrow(250);

        arrows.LootType = LootType.Newbied;

        pack.DropItem(arrows);
        pack.DropItem(new Gold(10, 25));

        AddItem(pack);

        Skills.Anatomy.Base = Utility.RandomMinMax(75.0, 100.0);
        Skills.Tactics.Base = Utility.RandomMinMax(75.0, 100.0);
        Skills.Archery.Base = Utility.RandomMinMax(75.0, 100.0);
        Skills.MagicResist.Base = Utility.RandomMinMax(75.0, 100.0);
        Skills.DetectHidden.Base = Utility.RandomMinMax(75.0, 100.0);
        NextCombatTime = Core.TickCount + 500;
        }
    public override Mobile Focus { get; set; }
}
