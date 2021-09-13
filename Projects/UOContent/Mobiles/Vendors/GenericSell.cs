using System;
using System.Collections.Generic;
using Server.Items;
using Server.Talent;

namespace Server.Mobiles
{
    public class GenericSellInfo : IShopSellInfo
    {
        private readonly Dictionary<Type, int> m_Table = new();
        private Type[] m_Types;

        public int GetSellPriceFor(Item item)
        {
            m_Table.TryGetValue(item.GetType(), out var price);
            double tycoonScalar = 0.0;
            double tycoonValue = 0.0;
            if (item.RootParent is PlayerMobile seller)
            {
                BaseTalent tycoon = seller.GetTalent(typeof(TycoonCrafter));
                if (tycoon != null)
                {
                    tycoonScalar = tycoon.ModifySpellScalar();
                }
            }

            if (item is BaseArmor armor)
            {
                price = armor.Quality switch
                {
                    ArmorQuality.Low         => (int)(price * 0.60),
                    ArmorQuality.Exceptional => (int)(price * 1.25),
                    _                        => price
                };

                price += 100 * (int)armor.Durability;
                price += 100 * (int)armor.ProtectionLevel;

                if (armor.Crafter != null && armor.Crafter == item.RootParent)
                {
                    tycoonValue *= (1.0 + tycoonScalar);
                    price += (int)tycoonValue;
                }

                if (price < 1)
                {
                    price = 1;
                }
            }
            else if (item is BaseWeapon weapon)
            {
                price = weapon.Quality switch
                {
                    WeaponQuality.Low         => (int)(price * 0.60),
                    WeaponQuality.Exceptional => (int)(price * 1.25),
                    _                         => price
                };

                price += 100 * (int)weapon.DurabilityLevel;
                price += 100 * (int)weapon.DamageLevel;

                if (weapon.Crafter != null && weapon.Crafter == item.RootParent)
                {
                    tycoonValue *= (1.0 + tycoonScalar);
                    price += (int)tycoonValue;
                }

                if (price < 1)
                {
                    price = 1;
                }
            }
            else if (item is BaseBeverage bev)
            {
                int price1 = price, price2 = price;
                if (bev is Pitcher)
                {
                    price1 = 3;
                    price2 = 5;
                }
                else if (bev is BeverageBottle)
                {
                    price1 = 3;
                    price2 = 3;
                }
                else if (bev is Jug)
                {
                    price1 = 6;
                    price2 = 6;
                }

                if (bev.IsEmpty || bev.Content == BeverageType.Milk)
                {
                    price = price1;
                }
                else
                {
                    price = price2;
                }
            }
           
            return price;
        }

        public int GetBuyPriceFor(Item item)
        {
            double talentScalar = 100.0;
            if (item.RootParent is PlayerMobile seller)
            {
                BaseTalent smoothTalker = seller.GetTalent(typeof(SmoothTalker));
                if (smoothTalker != null)
                {
                    talentScalar -= smoothTalker.ModifySpellScalar();
                }
            }
            int buyPrice = (int)(1.90 * GetSellPriceFor(item));
            buyPrice = AOS.Scale(buyPrice, (int)talentScalar);
            return buyPrice;
        }

        public Type[] Types
        {
            get
            {
                if (m_Types == null)
                {
                    m_Types = new Type[m_Table.Keys.Count];
                    m_Table.Keys.CopyTo(m_Types, 0);
                }

                return m_Types;
            }
        }

        public string GetNameFor(Item item) => item.Name ?? item.LabelNumber.ToString();

        public bool IsSellable(Item item) => !item.Nontransferable && IsInList(item.GetType());

        public bool IsResellable(Item item) => !item.Nontransferable && IsInList(item.GetType());

        public void Add(Type type, int price)
        {
            m_Table[type] = price;
            m_Types = null;
        }

        public bool IsInList(Type type) => m_Table.ContainsKey(type);
    }
}
