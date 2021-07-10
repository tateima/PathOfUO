namespace Server.Engines.Chat
{
    public static class ChatActionHandlers
    {
        private static readonly ChatActionHandler[] m_Handlers;

        static ChatActionHandlers()
        {
            m_Handlers = new ChatActionHandler[0x100];

            Register(0x41, true, true, ChangeChannelPassword);

            Register(0x58, false, false, LeaveChat);

            Register(0x61, false, true, ChannelMessage);
            Register(0x62, false, false, JoinChannel);
            Register(0x63, false, false, JoinNewChannel);
            Register(0x64, true, true, RenameChannel);
            Register(0x65, false, false, PrivateMessage);
            Register(0x66, false, false, AddIgnore);
            Register(0x67, false, false, RemoveIgnore);
            Register(0x68, false, false, ToggleIgnore);
            Register(0x69, true, true, AddVoice);
            Register(0x6A, true, true, RemoveVoice);
            Register(0x6B, true, true, ToggleVoice);
            Register(0x6C, true, true, AddModerator);
            Register(0x6D, true, true, RemoveModerator);
            Register(0x6E, true, true, ToggleModerator);
            Register(0x6F, false, false, AllowPrivateMessages);
            Register(0x70, false, false, DisallowPrivateMessages);
            Register(0x71, false, false, TogglePrivateMessages);
            Register(0x72, false, false, ShowCharacterName);
            Register(0x73, false, false, HideCharacterName);
            Register(0x74, false, false, ToggleCharacterName);
            Register(0x75, false, false, QueryWhoIs);
            Register(0x76, true, true, Kick);
            Register(0x77, true, true, EnableDefaultVoice);
            Register(0x78, true, true, DisableDefaultVoice);
            Register(0x79, true, true, ToggleDefaultVoice);
            Register(0x7A, false, true, EmoteMessage);
        }

        public static void Register(int actionID, bool requireModerator, bool requireConference, OnChatAction callback)
        {
            if (actionID >= 0 && actionID < m_Handlers.Length)
            {
                m_Handlers[actionID] = new ChatActionHandler(requireModerator, requireConference, callback);
            }
        }

        public static ChatActionHandler GetHandler(int actionID)
        {
            if (actionID >= 0 && actionID < m_Handlers.Length)
            {
                return m_Handlers[actionID];
            }

            return null;
        }

        public static void ChannelMessage(ChatUser from, Channel channel, string param)
        {
            if (channel.CanTalk(from))
            {
                channel.SendIgnorableMessage(57, from, from.GetColorCharacter() + from.Username, param); // %1: %2
            }
            else
            {
                from.SendMessage(36); // The moderator of this conference has not given you speaking privileges.
            }
        }

        public static void EmoteMessage(ChatUser from, Channel channel, string param)
        {
            if (channel.CanTalk(from))
            {
                channel.SendIgnorableMessage(58, from, from.GetColorCharacter() + from.Username, param); // %1 %2
            }
            else
            {
                from.SendMessage(36); // The moderator of this conference has not given you speaking privileges.
            }
        }

        public static void PrivateMessage(ChatUser from, Channel channel, string param)
        {
            var indexOf = param.IndexOfOrdinal(' ');

            var name = param[..indexOf];
            var text = param[(indexOf + 1)..];

            var target = ChatSystem.SearchForUser(from, name);

            if (target == null)
            {
                return;
            }

            if (target.IsIgnored(from))
            {
                // %1 has chosen to ignore you. None of your messages to them will get through.
                from.SendMessage(35, target.Username);
            }
            else if (target.IgnorePrivateMessage)
            {
                from.SendMessage(42, target.Username); // %1 has chosen to not receive private messages at the moment.
            }
            else
            {
                // [%1]: %2
                target.SendMessage(59, from.Mobile, $"{from.GetColorCharacter()}{from.Username}", text);
            }
        }

        public static void LeaveChat(ChatUser from, Channel channel, string param)
        {
            ChatUser.RemoveChatUser(from);
        }

        public static void ChangeChannelPassword(ChatUser from, Channel channel, string param)
        {
            channel.Password = param;
            from.SendMessage(60); // The password to the conference has been changed.
        }

        public static void AllowPrivateMessages(ChatUser from, Channel channel, string param)
        {
            from.IgnorePrivateMessage = false;
            from.SendMessage(37); // You can now receive private messages.
        }

        public static void DisallowPrivateMessages(ChatUser from, Channel channel, string param)
        {
            from.IgnorePrivateMessage = true;
            /* You will no longer receive private messages.
             * Those who send you a message will be notified that you are blocking incoming messages.
             */
            from.SendMessage(38);
        }

        public static void TogglePrivateMessages(ChatUser from, Channel channel, string param)
        {
            from.IgnorePrivateMessage = !from.IgnorePrivateMessage;
            from.SendMessage(from.IgnorePrivateMessage ? 38 : 37); // See above for messages
        }

        public static void ShowCharacterName(ChatUser from, Channel channel, string param)
        {
            from.Anonymous = false;

            // You are now showing your character name to any players who inquire with the whois command.
            from.SendMessage(39);
        }

        public static void HideCharacterName(ChatUser from, Channel channel, string param)
        {
            from.Anonymous = true;

            // You are no longer showing your character name to any players who inquire with the whois command.
            from.SendMessage(40);
        }

        public static void ToggleCharacterName(ChatUser from, Channel channel, string param)
        {
            from.Anonymous = !from.Anonymous;
            from.SendMessage(from.Anonymous ? 40 : 39); // See above for messages
        }

        public static void JoinChannel(ChatUser from, Channel channel, string param)
        {
            string name;
            string password = null;

            var start = param.IndexOfOrdinal('\"');

            if (start >= 0)
            {
                var end = param.IndexOf('\"', ++start);

                if (end >= 0)
                {
                    name = param.Substring(start, end - start);
                    password = param[++end..];
                }
                else
                {
                    name = param[start..];
                }
            }
            else
            {
                var indexOf = param.IndexOfOrdinal(' ');

                if (indexOf >= 0)
                {
                    name = param[..indexOf++];
                    password = param[indexOf..];
                }
                else
                {
                    name = param;
                }
            }

            password = (password?.Trim()).DefaultIfNullOrEmpty(null);

            var joined = Channel.FindChannelByName(name);

            if (joined == null)
            {
                from.SendMessage(33, name); // There is no conference named '%1'.
            }
            else
            {
                joined.AddUser(from, password);
            }
        }

        public static void JoinNewChannel(ChatUser from, Channel channel, string param)
        {
            if ((param = param.Trim()).Length == 0)
            {
                return;
            }

            string name;
            string password = null;

            var start = param.IndexOfOrdinal('{');

            if (start >= 0)
            {
                name = param[..start++];

                var end = param.IndexOf('}', start);

                if (end >= start)
                {
                    password = param.Substring(start, end - start);
                }
            }
            else
            {
                name = param;
            }

            password = (password?.Trim()).DefaultIfNullOrEmpty(null);

            Channel.AddChannel(name, password).AddUser(from, password);
        }

        public static void AddIgnore(ChatUser from, Channel channel, string param)
        {
            var target = ChatSystem.SearchForUser(from, param);

            if (target == null)
            {
                return;
            }

            from.AddIgnored(target);
        }

        public static void RemoveIgnore(ChatUser from, Channel channel, string param)
        {
            var target = ChatSystem.SearchForUser(from, param);

            if (target == null)
            {
                return;
            }

            from.RemoveIgnored(target);
        }

        public static void ToggleIgnore(ChatUser from, Channel channel, string param)
        {
            var target = ChatSystem.SearchForUser(from, param);

            if (target == null)
            {
                return;
            }

            if (from.IsIgnored(target))
            {
                from.RemoveIgnored(target);
            }
            else
            {
                from.AddIgnored(target);
            }
        }

        public static void AddVoice(ChatUser from, Channel channel, string param)
        {
            var target = ChatSystem.SearchForUser(from, param);

            if (target != null)
            {
                channel.AddVoiced(target, from);
            }
        }

        public static void RemoveVoice(ChatUser from, Channel channel, string param)
        {
            var target = ChatSystem.SearchForUser(from, param);

            if (target != null)
            {
                channel.RemoveVoiced(target, from);
            }
        }

        public static void ToggleVoice(ChatUser from, Channel channel, string param)
        {
            var target = ChatSystem.SearchForUser(from, param);

            if (target == null)
            {
                return;
            }

            if (channel.IsVoiced(target))
            {
                channel.RemoveVoiced(target, from);
            }
            else
            {
                channel.AddVoiced(target, from);
            }
        }

        public static void AddModerator(ChatUser from, Channel channel, string param)
        {
            var target = ChatSystem.SearchForUser(from, param);

            if (target != null)
            {
                channel.AddModerator(target, from);
            }
        }

        public static void RemoveModerator(ChatUser from, Channel channel, string param)
        {
            var target = ChatSystem.SearchForUser(from, param);

            if (target != null)
            {
                channel.RemoveModerator(target, from);
            }
        }

        public static void ToggleModerator(ChatUser from, Channel channel, string param)
        {
            var target = ChatSystem.SearchForUser(from, param);

            if (target == null)
            {
                return;
            }

            if (channel.IsModerator(target))
            {
                channel.RemoveModerator(target, from);
            }
            else
            {
                channel.AddModerator(target, from);
            }
        }

        public static void RenameChannel(ChatUser from, Channel channel, string param)
        {
            channel.Name = param;
        }

        public static void QueryWhoIs(ChatUser from, Channel channel, string param)
        {
            var target = ChatSystem.SearchForUser(from, param);

            if (target == null)
            {
                return;
            }

            if (target.Anonymous)
            {
                from.SendMessage(41, target.Username); // %1 is remaining anonymous.
            }
            else
            {
                from.SendMessage(43, target.Username, target.Mobile.Name); // %2 is known in the lands of Britannia as %2.
            }
        }

        public static void Kick(ChatUser from, Channel channel, string param)
        {
            var target = ChatSystem.SearchForUser(from, param);

            if (target != null)
            {
                channel.Kick(target, from);
            }
        }

        public static void EnableDefaultVoice(ChatUser from, Channel channel, string param)
        {
            channel.VoiceRestricted = false;
        }

        public static void DisableDefaultVoice(ChatUser from, Channel channel, string param)
        {
            channel.VoiceRestricted = true;
        }

        public static void ToggleDefaultVoice(ChatUser from, Channel channel, string param)
        {
            channel.VoiceRestricted = !channel.VoiceRestricted;
        }
    }
}
