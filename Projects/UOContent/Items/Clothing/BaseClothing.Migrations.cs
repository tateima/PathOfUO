using Server.Pantheon;
using Server.Talent;

namespace Server.Items;

public partial class BaseClothing
{
    // private void MigrateFrom(V7Content content)
    // {
    //     _resource = content.Resource ?? DefaultResource;
    //     _attributes = content.Attributes ?? AttributesDefaultValue();
    //     _clothingAttributes = content.ClothingAttributes ?? ClothingAttributesDefaultValue();
    //     _skillBonuses = content.SkillBonuses ?? SkillBonusesDefaultValue();
    //     _resistances = content.Resistances ?? ResistancesDefaultValue();
    //     _maxHitPoints = content.MaxHitPoints ?? 0;
    //     _playerConstructed = content.PlayerConstructed;
    //     _crafter = content.Crafter ?? "";
    //     _quality = content.Quality ?? ClothingQuality.Regular;
    //     _strReq = content.StrRequirement ?? -1;
    //     // _pockets = content.Pockets ?? "";
    //     // _alignmentRaw = content.AlignmentRaw ?? Deity.Alignment.None.ToString();
    //     // _pocketAmount = content.PocketAmount ?? 0;
    //     // _talentIndex = content.TalentIndex;
    //     // _talentLevel = content.TalentLevel ?? 0;
    //     // _talentLevel = 0;
    //     // _talentIndex = BaseTalent.InvalidTalentIndex;
    //     // _pocketAmount = 0;
    //     // _pockets = "";
    //     // _alignmentRaw = "None";
    //     // _temporary = false;
    // }
    private void MigrateFrom(V6Content content)
    {
        _resource = content.RawResource ?? DefaultResource;
        _attributes = content.Attributes ?? AttributesDefaultValue();
        _clothingAttributes = content.ClothingAttributes ?? ClothingAttributesDefaultValue();
        _skillBonuses = content.SkillBonuses ?? SkillBonusesDefaultValue();
        _resistances = content.Resistances ?? ResistancesDefaultValue();
        _maxHitPoints = content.MaxHitPoints ?? 0;
        _playerConstructed = content.PlayerConstructed;
        var crafter = content.Crafter;
        Timer.StartTimer(() => _crafter = crafter?.RawName);
        _quality = content.Quality ?? ClothingQuality.Regular;
        _strReq = content.StrRequirement ?? -1;
    }

    // Version 5 (pre-codegen)
    private void Deserialize(IGenericReader reader, int version)
    {
        var flags = (OldSaveFlag)reader.ReadEncodedInt();

        if (GetSaveFlag(flags, OldSaveFlag.Resource))
        {
            _resource = (CraftResource)reader.ReadEncodedInt();
        }
        else
        {
            _resource = DefaultResource;
        }

        Attributes = new AosAttributes(this);

        if (GetSaveFlag(flags, OldSaveFlag.Attributes))
        {
            Attributes.Deserialize(reader);
        }

        ClothingAttributes = new AosArmorAttributes(this);

        if (GetSaveFlag(flags, OldSaveFlag.ClothingAttributes))
        {
            ClothingAttributes.Deserialize(reader);
        }

        SkillBonuses = new AosSkillBonuses(this);

        if (GetSaveFlag(flags, OldSaveFlag.SkillBonuses))
        {
            SkillBonuses.Deserialize(reader);
        }

        Resistances = new AosElementAttributes(this);

        if (GetSaveFlag(flags, OldSaveFlag.Resistances))
        {
            Resistances.Deserialize(reader);
        }

        if (GetSaveFlag(flags, OldSaveFlag.MaxHitPoints))
        {
            _maxHitPoints = reader.ReadEncodedInt();
        }

        if (GetSaveFlag(flags, OldSaveFlag.HitPoints))
        {
            _hitPoints = reader.ReadEncodedInt();
        }

        if (GetSaveFlag(flags, OldSaveFlag.Crafter))
        {
            var crafter = reader.ReadEntity<Mobile>();
            Timer.StartTimer(() => _crafter = crafter?.RawName);
        }

        if (GetSaveFlag(flags, OldSaveFlag.Quality))
        {
            _quality = (ClothingQuality)reader.ReadEncodedInt();
        }

        if (GetSaveFlag(flags, OldSaveFlag.StrReq))
        {
            _strReq = reader.ReadEncodedInt();
        }

        PlayerConstructed = GetSaveFlag(flags, OldSaveFlag.PlayerConstructed);
    }
}
