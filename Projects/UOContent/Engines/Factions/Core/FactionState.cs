using System;
using System.Collections.Generic;

namespace Server.Factions
{
    public class FactionState
    {
        private const int BroadcastsPerPeriod = 2;
        private static readonly TimeSpan BroadcastPeriod = TimeSpan.FromHours(1.0);
        private readonly Faction m_Faction;

        private readonly DateTime[] m_LastBroadcasts = new DateTime[BroadcastsPerPeriod];
        private Mobile m_Commander;

        public FactionState(Faction faction)
        {
            m_Faction = faction;
            Tithe = 50;
            Members = new List<PlayerState>();
            Election = new Election(faction);
            FactionItems = new List<FactionItem>();
            Traps = new List<BaseFactionTrap>();
        }

        public FactionState(IGenericReader reader)
        {
            var version = reader.ReadEncodedInt();

            switch (version)
            {
                case 5:
                    {
                        LastAtrophy = reader.ReadDateTime();
                        goto case 4;
                    }
                case 4:
                    {
                        var count = reader.ReadEncodedInt();

                        for (var i = 0; i < count; ++i)
                        {
                            var time = reader.ReadDateTime();

                            if (i < m_LastBroadcasts.Length)
                            {
                                m_LastBroadcasts[i] = time;
                            }
                        }

                        goto case 3;
                    }
                case 3:
                case 2:
                case 1:
                    {
                        Election = new Election(reader);

                        goto case 0;
                    }
                case 0:
                    {
                        m_Faction = Faction.ReadReference(reader);

                        m_Commander = reader.ReadEntity<Mobile>();

                        if (version < 5)
                        {
                            LastAtrophy = Core.Now;
                        }

                        if (version < 4)
                        {
                            var time = reader.ReadDateTime();

                            if (m_LastBroadcasts.Length > 0)
                            {
                                m_LastBroadcasts[0] = time;
                            }
                        }

                        Tithe = reader.ReadEncodedInt();
                        Silver = reader.ReadEncodedInt();

                        var memberCount = reader.ReadEncodedInt();

                        Members = new List<PlayerState>();

                        for (var i = 0; i < memberCount; ++i)
                        {
                            var pl = new PlayerState(reader, m_Faction, Members);

                            if (pl.Mobile != null)
                            {
                                Members.Add(pl);
                            }
                        }

                        m_Faction.State = this;

                        m_Faction.ZeroRankOffset = Members.Count;
                        Members.Sort();

                        for (var i = Members.Count - 1; i >= 0; i--)
                        {
                            var player = Members[i];

                            if (player.KillPoints <= 0)
                            {
                                m_Faction.ZeroRankOffset = i;
                            }
                            else
                            {
                                player.RankIndex = i;
                            }
                        }

                        FactionItems = new List<FactionItem>();

                        if (version >= 2)
                        {
                            var factionItemCount = reader.ReadEncodedInt();

                            for (var i = 0; i < factionItemCount; ++i)
                            {
                                var factionItem = new FactionItem(reader, m_Faction);

                                Timer.StartTimer(factionItem.CheckAttach); // sandbox attachment
                            }
                        }

                        Traps = new List<BaseFactionTrap>();

                        if (version >= 3)
                        {
                            var factionTrapCount = reader.ReadEncodedInt();

                            for (var i = 0; i < factionTrapCount; ++i)
                            {
                                if (reader.ReadEntity<Item>() is BaseFactionTrap trap && !trap.CheckDecay())
                                {
                                    Traps.Add(trap);
                                }
                            }
                        }

                        break;
                    }
            }

            if (version < 1)
            {
                Election = new Election(m_Faction);
            }
        }

        public DateTime LastAtrophy { get; set; }

        public bool FactionMessageReady
        {
            get
            {
                for (var i = 0; i < m_LastBroadcasts.Length; ++i)
                {
                    if (Core.Now >= m_LastBroadcasts[i] + BroadcastPeriod)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsAtrophyReady => Core.Now >= LastAtrophy + TimeSpan.FromHours(47.0);

        public List<FactionItem> FactionItems { get; set; }

        public List<BaseFactionTrap> Traps { get; set; }

        public Election Election { get; set; }

        public Mobile Commander
        {
            get => m_Commander;
            set
            {
                m_Commander?.InvalidateProperties();

                m_Commander = value;

                if (m_Commander != null)
                {
                    m_Commander.SendLocalizedMessage(1042227); // You have been elected Commander of your faction

                    m_Commander.InvalidateProperties();

                    var pl = PlayerState.Find(m_Commander);

                    if (pl?.Finance != null)
                    {
                        pl.Finance.Finance = null;
                    }

                    if (pl?.Sheriff != null)
                    {
                        pl.Sheriff.Sheriff = null;
                    }
                }
            }
        }

        public int Tithe { get; set; }

        public int Silver { get; set; }

        public List<PlayerState> Members { get; set; }

        public int CheckAtrophy()
        {
            if (Core.Now < LastAtrophy + TimeSpan.FromHours(47.0))
            {
                return 0;
            }

            var distrib = 0;
            LastAtrophy = Core.Now;

            var members = new List<PlayerState>(Members);

            for (var i = 0; i < members.Count; ++i)
            {
                var ps = members[i];

                if (ps.IsActive)
                {
                    ps.IsActive = false;
                    continue;
                }

                if (ps.KillPoints > 0)
                {
                    var atrophy = (ps.KillPoints + 9) / 10;
                    ps.KillPoints -= atrophy;
                    distrib += atrophy;
                }
            }

            return distrib;
        }

        public void RegisterBroadcast()
        {
            for (var i = 0; i < m_LastBroadcasts.Length; ++i)
            {
                if (Core.Now >= m_LastBroadcasts[i] + BroadcastPeriod)
                {
                    m_LastBroadcasts[i] = Core.Now;
                    break;
                }
            }
        }

        public void Serialize(IGenericWriter writer)
        {
            writer.WriteEncodedInt(5); // version

            writer.Write(LastAtrophy);

            writer.WriteEncodedInt(m_LastBroadcasts.Length);

            for (var i = 0; i < m_LastBroadcasts.Length; ++i)
            {
                writer.Write(m_LastBroadcasts[i]);
            }

            Election.Serialize(writer);

            Faction.WriteReference(writer, m_Faction);

            writer.Write(m_Commander);

            writer.WriteEncodedInt(Tithe);
            writer.WriteEncodedInt(Silver);

            writer.WriteEncodedInt(Members.Count);

            for (var i = 0; i < Members.Count; ++i)
            {
                var pl = Members[i];

                pl.Serialize(writer);
            }

            writer.WriteEncodedInt(FactionItems.Count);

            for (var i = 0; i < FactionItems.Count; ++i)
            {
                FactionItems[i].Serialize(writer);
            }

            writer.WriteEncodedInt(Traps.Count);

            for (var i = 0; i < Traps.Count; ++i)
            {
                writer.Write(Traps[i]);
            }
        }
    }
}
