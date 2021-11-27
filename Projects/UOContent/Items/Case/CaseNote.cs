using Server.Talent;
using Server.Network;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using Server.Gumps;

namespace Server.Items
{
    public class CaseNote : Item
    {
        private Point2D m_CaseLocation;
        public Point2D CaseLocation {
            get { return m_CaseLocation; }
            set { m_CaseLocation = value; }
        }
        private List<Clue> m_Clues;
        public List<Clue> Clues {
            get { return m_Clues; }
            set { m_Clues = value; }
        }

        [Constructible]
        public CaseNote() : base(0x0FAA) {
            m_Clues = new List<Clue>();
            Name = "a case note";    
            Weight = 2.0;
            switch(Utility.RandomMinMax(1, 12))
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
        } 

        public CaseNote(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            string mapKey = "trammel_";
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
                Detective detective = player.GetTalent(typeof(Detective)) as Detective;
                if (detective != null) {  
                    List<string> notes = new List<string>();
                    List<int> imageIds = new List<int>();
                    message = "* You open a case note from the authorities *";
                    if (m_Clues.Count == 0) {
                        int clueCount = Utility.RandomMinMax(4,7);
                        string town = HauntedHook.RandomHook(mapKey + "town");
                        m_CaseLocation = HauntedLocation.RandomLocation(mapKey + town);
                        notes.Add(string.Format("The scene of this case is located at {0}, {1} X, {2} Y", town, m_CaseLocation.X.ToString(), m_CaseLocation.Y.ToString()));
                        while (m_Clues.Count < clueCount) {
                            Point2D clueLocation = new Point2D(m_CaseLocation.X + Utility.Random(8), m_CaseLocation.Y + Utility.Random(8));
                            var landTile = from.Map.Tiles.GetLandTile(clueLocation.X, clueLocation.Y);
                            var flags = TileData.LandTable[landTile.ID & TileData.MaxLandValue].Flags;
                            var impassable = (flags & TileFlag.Impassable) != 0;
                            if (!impassable) {
                                Clue clue = new Clue();
                                clue.Location = clueLocation;
                                m_Clues.Add(clue);
                            }
                        }
                    } 
                    for (var i = 0; i <= m_Clues.Count; i++) {
                        if (m_Clues[i].Solved) {
                            notes.Add(string.Format("Clue {0}: {1}, Trait: {2}, Organisation: {3}, Profession: {4}, Role: {5}", i.ToString(), m_Clues[i].Detail, m_Clues[i].Trait, m_Clues[i].Organisation, m_Clues[i].Profession, m_Clues[i].Role));
                            imageIds.Add(m_Clues[i].Item.ItemID);
                        } else {
                            notes.Add(string.Format("Clue {0}: ??????, Trait: ?????, Organisation: ?????, Profession: ?????, Role: ????", i.ToString()));
                            imageIds.Add(0);
                        }
                    }
                    from.SendGump(new MiscScrollGump(from, "A forensic investigation commissioned by the authorities", notes.ToArray(), imageIds.ToArray()));
                }
                from.LocalOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    false,
                    message
                );
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            AddNameProperties(list);
        }

        
        public void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_CaseLocation = reader.ReadPoint2D();
            List<Clue> clues = new List<Clue>();
            if (version >= 0) {
                int count = reader.ReadInt();
                for (int i = 0; i < count; i++) {
                    Clue clue = new Clue(false);
                    clue.Item = reader.ReadEntity<Item>();
                    clue.Solved = reader.ReadBool();
                    clue.Location = reader.ReadPoint2D();
                    clue.Trait = reader.ReadString();
                    clue.Role = reader.ReadString();
                    clue.Profession = reader.ReadString();
                    clue.Difficulty = reader.ReadInt();
                    clue.Detail = reader.ReadString();
                    clues.Add(clue);
                }
                m_Clues = clues;
            }
        }

        public void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_CaseLocation);
            writer.Write(m_Clues.Count);
            foreach(Clue clue in m_Clues) {
                writer.Write(clue.Item);
                writer.Write(clue.Solved);
                writer.Write(clue.Location);
                writer.Write(clue.Trait);
                writer.Write(clue.Role);
                writer.Write(clue.Profession);
                writer.Write(clue.Difficulty);
                writer.Write(clue.Detail);
            }
        }
    }
}
