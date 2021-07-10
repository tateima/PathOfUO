/*************************************************************************
 * ModernUO                                                              *
 * Copyright 2019-2020 - ModernUO Development Team                       *
 * Email: hi@modernuo.com                                                *
 * File: TcpServer.cs                                                    *
 *                                                                       *
 * This program is free software: you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation, either version 3 of the License, or     *
 * (at your option) any later version.                                   *
 *                                                                       *
 * You should have received a copy of the GNU General Public License     *
 * along with this program.  If not, see <http://www.gnu.org/licenses/>. *
 *************************************************************************/

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Server.Logging;

namespace Server.Network
{
    public static class TcpServer
    {
        private static readonly ILogger logger = LogFactory.GetLogger(typeof(TcpServer));

        private const int MaxConnectionsPerLoop = 250;

        // Sanity. 256 * 1024 * 4096 = ~1.3GB of ram
        public static int MaxConnections { get; set; } = 4096;

        private const long _listenerErrorMessageDelay = 10000; // 10 seconds
        private static long _nextMaximumSocketsReachedMessage;

        // AccountLoginReject BadComm
        private static readonly byte[] _socketRejected = { 0x82, 0xFF };

        public static IPEndPoint[] ListeningAddresses { get; private set; }
        public static TcpListener[] Listeners { get; private set; }
        public static HashSet<NetState> Instances { get; } = new(2048);

        private static readonly ConcurrentQueue<NetState> _connectedQueue = new();

        public static void Configure()
        {
            MaxConnections = ServerConfiguration.GetOrUpdateSetting("tcpServer.maxConnections", MaxConnections);
        }

        public static void Start()
        {
            HashSet<IPEndPoint> listeningAddresses = new HashSet<IPEndPoint>();
            List<TcpListener> listeners = new List<TcpListener>();

            foreach (var ipep in ServerConfiguration.Listeners)
            {
                var listener = CreateListener(ipep);
                if (listener == null)
                {
                    continue;
                }

                if (ipep.Address.Equals(IPAddress.Any) || ipep.Address.Equals(IPAddress.IPv6Any))
                {
                    listeningAddresses.UnionWith(GetListeningAddresses(ipep));
                }
                else
                {
                    listeningAddresses.Add(ipep);
                }

                listeners.Add(listener);
                listener.BeginAcceptingSockets();
            }

            foreach (var ipep in listeningAddresses)
            {
                logger.Information("Listening: {0}:{1}", ipep.Address, ipep.Port);
            }

            ListeningAddresses = listeningAddresses.ToArray();
            Listeners = listeners.ToArray();
        }

        public static IEnumerable<IPEndPoint> GetListeningAddresses(IPEndPoint ipep) =>
            NetworkInterface.GetAllNetworkInterfaces().SelectMany(adapter =>
                adapter.GetIPProperties().UnicastAddresses
                    .Where(uip => ipep.AddressFamily == uip.Address.AddressFamily)
                    .Select(uip => new IPEndPoint(uip.Address, ipep.Port))
            );

        public static TcpListener CreateListener(IPEndPoint ipep)
        {
            var listener = new TcpListener(ipep);
            listener.Server.ExclusiveAddressUse = false;

            try
            {
                listener.Start(8);
                return listener;
            }
            catch (SocketException se)
            {
                // WSAEADDRINUSE
                if (se.ErrorCode == 10048)
                {
                    logger.Warning("Listener: {0}:{1}: Failed (In Use)", ipep.Address, ipep.Port);
                }
                // WSAEADDRNOTAVAIL
                else if (se.ErrorCode == 10049)
                {
                    logger.Warning("Listener {0}:{1}: Failed (Unavailable)", ipep.Address, ipep.Port);
                }
                else
                {
                    logger.Warning(se, "Listener Exception:");
                }
            }

            return null;
        }

        public static int Slice()
        {
            int count = 0;

            while (++count <= MaxConnectionsPerLoop && _connectedQueue.TryDequeue(out var ns))
            {
                Instances.Add(ns);
                ns.LogInfo("Connected. [{0} Online]", Instances.Count);
                ns.Start();
            }

            return count;
        }

        private static async void BeginAcceptingSockets(this TcpListener listener)
        {
            while (true)
            {
                try
                {
                    var socket = await listener.AcceptSocketAsync();
                    var rejected = false;
                    if (Instances.Count >= MaxConnections)
                    {
                        rejected = true;

                        var ticks = Core.TickCount;

                        if (_nextMaximumSocketsReachedMessage <= ticks)
                        {
                            if (socket.RemoteEndPoint is IPEndPoint ipep)
                            {
                                var ip = ipep.Address.ToString();
                                logger.Warning("Listener {0}: Failed (Maximum connections reached)", ip);
                                NetState.TraceDisconnect("Maximum connections reached.", ip);
                            }

                            _nextMaximumSocketsReachedMessage = ticks + _listenerErrorMessageDelay;
                        }
                    }

                    var args = new SocketConnectEventArgs(socket);
                    EventSink.InvokeSocketConnect(args);

                    if (!args.AllowConnection)
                    {
                        rejected = true;
                        if (socket.RemoteEndPoint is IPEndPoint ipep)
                        {
                            var ip = ipep.Address.ToString();
                            NetState.TraceDisconnect("Rejected by socket event handler", ip);
                        }
                    }

                    if (rejected)
                    {
                        socket.Send(_socketRejected, SocketFlags.None);
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                    }
                    else
                    {
                        var ns = new NetState(new NetworkSocket(socket));
                        _connectedQueue.Enqueue(ns);
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
