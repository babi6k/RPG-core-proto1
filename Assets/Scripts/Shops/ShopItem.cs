using System;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Shops
{
    public class ShopItem
    {
        InventoryItem item;
        int availability;
        float price;
        int quantityInTransaction;

        public ShopItem(InventoryItem item, int availability, float price, int quantitiyInTransaction)
        {
            this.item = item;
            this.availability = availability;
            this.price = price;
            this.quantityInTransaction = quantitiyInTransaction;
        }

        public int GetAvailability()
        {
            return availability;
        }

        public Sprite GetIcon()
        {
            return item.GetIcon();
        }

        public float GetPrice()
        {
            return price;
        }

        public string GetName()
        {
            return item.GetDisplayName();
        }

        public InventoryItem GetInventoyItem()
        {
            return item;
        }

        public int GetQuantityInTransaction()
        {
            return quantityInTransaction;
        }
    }
}