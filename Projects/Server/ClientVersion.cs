using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Server.Buffers;

namespace Server
{
    public enum ClientType
    {
        Regular,
        UOTD,
        God,
        SA
    }

    public class ClientVersion : IComparable<ClientVersion>, IComparer<ClientVersion>
    {
        public ClientVersion(int maj, int min, int rev, int pat, ClientType type = ClientType.Regular)
        {
            Major = maj;
            Minor = min;
            Revision = rev;
            Patch = pat;
            Type = type;

            SourceString = Utility.Intern(ToStringImpl());
        }

        public ClientVersion(string fmt)
        {
            fmt = fmt.ToLower();
            SourceString = Utility.Intern(fmt);

            try
            {
                var br1 = fmt.IndexOfOrdinal('.');
                var br2 = fmt.IndexOf('.', br1 + 1);

                var br3 = br2 + 1;
                while (br3 < fmt.Length && char.IsDigit(fmt, br3))
                {
                    br3++;
                }

                Major = Utility.ToInt32(fmt.AsSpan()[..br1]);
                Minor = Utility.ToInt32(fmt.Substring(br1 + 1, br2 - br1 - 1));
                Revision = Utility.ToInt32(fmt.Substring(br2 + 1, br3 - br2 - 1));

                if (br3 < fmt.Length)
                {
                    if (Major <= 5 && Minor <= 0 && Revision <= 6) // Anything before 5.0.7
                    {
                        if (!char.IsWhiteSpace(fmt, br3))
                        {
                            Patch = fmt[br3] - 'a' + 1;
                        }
                    }
                    else
                    {
                        Patch = Utility.ToInt32(fmt.Substring(br3 + 1, fmt.Length - br3 - 1));
                    }
                }

                if (fmt.InsensitiveContains("god") || fmt.InsensitiveContains("gq"))
                {
                    Type = ClientType.God;
                }
                else if (fmt.InsensitiveContains("third dawn") ||
                         fmt.InsensitiveContains("uo:td") ||
                         fmt.InsensitiveContains("uotd") ||
                         fmt.InsensitiveContains("uo3d") ||
                         fmt.InsensitiveContains("uo:3d"))
                {
                    Type = ClientType.UOTD;
                }
                else
                {
                    Type = ClientType.Regular;
                }
            }
            catch
            {
                Major = 0;
                Minor = 0;
                Revision = 0;
                Patch = 0;
                Type = ClientType.Regular;
            }
        }

        public int Major { get; }

        public int Minor { get; }

        public int Revision { get; }

        public int Patch { get; }

        public ClientType Type { get; }

        public string SourceString { get; }

        public int CompareTo(ClientVersion o)
        {
            if (o == null)
            {
                return 1;
            }

            if (Major > o.Major)
            {
                return 1;
            }

            if (Major < o.Major)
            {
                return -1;
            }

            if (Minor > o.Minor)
            {
                return 1;
            }

            if (Minor < o.Minor)
            {
                return -1;
            }

            if (Revision > o.Revision)
            {
                return 1;
            }

            if (Revision < o.Revision)
            {
                return -1;
            }

            if (Patch > o.Patch)
            {
                return 1;
            }

            if (Patch < o.Patch)
            {
                return -1;
            }

            return 0;
        }

        int IComparer<ClientVersion>.Compare(ClientVersion x, ClientVersion y) => Compare(x, y);

        public static bool operator ==(ClientVersion l, ClientVersion r) => Compare(l, r) == 0;

        public static bool operator !=(ClientVersion l, ClientVersion r) => Compare(l, r) != 0;

        public static bool operator >=(ClientVersion l, ClientVersion r) => Compare(l, r) >= 0;

        public static bool operator >(ClientVersion l, ClientVersion r) => Compare(l, r) > 0;

        public static bool operator <=(ClientVersion l, ClientVersion r) => Compare(l, r) <= 0;

        public static bool operator <(ClientVersion l, ClientVersion r) => Compare(l, r) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(Major, Minor, Revision, Patch, Type);

        public override bool Equals(object obj)
        {
            var v = obj as ClientVersion;

            return Major == v?.Major
                   && Minor == v.Minor
                   && Revision == v.Revision
                   && Patch == v.Patch
                   && Type == v.Type;
        }

        private string ToStringImpl()
        {
            using var builder = new ValueStringBuilder(stackalloc char[32]);

            builder.Append(Major.ToString());
            builder.Append('.');
            builder.Append(Minor.ToString());
            builder.Append('.');
            builder.Append(Revision.ToString());

            if (Major <= 5 && Minor <= 0 && Revision <= 6) // Anything before 5.0.7
            {
                if (Patch > 0)
                {
                    builder.Append((char)('a' + (Patch - 1)));
                }
            }
            else
            {
                builder.Append('.');
                builder.Append(Patch.ToString());
            }

            if (Type != ClientType.Regular)
            {
                builder.Append(' ');
                builder.Append(Type.ToString().ToLower());
            }

            return builder.ToString();
        }

        public override string ToString() => SourceString;

        public static bool IsNull(object x) => ReferenceEquals(x, null);

        public static int Compare(ClientVersion a, ClientVersion b)
        {
            if (IsNull(a) && IsNull(b))
            {
                return 0;
            }

            if (IsNull(a))
            {
                return -1;
            }

            if (IsNull(b))
            {
                return 1;
            }

            return a.CompareTo(b);
        }
    }
}
