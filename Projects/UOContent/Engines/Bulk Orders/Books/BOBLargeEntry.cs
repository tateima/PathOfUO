namespace Server.Engines.BulkOrders
{
    public class BOBLargeEntry : IBOBEntry
    {
        public BOBLargeEntry(LargeBOD bod)
        {
            RequireExceptional = bod.RequireExceptional;

            DeedType = bod switch
            {
                LargeTailorBOD => BODType.Tailor,
                LargeSmithBOD  => BODType.Smith,
                _              => DeedType
            };

            Material = bod.Material;
            AmountMax = bod.AmountMax;

            Entries = new BOBLargeSubEntry[bod.Entries.Length];

            for (var i = 0; i < Entries.Length; ++i)
            {
                Entries[i] = new BOBLargeSubEntry(bod.Entries[i]);
            }
        }

        public BOBLargeEntry(IGenericReader reader)
        {
            var version = reader.ReadEncodedInt();

            switch (version)
            {
                case 0:
                    {
                        RequireExceptional = reader.ReadBool();

                        DeedType = (BODType)reader.ReadEncodedInt();

                        Material = (BulkMaterialType)reader.ReadEncodedInt();
                        AmountMax = reader.ReadEncodedInt();
                        Price = reader.ReadEncodedInt();

                        Entries = new BOBLargeSubEntry[reader.ReadEncodedInt()];

                        for (var i = 0; i < Entries.Length; ++i)
                        {
                            Entries[i] = new BOBLargeSubEntry(reader);
                        }

                        break;
                    }
            }
        }

        public BOBLargeSubEntry[] Entries { get; }

        public bool RequireExceptional { get; }

        public BODType DeedType { get; }

        public BulkMaterialType Material { get; }

        public int AmountMax { get; }

        public int Price { get; set; }

        public Item Reconstruct()
        {
            LargeBOD bod = DeedType switch
            {
                BODType.Smith  => new LargeSmithBOD(AmountMax, RequireExceptional, Material, ReconstructEntries()),
                BODType.Tailor => new LargeTailorBOD(AmountMax, RequireExceptional, Material, ReconstructEntries()),
                _              => null
            };

            for (var i = 0; i < bod?.Entries.Length; ++i)
            {
                bod.Entries[i].Owner = bod;
            }

            return bod;
        }

        private LargeBulkEntry[] ReconstructEntries()
        {
            var entries = new LargeBulkEntry[Entries.Length];

            for (var i = 0; i < Entries.Length; ++i)
            {
                entries[i] = new LargeBulkEntry(
                        null,
                        new SmallBulkEntry(Entries[i].ItemType, Entries[i].Number, Entries[i].Graphic)
                    )
                    { Amount = Entries[i].AmountCur };
            }

            return entries;
        }

        public void Serialize(IGenericWriter writer)
        {
            writer.WriteEncodedInt(0); // version

            writer.Write(RequireExceptional);

            writer.WriteEncodedInt((int)DeedType);
            writer.WriteEncodedInt((int)Material);
            writer.WriteEncodedInt(AmountMax);
            writer.WriteEncodedInt(Price);

            writer.WriteEncodedInt(Entries.Length);

            for (var i = 0; i < Entries.Length; ++i)
            {
                Entries[i].Serialize(writer);
            }
        }
    }
}
