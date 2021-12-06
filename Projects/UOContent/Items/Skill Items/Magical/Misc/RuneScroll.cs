using Server.Misc;

namespace Server.Items
{
    [Serializable(0, false)]
    public sealed partial class RuneScroll : Item
    {
        [SerializableField(0)]
        [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster, canModify: true)]")]
        private string _type;
        [SerializableFieldSaveFlag(0)]
        private bool ShouldSerializeType() => !string.IsNullOrEmpty(_type);
        [SerializableFieldDefault(0)]
        private string TypeDefaultValue() => "";

        [SerializableField(1)]
        [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster, canModify: true)]")]
        private string _symbolType;
        [SerializableFieldSaveFlag(1)]
        private bool ShouldSerializeSymbolType() => !string.IsNullOrEmpty(_symbolType);
        [SerializableFieldDefault(1)]
        private string SymbolTypeDefaultValue() => "";

        [SerializableField(2)]
        [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster)]")]
        private string _runeWordName;
        [SerializableFieldSaveFlag(2)]
        private bool ShouldSerializeRuneWordName() => !string.IsNullOrEmpty(_runeWordName);
        [SerializableFieldDefault(2)]
        private string RuneWordNameDefaultValue() => "";

        [InvalidateProperties]
        [SerializableField(3)]
        [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster)]")]
        private bool _identified;
        [SerializableFieldSaveFlag(3)]
        private bool ShouldSerializeIdentified() => true;
        [SerializableFieldDefault(3)]
        private bool IdentifiedDefaultValue() => false;

        [Constructible]
        public RuneScroll() : base(0xEF3)
        {
            _identified = false;
            switch(Utility.Random(1,5))
            {
                case 1:
                    _runeWordName = SocketBonus.RandomClothingRuneWord();
                    _type = "Clothing";
                    break;
                case 2:
                    _runeWordName = SocketBonus.RandomJewelleryRuneWord();
                    _type = "Jewellery";
                    break;
                case 3:
                    _runeWordName = SocketBonus.RandomShieldRuneWord();
                    _type = "Shield";
                    break;
                case 4:
                    _runeWordName = SocketBonus.RandomWeaponRuneWord();
                    _type = "Weapon";
                    break;
                default:
                    _runeWordName = SocketBonus.RandomArmorRuneWord();
                    _type = "Armor";
                    break;
            };
            _symbolType = SocketBonus.GetRuneWordSymbolType(_runeWordName);
            Name = "Ancient Runeword scroll";
            Weight = 1.0;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(
                1060847,
                "Rune name: {0}",
                _runeWordName
            );
            list.Add(
                1060847,
                "Rune symbol: {0}",
                _type
            );
            if (_identified)
            {
                list.Add(
                    1060847,
                    "Symbol identity: {0}",
                    _symbolType
                );
            }
        }
    }
}
