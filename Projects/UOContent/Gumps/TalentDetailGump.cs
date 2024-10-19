using System;
using System.Linq;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Talent;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Server.Gumps
{
    public class TalentDetailGump : Gump
    {
        private readonly List<TalentGump.TalentGumpPage> _TalentGumpPages;
        private readonly int _LastTalentIndex;
        private readonly int _Page;
        private readonly BaseTalent _Talent;

        public TalentDetailGump(Mobile from, int page, BaseTalent talent, int lastTalentIndex, List<TalentGump.TalentGumpPage> talentGumpPages) : base(0, 0)
        {
            _TalentGumpPages = talentGumpPages;
            _LastTalentIndex = lastTalentIndex;
            _Talent = talent;
            _Page = page;
            Closable = false;
            Disposable = true;
            Draggable = true;
            Resizable = true;
            int dimension = 500;
            AddPage(0);
            AddImageTiled(0, 0, dimension-100, dimension, 0xA8E);
            AddImageTiled(0, 0, 5, dimension, 0x27A7);
            AddImageTiled(0, 0, dimension-100, 5, 0x27A7);
            AddImageTiled(dimension-100, 0, 5, dimension, 0x27A7);
            AddImageTiled(0, dimension, dimension-100, 5, 0x27A7);
            // close button
            AddButton(dimension-100, 0, 40015, 40015, 1);
            int x = 10;
            int y = 10;
            var details = $"{talent.Description}<BR>{talent.AdditionalDetail}";
            if (talent.CanBeUsed)
            {
                details += "<BR>This talent will be added to your talent bar.";
            }
            AddImage(x + 10, y + 10, talent.ImageID);
            AddHtml(x + 80, y + 10, 300, 200, $"<BASEFONT COLOR=#FFFFE5>{talent.DisplayName}</FONT>");
            y += 55;
            AddHtml(x + 80, y, 290, 200, $"<BASEFONT COLOR=#FFFFE5>{details}</FONT>");
            y += talent.AddEndAdditionalDetailsY;
            if (talent.CooldownSeconds > 0)
            {
                y += 55;
                AddHtml(x + 80, y, 300, 200, $"<BASEFONT COLOR=#FFFFE5>Cooldown: {WaitTeleporter.FormatTime(Core.Now.AddSeconds(talent.CooldownSeconds) - Core.Now)}</FONT>");
            }
            if (talent.ManaRequired > 0)
            {
                y += 55;
                AddHtml(x + 80, y, 300, 200, $"<BASEFONT COLOR=#FFFFE5>Mana cost: {talent.ManaRequired.ToString()}</FONT>");
            }
            if (talent.StamRequired > 0)
            {
                y += 55;
                AddHtml(x + 80, y, 300, 200, $"<BASEFONT COLOR=#FFFFE5>Stamina cost: {talent.StamRequired.ToString()}</FONT>");
            }

            if (talent.TalentDependencies is not null)
            {
                string dependencies = "";
                int innerY = 0;
                foreach (var dependencyType in talent.TalentDependencies)
                {
                    BaseTalent dependsOn = TalentConstructor.Construct(dependencyType) as BaseTalent;
                    if (dependsOn is not null)
                    {
                        dependencies = $"{dependencies}{dependsOn.DisplayName}<BR>";
                        innerY += 17;
                    }
                }

                if (dependencies.Length > 0)
                {
                    if (talent.TalentDependencyMinimum > 1)
                    {
                        dependencies = $"A minimum of {talent.TalentDependencyMinimum} talents including:<BR>{dependencies}";
                    }
                    y += 55;
                    AddHtml(x + 80, y, 300, 200, $"<BASEFONT COLOR=#FFFFE5>Requires:<BR>{dependencies}</FONT>");
                    y += innerY;
                }
            }

            if (talent.BlockedBy.Length > 0)
            {
                y += 55;
                AddHtml(x + 80, y, 300, 200, $"<BASEFONT COLOR=#FFFFE5>Blocked by:<BR>{BlockedByList(talent)}</FONT>");
            }
            // Carpentry is the dummy default
            if (talent.RequiredWeaponSkill != SkillName.Carpentry)
            {
                y += 55;
                AddHtml(x + 80, y, 300, 200, $"<BASEFONT COLOR=#FFFFE5>Weapon skill:<BR>{talent.RequiredWeaponSkill.ToString()}</FONT>");
            }

            if (talent.RequiredWeapon.Length > 0)
            {
                y += 55;
                var requiredWeaponsList = ParseTypeList(talent.RequiredWeapon);
                AddHtml(x + 80, y, 300, 100, $"<BASEFONT COLOR=#FFFFE5>Required weapons:<BR>{requiredWeaponsList}</FONT>");
            }

            if (talent.RequiredSpell.Length > 0)
            {
                y += 55;
                var requiredSpellsList = ParseTypeList(talent.RequiredSpell);
                AddHtml(x + 80, y, 300, 100, $"<BASEFONT COLOR=#FFFFE5>Required spells:<BR>{requiredSpellsList}</FONT>");
            }
        }

        public static string BlockedByList(BaseTalent talent)
        {
            var blockedByStr = "";
            foreach (var type in talent.BlockedBy)
            {
                BaseTalent blockedBy = TalentConstructor.Construct(type) as BaseTalent;
                if (blockedBy != null)
                {
                    blockedByStr += $"{blockedBy.DisplayName}";
                    if (type != talent.BlockedBy[^1])
                    {
                        blockedByStr += ", ";
                    }
                }
            }

            return blockedByStr;
        }
        public string ParseTypeList(Type[] types)
        {
            var parsed = "";
            foreach (var type in types)
            {
                parsed += RemoveCamelCase(type.ToString());
                if (type != types[^1])
                {
                    parsed += ", ";
                }
            }
            return parsed;
        }

        public string RemoveCamelCase(string camel)
        {
            string parsed = camel;
            if (parsed.Contains("."))
            {
                parsed = camel.Split(".").Last();
            }
            parsed = Regex.Replace(parsed, "^(Base)", "Any");
            return Regex.Replace(parsed, "(\\B[A-Z])", " $1");
        }

        public override void OnResponse(NetState state, in RelayInfo info)
        {
            PlayerMobile player = (PlayerMobile)state.Mobile;

            if (player != null)
            {
                int page = _Page;
                int lastTalentIndex = _LastTalentIndex;
                if (info.ButtonID == 1)
                {
                    player.CloseGump<TalentDetailGump>();
                    player.SendGump(new TalentGump(player, page, lastTalentIndex, _TalentGumpPages));
                    return;
                }
                player.SendGump(new TalentDetailGump(player, page, _Talent, lastTalentIndex, _TalentGumpPages));
            }
        }
    }
}
