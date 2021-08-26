using System;
using Server.Network;

namespace Server.Items
{
    public class PlagueBeastBlood : PlagueBeastComponent
    {
        public PlagueBeastBlood() : base(0x122C, 0)
        {
            Timer.StartTimer(
                TimeSpan.FromSeconds(1.5),
                TimeSpan.FromSeconds(1.5),
                3,
                Hemorrhage
            );
        }

        public PlagueBeastBlood(Serial serial) : base(serial)
        {
        }

        public bool Patched => ItemID == 0x1765;

        public bool Starting => ItemID == 0x122C;

        public override bool OnBandage(Mobile from)
        {
            if (!IsAccessibleTo(from) || Patched)
            {
                return false;
            }

            if (Starting)
            {
                X += 2;
                Y -= 9;

                switch (Organ)
                {
                    case PlagueBeastRubbleOrgan:
                        Y -= 5;
                        break;
                    case PlagueBeastBackupOrgan:
                        X += 7;
                        break;
                }
            }
            else
            {
                X -= 4;
                Y -= 2;
            }

            ItemID = 0x1765;

            var pack = Owner?.Backpack;

            if (pack != null)
            {
                for (var i = 0; i < pack.Items.Count; i++)
                {
                    if (pack.Items[i] is PlagueBeastMainOrgan main && main.Complete)
                    {
                        main.FinishOpening(from);
                    }
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, 1071916); // * You patch up the wound with a bandage *

            return true;
        }

        private void Hemorrhage()
        {
            if (Deleted || Patched)
            {
                return;
            }

            Owner?.PlaySound(0x25);

            if (ItemID == 0x122A)
            {
                if (Owner != null)
                {
                    Owner.Unfreeze();
                    Owner.Kill();
                }
            }
            else
            {
                if (Starting)
                {
                    X += 8;
                    Y -= 10;
                }

                ItemID--;
            }
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadEncodedInt();
        }
    }
}
