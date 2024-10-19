using Server.Talent;
using Server.Mobiles;
using System.Collections.Generic;
using Server.Gumps;
using Server.Misc;

namespace Server.Items
{
    public class CaseNote : Item
    {
        public string NoteHeader { get; set; }
        public Point2D CaseLocation { get; set; }
        public List<Clue> Clues { get; set; }

        [Constructible]
        public CaseNote() : base(0x0FAA) {
            Clues = new List<Clue>();
            Name = "a case note";
            Weight = 2.0;
            ItemID = Utility.RandomMinMax(1, 12) switch
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
        }

        public CaseNote(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            const string mapKey = "trammel_";
            //if (from.Map == Map.Ilshenar) {
            //    mapKey = "ilshenar_"; // to do
            //} else if (from.Map == Map.Malas) {
            //    mapKey = "malas_"; // to do
            //} else if (from.Map == Map.TerMur) {
            //    mapKey = "termur_"; // to do
            //} else if (from.Map == Map.Tokuno) {
            //    mapKey = "tokuno_"; // to do
            //}
            if (from is PlayerMobile player) {
                string message = "* You cannot make out this case *";
                if (player.GetTalent(typeof(Detective)) is Detective detective) {
                    List<string> notes = new List<string>();
                    message = "* You open a case note from the authorities *";
                    if (Clues.Count == 0) {
                        int clueCount = Utility.RandomMinMax(4,7);
                        string town = HauntedHook.RandomHook(mapKey + "town");
                        CaseLocation = HauntedLocation.RandomLocation(town);
                        NoteHeader = $"The scene of this case is located at {town}, {CaseLocation.X.ToString()} X, {CaseLocation.Y.ToString()} Y";
                        while (Clues.Count < clueCount)
                        {
                            const int maxRange = 32;
                            const int minRange = 16;
                            Point2D clueLocation = new Point2D(CaseLocation.X + Utility.RandomMinMax(minRange, maxRange), CaseLocation.Y + Utility.RandomMinMax(minRange, maxRange));
                            if (Clues.Count > 0)
                            {
                                var previousClueLocation = Clues[^1].Location;
                                clueLocation = new Point2D(previousClueLocation.X + Utility.RandomMinMax(minRange, maxRange), previousClueLocation.Y + Utility.RandomMinMax(minRange, maxRange));
                            }
                            var landTile = from.Map.Tiles.GetLandTile(clueLocation.X, clueLocation.Y);
                            var flags = TileData.LandTable[landTile.ID & TileData.MaxLandValue].Flags;
                            var impassable = (flags & TileFlag.Impassable) != 0;
                            if (!impassable) {
                                Clue clue = new Clue
                                {
                                    Location = clueLocation
                                };
                                Clues.Add(clue);
                            }
                        }
                    }
                    notes.Add(NoteHeader);
                    for (var i = 0; i < Clues.Count; i++) {
                        if (Clues[i].Solved)
                        {
                            string itemName = SocketBonus.GetItemName(Clues[i].Item);
                            notes.Add(
                                $"Clue {(i+1).ToString()}: Item: {itemName}, Detail: {Clues[i].Detail}, Trait: {Clues[i].Trait}, Organisation: {Clues[i].Organisation}, Profession: {Clues[i].Profession}, Role: {Clues[i].Role}"
                            );
                        } else {
                            notes.Add(
                                $"Clue {(i+1).ToString()}: Item: ???????, Detail: ??????, Trait: ?????, Organisation: ?????, Profession: ?????, Role: ????"
                            );
                        }
                    }
                    from.SendGump(new MiscScrollGump("A forensic investigation commissioned by the authorities", notes.ToArray(), detective.ImageID));
                }
                from.LocalOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    false,
                    message
                );
            }
        }
        public override void GetProperties(IPropertyList list)
        {
            AddNameProperties(list);
        }


        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version > 0)
            {
                CaseLocation = reader.ReadPoint2D();
                NoteHeader = reader.ReadString();
                List<Clue> clues = new List<Clue>();
                if (Clues.Count > 0) {
                    int count = reader.ReadInt();
                    for (int i = 0; i < count; i++) {
                        Clue clue = new Clue(false)
                        {
                            Item = reader.ReadEntity<Item>(),
                            Solved = reader.ReadBool(),
                            Location = reader.ReadPoint2D(),
                            Trait = reader.ReadString(),
                            Role = reader.ReadString(),
                            Profession = reader.ReadString(),
                            Difficulty = reader.ReadInt(),
                            Detail = reader.ReadString(),
                            Organisation = reader.ReadString()
                        };
                        clues.Add(clue);
                    }
                    Clues = clues;
                }
            }
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);
            if (Clues != null)
            {
                writer.Write(1); // version
                writer.Write(CaseLocation);
                writer.Write(NoteHeader);
                writer.Write(Clues.Count);
                foreach(Clue clue in Clues) {
                    writer.Write(clue.Item);
                    writer.Write(clue.Solved);
                    writer.Write(clue.Location);
                    writer.Write(clue.Trait);
                    writer.Write(clue.Role);
                    writer.Write(clue.Profession);
                    writer.Write(clue.Difficulty);
                    writer.Write(clue.Detail);
                    writer.Write(clue.Organisation);
                }
            }
            else
            {
                writer.Write(0); // version
            }
        }
    }
}
