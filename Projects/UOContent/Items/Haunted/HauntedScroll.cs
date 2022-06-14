using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;
using Server.Talent;
using ModernUO.Serialization;

namespace Server.Items
{
    [SerializationGenerator(0, false)]
    public partial class HauntedScroll : Item
    {
        [SerializableField(0)]
        [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster)]")]
        public Point2D _chapterLocation;

        [SerializableFieldSaveFlag(0)]
        private bool ShouldSerializeChapterLocation() => _chapterLocation.X > 0 || _chapterLocation.Y > 0;

        [SerializableField(1)]
        [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster)]")]
        [InvalidateProperties]
        public int _hookNumber;

        [SerializableFieldSaveFlag(1)]
        private bool ShouldSerializeHookNumber() => _hookNumber > 0;


        [SerializableField(2)]
        [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster)]")]
        [InvalidateProperties]
        public string[] _content;

        [SerializableFieldSaveFlag(2)]
        private bool ShouldSerializeContent() => _content.Length > 0;

        [SerializableField(3)]
        [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster)]")]
        [InvalidateProperties]
        public string _mapKey;

        [SerializableFieldSaveFlag(3)]
        private bool ShouldSerializeMapKey() => _mapKey != null;

        [SerializableField(4)]
        [SerializableFieldAttr("[CommandProperty(AccessLevel.GameMaster)]")]
        [InvalidateProperties]
        public string _protagonist;

        [SerializableFieldSaveFlag(4)]
        private bool ShouldSerializeProtagonist() => _protagonist != null;

        [Constructible]
        public HauntedScroll(string protagonist, List<string> hooks, string mapKey) : base(0x46B2)
        {
            ItemID = Utility.Random(12) switch
            {
                1  => 0x0E35,
                2  => 0x0E36,
                3  => 0x0E37,
                4  => 0x0E38,
                5  => 0x0E39,
                6  => 0x0E3A,
                7  => 0x0EF4,
                8  => 0x0EF5,
                9  => 0x0EF6,
                10 => 0x0EF7,
                11 => 0x0EF8,
                12 => 0x0EF9,
                _  => ItemID
            };

            string mapContext = (mapKey == "trammel_") ? "britannian" : mapKey.Replace("_", "");
            _content = hooks.ToArray();
            _protagonist = protagonist;
            _mapKey = mapKey;
            Name = $"a {mapContext} haunted scroll";
            _hookNumber = 1;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile playerMobile)
            {
                if (playerMobile.GetTalent(typeof(Mediumship)) is Mediumship mediumship)
                {
                    from.SendGump(
                        new MiscScrollGump(
                            $"A haunted message from a soul called {_protagonist}",
                            _content,
                            mediumship.ImageID
                        )
                    );
                }
            }

        }

        private void Deserialize(IGenericReader reader, int version)
        {
            base.Deserialize(reader);

            _chapterLocation = reader.ReadPoint2D();
            _hookNumber = reader.ReadInt();
            _content = new string[reader.ReadInt()];

            for (var i = 0; i < _content.Length; ++i)
            {
                _content[i] = reader.ReadString();
            }
            _mapKey = reader.ReadString();
        }
    }
}
