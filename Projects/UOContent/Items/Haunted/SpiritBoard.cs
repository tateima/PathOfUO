using Server.Talent;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class SpiritBoard : Item
    {
        [Constructible]
        public SpiritBoard() : base(0x0FAA) {
            int random = Utility.Random(3);
            if (random == 1) {
                Hue = 0x96D;
                Name = "a golden spirit board";
            }
            if (random == 2) {
                Hue = 0x97A;
                Name = "an agapite spirit board";
            } 
            if (random == 3) {
                Hue = 0x961;
                Name = "a silver spirit board";
            }         
            Weight = 2.0;
        } 

        public SpiritBoard(Serial serial) : base(serial)
        {
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
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile player) {
                string message = "";
                Mediumship mediumship = player.GetTalent(typeof(Mediumship)) as Mediumship;
                if (mediumship != null) {
                    HauntedScroll scroll = mediumship.GetPlayerScroll(from);
                    if (scroll != null) {
                        if (from.Map == Map.Trammel || from.Map == Map.Felucca) {
                            Point2D chapterLocation = scroll.ChapterLocation;
                            string temperature = "icy";
                            if (!from.InRange(chapterLocation, 75)) { 
                                Direction direction = player.GetDirectionTo(chapterLocation.X, chapterLocation.Y, false);
                                if (from.InRange(chapterLocation, 100)) {
                                    temperature = "hot";
                                } else if (from.InRange(chapterLocation, 150)) {
                                    temperature = "very warm";
                                } else if (from.InRange(chapterLocation, 200)) {
                                    temperature = "warm";
                                } else if (from.InRange(chapterLocation, 250)) {
                                    temperature = "lukewarm";
                                } else if (from.InRange(chapterLocation, 300)) {
                                    temperature = "cool";
                                } else if (from.InRange(chapterLocation, 350)) {
                                    temperature = "very cool";
                                } else if (from.InRange(chapterLocation, 400)) {
                                    temperature = "cold";
                                } else if (from.InRange(chapterLocation, 450)) {
                                    temperature = "very cold";
                                } else if (from.InRange(chapterLocation, 500)) {
                                    temperature = "slightly icy";
                                }
                                if (chapterLocation.X > 4999 && from.X < 4999) { // its in the second age area and they are not
                                    message = string.Format("* Your spirit board gives you a vision of another land with a vast inland sea and a fiery volcano, the board is {0} to the touch. *", temperature);
                                } else if (chapterLocation.X < 4999 && from.X > 4999) { // its in britannia and they are in second age
                                    message = string.Format("* Your spirit board gives you a vision of your homeland, the board is {1} to the touch. *", direction.ToString(), temperature);
                                } else {
                                    message = string.Format("* Your spirit board leads you to a {0} direction, the board is {1} to the touch. *", direction.ToString(), temperature);
                                }
                            } else {
                                message = "* You are so close to the source that the spirit board rattles furiously, it is too hard to discern the exact location *";
                            }                            
                        } else {
                            message = "* You cannot commune with the netherworld at this location *";
                        }
                    } else {
                        message = "* You cannot commune with the netherworld as you have no seanced scroll on your person *";
                    }
                } else {
                     message = "* You do not have the appropriate talent to use this item *";
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
    }
}
