using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Control;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable
    {
        [SerializeField] string shopName;
        [SerializeField] List<StockItemConfig> stockConfig = new List<StockItemConfig>();

        Shopper currentShopper;

        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(0, 100)]
            public float buyingDiscountPercentage;
        }

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
        Dictionary<InventoryItem, int> stock = new Dictionary<InventoryItem, int>();

        public event Action onChange;

        private void Awake() 
        {
            foreach (StockItemConfig config in stockConfig)
            {
                stock[config.item] = config.initialStock;
            }
        }

        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }

        public string GetShopName() { return shopName; }

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            return GetAllItems();
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            foreach (StockItemConfig config in stockConfig)
            {
                float price = config.item.GetPrice() * (1 - config.buyingDiscountPercentage / 100);
                int quantitiyInTransaction = 0;
                transaction.TryGetValue(config.item, out quantitiyInTransaction);
                int currentStock = stock[config.item];
                yield return new ShopItem(config.item, currentStock,
                 price, quantitiyInTransaction);
            }
        }

        public void SelectFilter(ItemCategory category)
        {

        }

        public ItemCategory GetFilter() { return ItemCategory.None; }

        public void SelectMode(bool isBuying)
        {

        }

        public bool IsBuyingMode()
        {
            return true;
        }

        public bool CanTransact()
        {
            if (IsTransactionEmpty()) return false;
            if (!HasSuffiecientFund()) return false;
            if (!HasInventorySpace()) return false;
            return true;
        }

        public bool HasSuffiecientFund()
        {
            Money shopperMoney = currentShopper.GetComponent<Money>();
            if (shopperMoney == null) return false;
           return shopperMoney.GetBalance() >= TransactionTotal();
        }

         public bool HasInventorySpace()
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null) return false;
            List<InventoryItem> flatItems = new List<InventoryItem>();
            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoyItem();
                int quantity = shopItem.GetQuantityInTransaction();
                for (int i = 0; i < quantity; i++)
                {
                    flatItems.Add(item);
                }
            } 
            return shopperInventory.HasSpaceFor(flatItems);
        }

        public bool IsTransactionEmpty()
        {
            return transaction.Count == 0;
        }

        public void ConfirmTransaction()
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            Money shopperMoney = currentShopper.GetComponent<Money>();
            if (shopperInventory == null) return;

            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoyItem();
                int quantity = shopItem.GetQuantityInTransaction();
                float price = shopItem.GetPrice();
                for (int i = 0; i < quantity; i++)
                {
                    if (shopperMoney.GetBalance() < price) break;
                    bool success = shopperInventory.AddToFirstEmptySlot(item,1);
                    if (success)
                    {
                        AddToTransaction(item,-1);
                        stock[item] --;
                        shopperMoney.UpdateBalance(-price);
                    }
                }
            }

            if (onChange != null)
            {
                onChange();
            }
        }

        public float TransactionTotal()
        {
            float total = 0;
            foreach (ShopItem item in GetAllItems())
            {
                total += item.GetPrice() * item.GetQuantityInTransaction();
            }
            return total;
        }

        public float BankTotal()
        {
            return currentShopper.GetComponent<Money>().GetBalance();
        }

        public void AddToTransaction(InventoryItem item, int quantitiy)
        {
            if (!transaction.ContainsKey(item))
            {
                transaction[item] = 0;
            }
            if (transaction[item] + quantitiy > stock[item])
            {
                transaction[item] = stock[item];
            }
            else
            {
                transaction[item] += quantitiy;
            }

            if (transaction[item] <= 0)
            {
                transaction.Remove(item);
            }

            if (onChange != null)
            {
                onChange();
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Shop;
        }

        public bool HandleRayCast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().SetActiveShop(this);
            }
            return true;
        }
       
    }
}
