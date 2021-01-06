/*************************************************************************
 * ModernUO                                                              *
 * Copyright (C) 2019-2021 - ModernUO Development Team                   *
 * Email: hi@modernuo.com                                                *
 * File: GumpHtmlLocalized.cs                                            *
 *                                                                       *
 * This program is free software: you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation, either version 3 of the License, or     *
 * (at your option) any later version.                                   *
 *                                                                       *
 * You should have received a copy of the GNU General Public License     *
 * along with this program.  If not, see <http://www.gnu.org/licenses/>. *
 *************************************************************************/

using System.Buffers;
using Server.Collections;

namespace Server.Gumps
{
    public enum GumpHtmlLocalizedType
    {
        Plain,
        Color,
        Args
    }

    public class GumpHtmlLocalized : GumpEntry
    {
        public static readonly byte[] LayoutNamePlain = Gump.StringToBuffer("xmfhtmlgump");
        public static readonly byte[] LayoutNameColor = Gump.StringToBuffer("xmfhtmlgumpcolor");
        public static readonly byte[] LayoutNameArgs = Gump.StringToBuffer("xmfhtmltok");

        public GumpHtmlLocalized(
            int x, int y, int width, int height, int number,
            bool background = false, bool scrollbar = false
        )
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Number = number;
            Background = background;
            Scrollbar = scrollbar;

            Type = GumpHtmlLocalizedType.Plain;
        }

        public GumpHtmlLocalized(
            int x, int y, int width, int height, int number, int color,
            bool background = false, bool scrollbar = false
        )
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Number = number;
            Color = color;
            Background = background;
            Scrollbar = scrollbar;

            Type = GumpHtmlLocalizedType.Color;
        }

        public GumpHtmlLocalized(
            int x, int y, int width, int height, int number, string args, int color,
            bool background = false, bool scrollbar = false
        )
        {
            // Are multiple arguments unsupported? And what about non ASCII arguments?

            X = x;
            Y = y;
            Width = width;
            Height = height;
            Number = number;
            Args = args;
            Color = color;
            Background = background;
            Scrollbar = scrollbar;

            Type = GumpHtmlLocalizedType.Args;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Number { get; set; }

        public string Args { get; set; }

        public int Color { get; set; }

        public bool Background { get; set; }

        public bool Scrollbar { get; set; }

        public GumpHtmlLocalizedType Type { get; set; }

        public override string Compile(OrderedHashSet<string> strings) =>
            Type switch
            {
                GumpHtmlLocalizedType.Plain =>
                    $"{{ xmfhtmlgump {X} {Y} {Width} {Height} {Number} {(Background ? 1 : 0)} {(Scrollbar ? 1 : 0)} }}",
                GumpHtmlLocalizedType.Color =>
                    $"{{ xmfhtmlgumpcolor {X} {Y} {Width} {Height} {Number} {(Background ? 1 : 0)} {(Scrollbar ? 1 : 0)} {Color} }}",
                _ =>
                    $"{{ xmfhtmltok {X} {Y} {Width} {Height} {(Background ? 1 : 0)} {(Scrollbar ? 1 : 0)} {Color} {Number} @{Args}@ }}"
            };

        public override void AppendTo(ref SpanWriter writer, OrderedHashSet<string> strings, ref int entries, ref int switches)
        {
            writer.Write((ushort)0x7B20); // "{ "

            switch (Type)
            {
                case GumpHtmlLocalizedType.Plain:
                    {
                        writer.Write(LayoutNamePlain);
                        writer.WriteAscii(' ');
                        writer.WriteAscii(X.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Y.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Width.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Height.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Number.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Background ? '1' : '0');
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Scrollbar ? '1' : '0');

                        break;
                    }
                case GumpHtmlLocalizedType.Color:
                    {
                        writer.Write(LayoutNameColor);
                        writer.WriteAscii(' ');
                        writer.WriteAscii(X.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Y.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Width.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Height.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Number.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Background ? '1' : '0');
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Scrollbar ? '1' : '0');
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Color.ToString());

                        break;
                    }
                case GumpHtmlLocalizedType.Args:
                    {
                        writer.Write(LayoutNameArgs);
                        writer.WriteAscii(' ');
                        writer.WriteAscii(X.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Y.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Width.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Height.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Background ? '1' : '0');
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Scrollbar ? '1' : '0');
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Color.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii(Number.ToString());
                        writer.WriteAscii(' ');
                        writer.WriteAscii('@');
                        writer.WriteAscii(Args ?? "");
                        writer.WriteAscii('@');

                        break;
                    }
            }

            writer.Write((ushort)0x207D); // " }"
        }
    }
}
