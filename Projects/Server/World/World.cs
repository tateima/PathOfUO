/*************************************************************************
 * ModernUO                                                              *
 * Copyright 2019-2020 - ModernUO Development Team                       *
 * Email: hi@modernuo.com                                                *
 * File: World.cs                                                        *
 *                                                                       *
 * This program is free software: you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation, either version 3 of the License, or     *
 * (at your option) any later version.                                   *
 *                                                                       *
 * You should have received a copy of the GNU General Public License     *
 * along with this program.  If not, see <http://www.gnu.org/licenses/>. *
 *************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Server.Guilds;
using Server.Logging;
using Server.Network;

namespace Server
{
    public enum WorldState
    {
        Initial,
        Loading,
        Running,
        Saving,
        WritingSave
    }

    public static class World
    {
        private static readonly ILogger logger = LogFactory.GetLogger(typeof(World));

        private static readonly ManualResetEvent m_DiskWriteHandle = new(true);
        private static readonly Dictionary<Serial, IEntity> _pendingAdd = new();
        private static readonly Dictionary<Serial, IEntity> _pendingDelete = new();
        private static readonly ConcurrentQueue<Item> _decayQueue = new();

        private static string _tempSavePath; // Path to the temporary folder for the save
        private static string _savePath; // Path to "Saves" folder

        public const bool DirtyTrackingEnabled = false;
        public const uint ItemOffset = 0x40000000;
        public const uint MaxItemSerial = 0x7FFFFFFF;
        public const uint MaxMobileSerial = ItemOffset - 1;
        private const uint _maxItems = MaxItemSerial - ItemOffset + 1;

        private static Serial _lastMobile = Serial.Zero;
        private static Serial _lastItem = (Serial)ItemOffset;
        private static Serial _lastGuild = Serial.Zero;

        public static Serial NewMobile
        {
            get
            {
                var last = _lastMobile;
                var maxMobile = (Serial)MaxMobileSerial;

                for (int i = 0; i < MaxMobileSerial; i++)
                {
                    last++;

                    if (last > maxMobile)
                    {
                        last = (Serial)1;
                    }

                    if (FindMobile(last) == null)
                    {
                        return _lastMobile = last;
                    }
                }

                OutOfMemory("No serials left to allocate for mobiles");
                return Serial.MinusOne;
            }
        }

        public static Serial NewItem
        {
            get
            {
                var last = _lastItem;

                for (int i = 0; i < _maxItems; i++)
                {
                    last++;

                    if (last > MaxItemSerial)
                    {
                        last = (Serial)ItemOffset;
                    }

                    if (FindItem(last) == null)
                    {
                        return _lastItem = last;
                    }
                }

                OutOfMemory("No serials left to allocate for items");
                return Serial.MinusOne;
            }
        }

        public static Serial NewGuild
        {
            get
            {
                while (FindGuild(_lastGuild += 1) != null)
                {
                }

                return _lastGuild;
            }
        }

        private static void OutOfMemory(string message) => throw new OutOfMemoryException(message);

        internal static List<Type> ItemTypes { get; } = new();
        internal static List<Type> MobileTypes { get; } = new();
        internal static List<Type> GuildTypes { get; } = new();

        public static WorldState WorldState { get; private set; }
        public static bool Saving => WorldState == WorldState.Saving;
        public static bool Running => WorldState != WorldState.Loading && WorldState != WorldState.Initial;
        public static bool Loading => WorldState == WorldState.Loading;

        public static Dictionary<Serial, Mobile> Mobiles { get; private set; }
        public static Dictionary<Serial, Item> Items { get; private set; }
        public static Dictionary<Serial, BaseGuild> Guilds { get; private set; }

        public static void Configure()
        {
            var tempSavePath = ServerConfiguration.GetOrUpdateSetting("world.tempSavePath", "temp");
            _tempSavePath = Path.Combine(Core.BaseDirectory, tempSavePath);
            var savePath = ServerConfiguration.GetOrUpdateSetting("world.savePath", "Saves");
            _savePath = Path.Combine(Core.BaseDirectory, savePath);

            // Mobiles & Items
            Persistence.Register("Mobiles & Items", SaveEntities, WriteEntities, LoadEntities, 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WaitForWriteCompletion()
        {
            m_DiskWriteHandle.WaitOne();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnqueueForDecay(Item item)
        {
            if (WorldState != WorldState.Saving)
            {
                logger.Warning($"Attempting to queue {item} for decay but the world is not saving");
                return;
            }

            _decayQueue.Enqueue(item);
        }

        public static void Broadcast(int hue, bool ascii, string text)
        {
            var length = OutgoingMessagePackets.GetMaxMessageLength(text);

            Span<byte> buffer = stackalloc byte[length].InitializePacket();

            foreach (var ns in TcpServer.Instances)
            {
                if (ns.Mobile == null)
                {
                    continue;
                }

                length = OutgoingMessagePackets.CreateMessage(
                    buffer, Serial.MinusOne, -1, MessageType.Regular, hue, 3, ascii, "ENU", "System", text
                );

                if (length != buffer.Length)
                {
                    buffer = buffer[..length]; // Adjust to the actual size
                }

                ns.Send(buffer);
            }

            NetState.FlushAll();
        }

        public static void BroadcastStaff(int hue, bool ascii, string text)
        {
            var length = OutgoingMessagePackets.GetMaxMessageLength(text);

            Span<byte> buffer = stackalloc byte[length].InitializePacket();

            foreach (var ns in TcpServer.Instances)
            {
                if (ns.Mobile == null || ns.Mobile.AccessLevel < AccessLevel.GameMaster)
                {
                    continue;
                }

                length = OutgoingMessagePackets.CreateMessage(
                    buffer, Serial.MinusOne, -1, MessageType.Regular, hue, 3, ascii, "ENU", "System", text
                );

                if (length != buffer.Length)
                {
                    buffer = buffer[..length]; // Adjust to the actual size
                }

                ns.Send(buffer);
            }

            NetState.FlushAll();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Broadcast(int hue, bool ascii, string format, params object[] args) =>
            Broadcast(hue, ascii, string.Format(format, args));

        internal static void LoadEntities(string basePath)
        {
            IIndexInfo<Serial> itemIndexInfo = new EntityTypeIndex("Items");
            IIndexInfo<Serial> mobileIndexInfo = new EntityTypeIndex("Mobiles");
            IIndexInfo<Serial> guildIndexInfo = new EntityTypeIndex("Guilds");

            Mobiles = EntityPersistence.LoadIndex(basePath, mobileIndexInfo, out List<EntityIndex<Mobile>> mobiles);
            Items = EntityPersistence.LoadIndex(basePath, itemIndexInfo, out List<EntityIndex<Item>> items);
            Guilds = EntityPersistence.LoadIndex(basePath, guildIndexInfo, out List<EntityIndex<BaseGuild>> guilds);

            EntityPersistence.LoadData(basePath, mobileIndexInfo, mobiles);
            EntityPersistence.LoadData(basePath, itemIndexInfo, items);
            EntityPersistence.LoadData(basePath, guildIndexInfo, guilds);
        }

        public static void Load()
        {
            if (WorldState != WorldState.Initial)
            {
                return;
            }

            WorldState = WorldState.Loading;

            logger.Information("Loading world");
            var watch = Stopwatch.StartNew();

            Persistence.Load(_savePath);
            EventSink.InvokeWorldLoad();

            ProcessSafetyQueues();

            foreach (var item in Items.Values)
            {
                if (item.Parent == null)
                {
                    item.UpdateTotals();
                }

                item.ClearProperties();
            }

            foreach (var m in Mobiles.Values)
            {
                m.UpdateRegion(); // Is this really needed?
                m.UpdateTotals();

                m.ClearProperties();
            }

            watch.Stop();

            logger.Information(string.Format("World loaded ({1} items, {2} mobiles) ({0:F2} seconds)",
                watch.Elapsed.TotalSeconds,
                Items.Count,
                Mobiles.Count
            ));

            WorldState = WorldState.Running;
        }

        private static void ProcessSafetyQueues()
        {
            foreach (var entity in _pendingAdd.Values)
            {
                AddEntity(entity);
            }

            foreach (var entity in _pendingDelete.Values)
            {
                if (_pendingAdd.ContainsKey(entity.Serial))
                {
                    logger.Warning("Entity {0} was both pending both deletion and addition after save", entity);
                }

                RemoveEntity(entity);
            }
        }

        private static void AppendSafetyLog(string action, ISerializable entity)
        {
            var message =
                $"Warning: Attempted to {action} {entity} during world save.{Environment.NewLine}This action could cause inconsistent state.{Environment.NewLine}It is strongly advised that the offending scripts be corrected.";

            logger.Information(message);

            try
            {
                using var op = new StreamWriter("world-save-errors.log", true);
                op.WriteLine("{0}\t{1}", DateTime.UtcNow, message);
                op.WriteLine(new StackTrace(2).ToString());
                op.WriteLine();
            }
            catch
            {
                // ignored
            }
        }

        private static void FinishWorldSave()
        {
            WorldState = WorldState.Running;

            ProcessDecay();
            ProcessSafetyQueues();
        }

        private static void TraceSave(params IEnumerable<KeyValuePair<string, int>>[] entityTypes)
        {
            try
            {
                int count = 0;

                var timestamp = Utility.GetTimeStamp();
                using var op = new StreamWriter("Logs/Saves/Save-Stats-{0}.log", true);

                for (var i = 0; i < entityTypes.Length; i++)
                {
                    foreach (var (t, c) in entityTypes[i])
                    {
                        op.WriteLine("{0}: {1}", t, c);
                        count++;
                    }
                }

                op.WriteLine("- Total: {0}", count);

                op.WriteLine();
                op.WriteLine();
            }
            catch
            {
                // ignored
            }
        }

        internal static void WriteEntities(string basePath)
        {
            IIndexInfo<Serial> itemIndexInfo = new EntityTypeIndex("Items");
            IIndexInfo<Serial> mobileIndexInfo = new EntityTypeIndex("Mobiles");
            IIndexInfo<Serial> guildIndexInfo = new EntityTypeIndex("Guilds");

            EntityPersistence.WriteEntities(mobileIndexInfo, Mobiles, MobileTypes, basePath, out var mobileCounts);
            EntityPersistence.WriteEntities(itemIndexInfo, Items, ItemTypes, basePath, out var itemCounts);
            EntityPersistence.WriteEntities(guildIndexInfo, Guilds, GuildTypes, basePath, out var guildCounts);

            TraceSave(mobileCounts?.ToList(), itemCounts?.ToList(), guildCounts?.ToList());
        }

        public static void WriteFiles(object state)
        {
            Exception exception = null;

            var tempPath = Path.Combine(_tempSavePath, Utility.GetTimeStamp());

            try
            {
                var watch = Stopwatch.StartNew();
                logger.Information("Writing world save snapshot");

                Persistence.WriteSnapshot(tempPath);

                watch.Stop();

                logger.Information("Writing world save snapshot done ({0:F2} seconds)", watch.Elapsed.TotalSeconds);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception != null)
            {
                logger.Error(exception, "Writing world save snapshot failed.");
                Persistence.TraceException(exception);

                BroadcastStaff(0x35, true, "Writing world save snapshot failed.");
            }
            else
            {
                try
                {
                    EventSink.InvokeWorldSavePostSnapshot(_savePath, tempPath);
                    Directory.Move(tempPath, _savePath);
                }
                catch (Exception ex)
                {
                    Persistence.TraceException(ex);
                }
            }

            m_DiskWriteHandle.Set();

            Timer.StartTimer(FinishWorldSave);
        }

        private static void ProcessDecay()
        {
            while (_decayQueue.TryDequeue(out var item))
            {
                if (item.OnDecay())
                {
                    // TODO: Add Logging
                    item.Delete();
                }
            }
        }

        private static DateTime _serializationStart;

        internal static void SaveEntities()
        {
            _serializationStart = DateTime.UtcNow;
            EntityPersistence.SaveEntities(Items.Values, SaveEntity);
            EntityPersistence.SaveEntities(Mobiles.Values, SaveEntity);
            EntityPersistence.SaveEntities(Guilds.Values, SaveEntity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SaveEntity<T>(T entity) where T : class, ISerializable
        {
            if (entity is Item item && item.CanDecay() && item.LastMoved + item.DecayTime <= _serializationStart)
            {
                EnqueueForDecay(item);
            }

            entity.Serialize();
        }

        public static void Save()
        {
            if (WorldState != WorldState.Running)
            {
                return;
            }

            WaitForWriteCompletion(); // Blocks Save until current disk flush is done.

            WorldState = WorldState.Saving;

            m_DiskWriteHandle.Reset();

            Broadcast(0x35, true, "The world is saving, please wait.");

            logger.Information("Saving world");

            var watch = Stopwatch.StartNew();

            Exception exception = null;

            try
            {
                Persistence.Serialize();
                EventSink.InvokeWorldSave();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            WorldState = WorldState.WritingSave;

            watch.Stop();

            if (exception == null)
            {
                var duration = watch.Elapsed.TotalSeconds;
                logger.Information("World save completed ({0:F2} seconds)", duration);

                // Only broadcast if it took at least 150ms
                if (duration >= 0.15)
                {
                    Broadcast(0x35, true, $"World Save completed in {duration:F2} seconds.");
                }
            }
            else
            {
                logger.Error(exception, "World save failed");
                Persistence.TraceException(exception);

                BroadcastStaff(0x35, true, "World save failed.");
            }

            ThreadPool.QueueUserWorkItem(WriteFiles);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEntity FindEntity(Serial serial, bool returnDeleted = false) => FindEntity<IEntity>(serial);

        public static T FindEntity<T>(Serial serial, bool returnDeleted = false) where T : class, IEntity
        {
            switch (WorldState)
            {
                default: return default;
                case WorldState.Loading:
                case WorldState.Saving:
                case WorldState.WritingSave:
                    {
                        if (_pendingDelete.TryGetValue(serial, out var entity))
                        {
                            return !returnDeleted ? null : entity as T;
                        }

                        if (_pendingAdd.TryGetValue(serial, out entity))
                        {
                            return entity as T;
                        }

                        goto case WorldState.Running;
                    }
                case WorldState.Running:
                    {
                        if (serial.IsItem)
                        {
                            Items.TryGetValue(serial, out var item);
                            return item as T;
                        }

                        if (serial.IsMobile)
                        {
                            Mobiles.TryGetValue(serial, out var mob);
                            return mob as T;
                        }

                        return default;
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Item FindItem(Serial serial, bool returnDeleted = false) => FindEntity<Item>(serial, returnDeleted);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mobile FindMobile(Serial serial, bool returnDeleted = false) =>
            FindEntity<Mobile>(serial, returnDeleted);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BaseGuild FindGuild(Serial serial) => Guilds.TryGetValue(serial, out var guild) ? guild : null;

        public static void AddEntity<T>(T entity) where T : class, IEntity
        {
            switch (WorldState)
            {
                default: // Not Running
                    {
                        throw new Exception($"Added {entity.GetType().Name} before world load.\n");
                    }
                case WorldState.Saving:
                    {
                        AppendSafetyLog("add", entity);
                        goto case WorldState.WritingSave;
                    }
                case WorldState.Loading:
                case WorldState.WritingSave:
                    {
                        if (_pendingDelete.Remove(entity.Serial))
                        {
                            logger.Warning($"Deleted then added {entity.GetType().Name} during {WorldState.ToString()} state.");
                        }
                        _pendingAdd[entity.Serial] = entity;
                        break;
                    }
                case WorldState.Running:
                    {
                        if (entity.Serial.IsItem)
                        {
                            Items[entity.Serial] = entity as Item;
                        }

                        if (entity.Serial.IsMobile)
                        {
                            Mobiles[entity.Serial] = entity as Mobile;
                        }
                        break;
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddGuild(BaseGuild guild) => Guilds[guild.Serial] = guild;

        public static void RemoveEntity<T>(T entity) where T : class, IEntity
        {
            switch (WorldState)
            {
                default: // Not Running
                    {
                        throw new Exception($"Removed {entity.GetType().Name} before world load.\n");
                    }
                case WorldState.Saving:
                    {
                        AppendSafetyLog("delete", entity);
                        goto case WorldState.WritingSave;
                    }
                case WorldState.Loading:
                case WorldState.WritingSave:
                    {
                        _pendingAdd.Remove(entity.Serial);
                        _pendingDelete[entity.Serial] = entity;
                        break;
                    }
                case WorldState.Running:
                    {
                        if (entity.Serial.IsItem)
                        {
                            Items.Remove(entity.Serial);
                        }

                        if (entity.Serial.IsMobile)
                        {
                            Mobiles.Remove(entity.Serial);
                        }
                        break;
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveGuild(BaseGuild guild) => Guilds.Remove(guild.Serial);

        public static T ReadEntity<T>(this IGenericReader reader) where T : class, ISerializable
        {
            Serial serial = reader.ReadSerial();
            var typeT = typeof(T);

            // Add to this list when creating new serializable types
            if (typeof(BaseGuild).IsAssignableTo(typeT))
            {
                return FindGuild(serial) as T;
            }

            return FindEntity<IEntity>(serial) as T;
        }

        public static List<T> ReadEntityList<T>(this IGenericReader reader) where T : class, ISerializable
        {
            var count = reader.ReadInt();

            var list = new List<T>(count);

            for (var i = 0; i < count; ++i)
            {
                var entity = reader.ReadEntity<T>();
                if (entity != null)
                {
                    list.Add(entity);
                }
            }

            return list;
        }

        public static HashSet<T> ReadEntitySet<T>(this IGenericReader reader) where T : class, ISerializable
        {
            var count = reader.ReadInt();

            var set = new HashSet<T>(count);

            for (var i = 0; i < count; ++i)
            {
                var entity = reader.ReadEntity<T>();
                if (entity != null)
                {
                    set.Add(entity);
                }
            }

            return set;
        }

        public static void Write(this IGenericWriter writer, ISerializable value)
        {
            writer.Write(value?.Deleted != false ? Serial.MinusOne : value.Serial);
        }

        public static void Write<T>(this IGenericWriter writer, ICollection<T> coll) where T : class, ISerializable
        {
            writer.Write(coll.Count);
            foreach (var entry in coll)
            {
                writer.Write(entry);
            }
        }

        public static void Write<T>(
            this IGenericWriter writer, ICollection<T> coll, Action<IGenericWriter, T> action
        ) where T : class, ISerializable
        {
            if (coll == null)
            {
                writer.Write(0);
                return;
            }

            writer.Write(coll.Count);
            foreach (var entry in coll)
            {
                action(writer, entry);
            }
        }
    }
}
