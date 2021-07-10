using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells.Spellweaving
{
    public class ArcaneCircleSpell : ArcanistSpell
    {
        private static readonly SpellInfo m_Info = new(
            "Arcane Circle",
            "Myrshalee",
            -1
        );

        public ArcaneCircleSpell(Mobile caster, Item scroll = null)
            : base(caster, scroll, m_Info)
        {
        }

        public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(0.5);

        public override double RequiredSkill => 0.0;
        public override int RequiredMana => 24;

        public override bool CheckCast()
        {
            if (!IsValidLocation(Caster.Location, Caster.Map))
            {
                Caster.SendLocalizedMessage(
                    1072705
                ); // You must be standing on an arcane circle, pentagram or abbatoir to use this spell.
                return false;
            }

            if (GetArcanists().Count < 2)
            {
                Caster.SendLocalizedMessage(1080452); // There are not enough spellweavers present to create an Arcane Focus.
                return false;
            }

            return base.CheckCast();
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x3779, 10, 20, 0x0, EffectLayer.Waist);
                Caster.PlaySound(0x5C0);

                var Arcanists = GetArcanists();

                var duration = TimeSpan.FromHours(Math.Max(1, (int)(Caster.Skills.Spellweaving.Value / 24)));

                var strengthBonus =
                    Math.Min(
                        Arcanists.Count,
                        IsSanctuary(Caster.Location, Caster.Map)
                            ? 6
                            : 5
                    ); // The Sanctuary is a special, single location place

                for (var i = 0; i < Arcanists.Count; i++)
                {
                    GiveArcaneFocus(Arcanists[i], duration, strengthBonus);
                }
            }

            FinishSequence();
        }

        private static bool IsSanctuary(Point3D p, Map m) =>
            (m == Map.Trammel || m == Map.Felucca) && p.X == 6267 && p.Y == 131;

        private static bool IsValidLocation(Point3D location, Map map)
        {
            var lt = map.Tiles.GetLandTile(location.X, location.Y); // Land   Tiles

            if (IsValidTile(lt.ID) && lt.Z == location.Z)
            {
                return true;
            }

            var tiles = map.Tiles.GetStaticTiles(location.X, location.Y); // Static Tiles

            for (var i = 0; i < tiles.Length; ++i)
            {
                var t = tiles[i];
                var id = TileData.ItemTable[t.ID & TileData.MaxItemValue];

                var tand = t.ID;

                if (t.Z + id.CalcHeight != location.Z)
                {
                    continue;
                }

                if (IsValidTile(tand))
                {
                    return true;
                }
            }

            var eable = map.GetItemsInRange(location, 0);

            foreach (var item in eable)
            {
                if (item.Z + item.ItemData.CalcHeight == location.Z && IsValidTile(item.ItemID))
                {
                    return true;
                }
            }

            eable.Free();

            return false;
        }

        public static bool IsValidTile(int itemID) =>
            itemID == 0xFEA || itemID == 0x1216 || itemID == 0x307F || itemID == 0x1D10 || itemID == 0x1D0F ||
            itemID == 0x1D1F ||
            itemID == 0x1D12;

        private List<Mobile> GetArcanists()
        {
            var weavers = new List<Mobile> { Caster };

            // OSI Verified: Even enemies/combatants count
            // Everyone gets the Arcane Focus, power capped elsewhere

            var eable = Caster.GetMobilesInRange(1);

            foreach (var m in eable)
            {
                if (m != Caster && m is PlayerMobile && Caster.CanBeBeneficial(m, false) &&
                    Math.Abs(Caster.Skills.Spellweaving.Value - m.Skills.Spellweaving.Value) <= 20)
                {
                    weavers.Add(m);
                }
            }

            eable.Free();

            return weavers;
        }

        private void GiveArcaneFocus(Mobile to, TimeSpan duration, int strengthBonus)
        {
            if (to == null) // Sanity
            {
                return;
            }

            var focus = FindArcaneFocus(to);

            if (focus == null)
            {
                focus = new ArcaneFocus(duration, strengthBonus);
                if (to.PlaceInBackpack(focus))
                {
                    focus.SendTimeRemainingMessage(to);
                    to.SendLocalizedMessage(1072740); // An arcane focus appears in your backpack.
                }
                else
                {
                    focus.Delete();
                }
            }
            else // OSI renewal rules: the new one will override the old one, always.
            {
                to.SendLocalizedMessage(1072828); // Your arcane focus is renewed.
                focus.LifeSpan = duration;
                focus.CreationTime = Core.Now;
                focus.StrengthBonus = strengthBonus;
                focus.InvalidateProperties();
                focus.SendTimeRemainingMessage(to);
            }
        }
    }
}
