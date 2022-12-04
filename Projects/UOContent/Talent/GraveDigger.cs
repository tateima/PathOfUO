using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class GraveDigger : BaseTalent
    {
        private Dictionary<Point2D, DateTime> _lastGraves;

        public GraveDigger()
        {
            _lastGraves = new Dictionary<Point2D, DateTime>();
            DisplayName = "Grave digger";
            Description =
                "Allows you to dig up items from graveyards as well as other things! Each level improves rewards. Requires 70+ mining.";
            ImageID = 402;
            GumpHeight = 85;
            AddEndY = 105;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Mining.Base >= 70;

        public void Dig(BaseHarvestTool tool, Mobile mobile, Point3D point)
        {
            if (HasSkillRequirement(mobile))
            {
                var expiryTime = DateTime.Now.AddMinutes(-30);
                var expiredGraves = _lastGraves.Where(g => g.Value < expiryTime).ToList();
                foreach (KeyValuePair<Point2D, DateTime> entry in expiredGraves)
                {
                    _lastGraves.Remove(entry.Key);
                }

                bool canDig = _lastGraves.All(entry => !mobile.InRange(entry.Key, 3));
                if (canDig && mobile.BeginAction(tool))
                {
                    new DigTimer(mobile, tool, point, this).Start();
                }
                else
                {
                    mobile.SendMessage("You cannot dig here right now");
                }
            }
            else
            {
                mobile.SendMessage("You cannot dig as you don't have the skill.");
            }
        }

        private class DigTimer : Timer
        {
            private readonly Mobile _from;
            private readonly BaseHarvestTool _tool;
            private readonly long _lastMoveTime;
            private readonly long _nextActionTime;
            private readonly long _nextSpellTime;
            private readonly GraveDigger _graveDigger;
            private int _count;
            private TreasureChestDirt _dirt1;
            private TreasureChestDirt _dirt2;
            private GraveDiggerBone _bone1;
            private GraveDiggerBone _bone2;
            private readonly Point3D _location;

            public DigTimer(Mobile from, BaseHarvestTool tool, Point3D location, GraveDigger graveDigger) : base(
                TimeSpan.Zero,
                TimeSpan.FromSeconds(1.0)
            )
            {
                _from = from;
                _location = location;
                _tool = tool;
                _nextSpellTime = from.NextSpellTime;
                _nextActionTime = from.NextActionTime;
                _lastMoveTime = from.LastMoveTime;
                _graveDigger = graveDigger;
            }

            private void Terminate(bool delete = false)
            {
                _graveDigger._lastGraves.Add(new Point2D(_location.X, _location.Y), DateTime.Now);
                Stop();
                _from.EndAction(_tool);
                if (delete)
                {
                    _tool.Delete();
                }

                if (_dirt1 != null)
                {
                    _dirt1.Delete();
                    _dirt2.Delete();
                }
            }

            protected override void OnTick()
            {
                if (_tool.UsesRemaining == 1)
                {
                    Terminate(true);
                }

                if (_nextSpellTime != _from.NextSpellTime ||
                    _nextActionTime != _from.NextActionTime)
                {
                    Terminate();
                    return;
                }

                if (_lastMoveTime != _from.LastMoveTime)
                {
                    _from.SendMessage("You cannot move around while digging up graves.");
                    Terminate();
                    return;
                }

                _count++;

                _from.RevealingAction();
                _from.Direction = _from.GetDirectionTo(_location);

                if (_from.Body.IsHuman && !_from.Mounted)
                {
                    _from.Animate(11, 5, 1, true, false, 0);
                }

                if (_count > 1 && _dirt1 == null)
                {
                    _dirt1 = new TreasureChestDirt();
                    _dirt1.MoveToWorld(_location, _from.Map);

                    _dirt2 = new TreasureChestDirt();
                    _dirt2.MoveToWorld(new Point3D(_location.X, _location.Y - 1, _location.Z), _from.Map);
                }

                if (_count > 5 && _bone1 is null)
                {
                    _bone1 = new GraveDiggerBone();
                    _bone1.MoveToWorld(new Point3D(_location.X, _location.Y - 1, _location.Z), _from.Map);
                }

                if (_count > 10 && _bone2 is null)
                {
                    _bone2 = new GraveDiggerBone();
                    _bone2.MoveToWorld(new Point3D(_location.X, _location.Y + 1, _location.Z), _from.Map);
                }

                new SoundTimer(_from, 0x125 + _count % 2).Start();

                if (Utility.Random(100) < 10)
                {
                    // spawn undead
                    BaseCreature undead = Utility.RandomMinMax(1, 7 + _graveDigger.Level) switch
                    {
                        1  => new Shade(),
                        2  => new Spectre(),
                        3  => new Wraith(),
                        4  => new Skeleton(),
                        5  => new Zombie(),
                        6  => new Ghoul(),
                        7  => new RottingCorpse(),
                        8  => new SkeletalKnight(),
                        9  => new BoneKnight(),
                        10 => new BoneMagi(),
                        11 => new SkeletalMage(),
                        12 => new Lich(),
                        _  => null
                    };

                    if (undead != null)
                    {
                        Effects.PlaySound(_from.Location, _from.Map, 0x1FB);
                        Effects.SendLocationParticles(
                            EffectItem.Create(undead.Location, undead.Map, EffectItem.DefaultDuration),
                            0x37CC,
                            1,
                            40,
                            97,
                            3,
                            9917,
                            0
                        );
                        undead.MoveToWorld(_location, _from.Map);
                        Terminate();
                    }
                }
                else if (Utility.Random(100) < _graveDigger.Level)
                {
                    if (Utility.RandomBool())
                    {
                        var brigand = new Brigand();
                        brigand.MoveToWorld(_location, _from.Map);
                        _from.SendMessage("You dig up a murderer who was buried alive.");
                        Terminate();
                    }
                    else
                    {
                        if (_from.Backpack != null)
                        {
                            if (Utility.Random(500) < 1)
                            {
                                _from.Backpack.AddItem(new RuneScroll());
                            }
                            else
                            {
                                switch (Utility.RandomMinMax(1, 12))
                                {
                                    case 1:
                                        var gem = Loot.RandomGem();
                                        _from.Backpack.AddItem(gem);
                                        break;
                                    case 2:
                                        var gold = new Gold(Utility.Random(_graveDigger.Level));
                                        _from.Backpack.AddItem(gold);
                                        break;
                                    case 3:
                                        var jewelry = Loot.RandomJewelry();
                                        _from.Backpack.AddItem(jewelry);
                                        break;
                                    case 4:
                                        var book = Loot.RandomLibraryBook();
                                        _from.Backpack.AddItem(book);
                                        break;
                                    case 5:
                                        var reagent = Loot.RandomNecromancyReagent();
                                        _from.Backpack.AddItem(reagent);
                                        break;
                                    case 6:
                                        var potion = Loot.RandomPotion();
                                        _from.Backpack.AddItem(potion);
                                        break;
                                    case 7:
                                        var scroll = Loot.RandomScroll(0, 15, SpellbookType.Necromancer);
                                        if (Utility.RandomBool())
                                        {
                                            scroll = Loot.RandomScroll(0, 63, SpellbookType.Regular);
                                        }

                                        _from.Backpack.AddItem(scroll);
                                        break;
                                    case 8:
                                        var weapon = Loot.RandomWeapon();
                                        _from.Backpack.AddItem(weapon);
                                        break;
                                    case 9:
                                        var wand = Loot.RandomWand();
                                        _from.Backpack.AddItem(wand);
                                        break;
                                    case 10:
                                        var clothing = Loot.RandomClothing();
                                        _from.Backpack.AddItem(clothing);
                                        break;
                                    case 11:
                                        var hat = Loot.RandomHat();
                                        _from.Backpack.AddItem(hat);
                                        break;
                                    case 12:
                                        var shield = Loot.RandomShield();
                                        _from.Backpack.AddItem(shield);
                                        break;
                                }
                            }

                            if (Utility.RandomBool())
                            {
                                Terminate();
                            }

                            _from.SendMessage("You dig up an item from the grave.");
                        }
                    }
                }

                _tool.UsesRemaining--;
            }

            private class SoundTimer : Timer
            {
                private readonly Mobile _from;
                private readonly int _soundId;

                public SoundTimer(Mobile from, int soundID) : base(TimeSpan.FromSeconds(0.9))
                {
                    _from = from;
                    _soundId = soundID;
                }

                protected override void OnTick()
                {
                    _from.PlaySound(_soundId);
                }
            }
        }
    }
    public sealed class GraveDiggerBone : Item
    {
        public GraveDiggerBone() : base(0x0ECA)
        {
            Movable = false;
            ItemID = Utility.RandomMinMax(1, 8) switch
            {
                1 => 0x0ECA,
                2 => 0x0ECB,
                3 => 0x0ECC,
                4 => 0x0ECD,
                5 => 0x0ECE,
                6 => 0x0ED0,
                7 => 0x0ED1,
                8 => 0x0ED2,
                _ => ItemID
            };

            Timer.StartTimer(TimeSpan.FromMinutes(2.0), Delete);
        }

        public GraveDiggerBone(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadEncodedInt();

            Delete();
        }
    }
}
