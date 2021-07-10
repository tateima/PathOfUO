using System;

namespace Server.Misc
{
    public enum ProfanityAction
    {
        None,           // no action taken
        Disallow,       // speech is not displayed
        Criminal,       // makes the player criminal, not killable by guards
        CriminalAction, // makes the player criminal, can be killed by guards
        Disconnect,     // player is kicked
        Other           // some other implementation
    }

    public static class ProfanityProtection
    {
        // TODO: Move this to configuration
        private static readonly bool Enabled = false;

        private static readonly ProfanityAction
            Action = ProfanityAction.Disallow; // change here what to do when profanity is detected

        public static char[] Exceptions { get; } =
        {
            ' ', '-', '.', '\'', '"', ',', '_', '+', '=', '~', '`', '!', '^', '*', '\\', '/', ';', ':', '<', '>', '[', ']',
            '{', '}', '?', '|', '(', ')', '%', '$', '&', '#', '@'
        };

        public static string[] StartDisallowed { get; } = Array.Empty<string>();

        public static string[] Disallowed { get; } =
        {
            "jigaboo",
            "chigaboo",
            "wop",
            "kyke",
            "kike",
            "tit",
            "spic",
            "prick",
            "piss",
            "lezbo",
            "lesbo",
            "felatio",
            "dyke",
            "dildo",
            "chinc",
            "chink",
            "cunnilingus",
            "cum",
            "cocksucker",
            "cock",
            "clitoris",
            "clit",
            "ass",
            "hitler",
            "penis",
            "nigga",
            "nigger",
            "klit",
            "kunt",
            "jiz",
            "jism",
            "jerkoff",
            "jackoff",
            "goddamn",
            "fag",
            "blowjob",
            "bitch",
            "asshole",
            "dick",
            "pussy",
            "snatch",
            "cunt",
            "twat",
            "shit",
            "fuck"
        };

        public static void Initialize()
        {
            if (Enabled)
            {
                EventSink.Speech += EventSink_Speech;
            }
        }

        private static bool OnProfanityDetected(Mobile from, string speech)
        {
            switch (Action)
            {
                case ProfanityAction.None:     return true;
                case ProfanityAction.Disallow: return false;
                case ProfanityAction.Criminal:
                    from.Criminal = true;
                    return true;
                case ProfanityAction.CriminalAction:
                    from.CriminalAction(false);
                    return true;
                case ProfanityAction.Disconnect:
                    {
                        from.NetState?.Disconnect("Using profanity.");

                        return false;
                    }
                default:
                case ProfanityAction.Other: // TODO: Provide custom implementation if this is chosen
                    {
                        return true;
                    }
            }
        }

        private static void EventSink_Speech(SpeechEventArgs e)
        {
            var from = e.Mobile;

            if (from.AccessLevel > AccessLevel.Player)
            {
                return;
            }

            if (!NameVerification.Validate(
                e.Speech,
                0,
                int.MaxValue,
                true,
                true,
                false,
                int.MaxValue,
                Exceptions,
                Disallowed,
                StartDisallowed
            ))
            {
                e.Blocked = !OnProfanityDetected(from, e.Speech);
            }
        }
    }
}
