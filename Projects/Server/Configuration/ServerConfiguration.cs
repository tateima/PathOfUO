/*************************************************************************
 * ModernUO                                                              *
 * Copyright 2019-2020 - ModernUO Development Team                       *
 * Email: hi@modernuo.com                                                *
 * File: ServerConfiguration.cs                                          *
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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Server.Json;
using Server.Logging;

namespace Server
{
    public static class ServerConfiguration
    {
        private static readonly ILogger logger = LogFactory.GetLogger(typeof(ServerConfiguration));

        private const string m_RelPath = "Configuration/modernuo.json";
        private static readonly string m_FilePath = Path.Join(Core.BaseDirectory, m_RelPath);
        private static ServerSettings m_Settings;
        private static bool m_Mocked;

        public static List<string> DataDirectories => m_Settings.DataDirectories;

        public static List<IPEndPoint> Listeners => m_Settings.Listeners;

        public static string GetSetting(string key, string defaultValue)
        {
            m_Settings.Settings.TryGetValue(key, out var value);
            return value ?? defaultValue;
        }

        public static int GetSetting(string key, int defaultValue)
        {
            m_Settings.Settings.TryGetValue(key, out var strValue);
            return int.TryParse(strValue, out var value) ? value : defaultValue;
        }

        public static long GetSetting(string key, long defaultValue)
        {
            m_Settings.Settings.TryGetValue(key, out var strValue);
            return long.TryParse(strValue, out var value) ? value : defaultValue;
        }

        public static bool GetSetting(string key, bool defaultValue)
        {
            m_Settings.Settings.TryGetValue(key, out var strValue);
            return bool.TryParse(strValue, out var value) ? value : defaultValue;
        }

        public static T GetSetting<T>(string key, T defaultValue) where T : struct, Enum
        {
            m_Settings.Settings.TryGetValue(key, out var strValue);
            return Enum.TryParse(strValue, out T value) ? value : defaultValue;
        }

        public static T? GetSetting<T>(string key) where T : struct, Enum
        {
            if (!m_Settings.Settings.TryGetValue(key, out var strValue))
            {
                return null;
            }

            return Enum.TryParse(strValue, out T value) ? value : null;
        }

        public static string GetOrUpdateSetting(string key, string defaultValue)
        {
            if (m_Settings.Settings.TryGetValue(key, out var value))
            {
                return value;
            }

            SetSetting(key, value = defaultValue);
            return value;
        }

        public static int GetOrUpdateSetting(string key, int defaultValue)
        {
            int value;

            if (m_Settings.Settings.TryGetValue(key, out var strValue))
            {
                value = int.TryParse(strValue, out value) ? value : defaultValue;
            }
            else
            {
                SetSetting(key, (value = defaultValue).ToString());
            }

            return value;
        }

        public static long GetOrUpdateSetting(string key, long defaultValue)
        {
            long value;

            if (m_Settings.Settings.TryGetValue(key, out var strValue))
            {
                value = long.TryParse(strValue, out value) ? value : defaultValue;
            }
            else
            {
                SetSetting(key, (value = defaultValue).ToString());
            }

            return value;
        }

        public static bool GetOrUpdateSetting(string key, bool defaultValue)
        {
            bool value;

            if (m_Settings.Settings.TryGetValue(key, out var strValue))
            {
                value = bool.TryParse(strValue, out value) ? value : defaultValue;
            }
            else
            {
                SetSetting(key, (value = defaultValue).ToString());
            }

            return value;
        }

        public static TimeSpan GetOrUpdateSetting(string key, TimeSpan defaultValue)
        {
            TimeSpan value;

            if (m_Settings.Settings.TryGetValue(key, out var strValue))
            {
                value = TimeSpan.TryParse(strValue, out value) ? value : defaultValue;
            }
            else
            {
                SetSetting(key, (value = defaultValue).ToString());
            }

            return value;
        }

        public static T GetOrUpdateSetting<T>(string key, T defaultValue) where T : struct, Enum
        {
            T value;

            if (m_Settings.Settings.TryGetValue(key, out var strValue))
            {
                value = Enum.TryParse(strValue, out value) ? value : defaultValue;
            }
            else
            {
                SetSetting(key, (value = defaultValue).ToString());
            }

            return value;
        }

        public static void SetSetting(string key, string value)
        {
            m_Settings.Settings[key] = value;
            Save();
        }

        // If mock is enabled we skip the console readline.
        public static void Load(bool mocked = false)
        {
            m_Mocked = mocked;
            var updated = false;

            if (File.Exists(m_FilePath))
            {
                logger.Information($"Reading server configuration from {m_RelPath}...");
                m_Settings = JsonConfig.Deserialize<ServerSettings>(m_FilePath);

                if (m_Settings == null)
                {
                    logger.Error($"Reading server configuration failed");
                    throw new FileNotFoundException($"Failed to deserialize {m_FilePath}.");
                }

                logger.Information($"Reading server configuration done");
            }
            else
            {
                updated = true;
                m_Settings = new ServerSettings();
            }

            if (mocked)
            {
                return;
            }

            if (m_Settings.DataDirectories.Count == 0)
            {
                updated = true;
                m_Settings.DataDirectories.AddRange(GetDataDirectories());
            }

            if (m_Settings.Listeners.Count == 0)
            {
                updated = true;
                m_Settings.Listeners.AddRange(GetListeners());
            }

            if (m_Settings.Expansion == null)
            {
                var expansion = GetSetting<Expansion>("currentExpansion");
                var hasExpansion = expansion != null;

                expansion ??= GetExpansion();

                if (expansion <= Expansion.ML && !hasExpansion)
                {
                    SetPre6000Support();
                }

                updated = true;
                m_Settings.Expansion = expansion;
            }

            Core.Expansion = m_Settings.Expansion.Value;

            if (updated)
            {
                Save();
                logger.Information($"Server configuration saved to {m_RelPath}.");
            }
        }

        private static void SetPre6000Support()
        {
            Console.WriteLine("Will you be using a client version older than 6.0.0.0?");

            do
            {
                Console.Write("y or [n]> ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) || input.InsensitiveStartsWith("n"))
                {
                    return;
                }

                if (input.InsensitiveStartsWith("y"))
                {
                    SetSetting("maps.enablePre6000Trammel", true.ToString());
                    SetSetting("maps.enableMapDiffPatches", true.ToString());
                    return;
                }

                logger.Information($"Invalid option. ({input})");
            } while (true);
        }

        private static Expansion GetExpansion()
        {
            Console.WriteLine("Please choose an expansion by typing the number or short name:");
            var expansions = ExpansionInfo.Table;

            for (int i = 0; i < expansions.Length; i++)
            {
                var info = expansions[i];
                Console.WriteLine(" - {0,2}: {1} ({2})", i, ((Expansion)info.ID).ToString(), info.Name);
            }

            var maxExpansion = (Expansion)expansions[^1].ID;
            var maxExpansionName = maxExpansion.ToString();

            do
            {
                Console.Write("[{0}]> ", maxExpansionName);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    return maxExpansion;
                }

                if (int.TryParse(input, NumberStyles.Integer, null, out var number) && number >= 0 &&
                    number < expansions.Length)
                {
                    return (Expansion)number;
                }

                if (Enum.TryParse<Expansion>(input, out var expansion))
                {
                    return expansion;
                }

                logger.Information($"Invalid expansion. ({input})");
            } while (true);
        }

        private static List<string> GetDataDirectories()
        {
            Console.WriteLine("Please enter the absolute path to the Ultima Online data:");

            var directories = new List<string>();

            do
            {
                Console.Write("{0}> ", directories.Count > 0 ? "[finish] " : " ");
                var directory = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(directory))
                {
                    break;
                }

                if (Directory.Exists(directory))
                {
                    directories.Add(directory);
                    logger.Information($"Path {directory} added.");
                }
                else
                {
                    logger.Information($"Path does not exist. ({directory})");
                }
            } while (true);

            return directories;
        }

        private static List<IPEndPoint> GetListeners()
        {
            Console.WriteLine("Please enter the IP and ports to listen:");
            Console.WriteLine(" - Only enter IP addresses directly bound to this machine");
            Console.WriteLine(" - To listen to all IP addresses enter 0.0.0.0");

            var ips = new List<IPEndPoint>();

            do
            {
                // IP:Port?
                Console.Write("[{0}]> ", ips.Count > 0 ? "finish" : "0.0.0.0:2593");
                var ipStr = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(ipStr))
                {
                    break;
                }

                if (!ipStr.ContainsOrdinal(':'))
                {
                    ipStr += ":2593";
                }

                if (IPEndPoint.TryParse(ipStr, out var ip))
                {
                    ips.Add(ip);
                    logger.Information($"Core: {ipStr} added.");
                }
                else
                {
                    logger.Information($"{ipStr} is not a valid IP or port");
                }
            } while (true);

            if (ips.Count == 0)
            {
                ips.Add(new IPEndPoint(IPAddress.Any, 2593));
            }

            return ips;
        }

        public static void Save()
        {
            if (m_Mocked)
            {
                return;
            }

            JsonConfig.Serialize(m_FilePath, m_Settings);
        }
    }
}
