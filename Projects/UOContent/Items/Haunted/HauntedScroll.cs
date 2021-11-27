using System;
using System.Collections.Generic;
using Server.Gumps;

namespace Server.Items
{
    [Serializable(0, false)]
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
        public HauntedScroll() : base (0x46B2)
        {
            _content = Array.Empty<string>();
            Name = "a britannian haunted scroll";
            _hookNumber = 1;            
        }

        [Constructible]
        public HauntedScroll(string protagonist, List<string> hooks, string mapKey) : base(0x46B2)
        {
            switch(Utility.Random(12))
            {
                case 1:
                    ItemID = 0x0E35;
                    break;
                case 2:
                    ItemID = 0x0E36;
                    break;
                case 3:
                    ItemID = 0x0E37;
                    break;
                case 4:
                    ItemID = 0x0E38;
                    break;
                case 5:
                    ItemID = 0x0E39;
                    break;
                case 6:
                    ItemID = 0x0E3A;
                    break;
                case 7:
                    ItemID = 0x0EF4;
                    break;
                case 8:
                    ItemID = 0x0EF5;
                    break;
                case 9:
                    ItemID = 0x0EF6;
                    break;
                case 10:
                    ItemID = 0x0EF7;
                    break;
                case 11:
                    ItemID = 0x0EF8;
                    break;
                case 12:
                    ItemID = 0x0EF9;
                    break;
            }

            string mapContext = (mapKey == "trammel_") ? "britannian" : mapKey.Replace("_", "");
            _content = hooks.ToArray();
            _protagonist = protagonist;
            _mapKey = mapKey;
            Name = string.Format("a {0} haunted scroll", mapContext);
            _hookNumber = 1;
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new MiscScrollGump(from, string.Format("A haunted message from a soul called {0}", _protagonist), _content));
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
