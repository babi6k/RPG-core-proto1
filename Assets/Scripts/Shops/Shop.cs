using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using RPG.Control;
using RPG.Inventories;
using RPG.Stats;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable, ISaveable
    {
        [SerializeField] string shopName;
        [Range(0, 100)]
        [SerializeField] float sellingPercentage = 80f;
        [SerializeField] List<StockItemConfig> stockConfig = new List<StockItemConfig>();

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
        Dictionary<InventoryItem, int> stockSold = new Dictionary<InventoryItem, int>();

        Shopper currentShopper = null;
        bool isBuyingMode = true;
        ItemCategory filter = ItemCategory.None;

        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(0, 100)]
            public float buyingDiscountPercentage;
            public int levelToUnlock = 0;
        }

        public event Action onChange;

        
#region Getters
        public string GetShopName() { return shopName; }

        public bool IsBuyingMode(){return isBuyingMode;}

        private int GetShopperLevel()
        {
            BaseStats stats = currentShopper.GetComponent<BaseStats>();
            if (stats == null) return 0;

            return stats.GetLevel();
        }

         public CursorType GetCursorType()
        {
            return CursorType.Shop;
        }
#endregion

        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }
        public void SelectMode(bool isBuying)
        {
            isBuyingMode = isBuying;
            if (onChange != null)
            {
                onChange();
            }
        }

#region filters getting shop items
        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoyItem();
                if (filter == ItemCategory.None || item.GetCategory() == filter)
                {
                    yield return shopItem;
                }
            }
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            Dictionary<InventoryItem, float> prices = GetPrices();
            Dictionary<InventoryItem, int> availabilites = GetAvailabilies();
            foreach (InventoryItem item in availabilites.Keys)
            {
                if (availabilites[item] <= 0) continue;
                float price = prices[item];
                int quantitiyInTransaction = 0;
                transaction.TryGetValue(item, out quantitiyInTransaction);
                int availability = availabilites[item];
                yield return new ShopItem(item, availability,price, quantitiyInTransaction);
            }
        }

        public void SelectFilter(ItemCategory category)
        {
            filter = category;
            print(category);

            if (onChange != null)
            {
                onChange();
            }
        }

        public ItemCategory GetFilter()
        {
            return filter;
        }
#endregion
        
       
#region Transactions
        public bool CanTransact()
        {
            if (IsTransactionEmpty()) return false;
            if (!HasSuffiecientFund()) return false;
            if (!HasInventorySpace()) return false;
            return true;
        }

        public bool HasSuffiecientFund()
        {
            if (!isBuyingMode) return true;
            Money shopperMoney = currentShopper.GetComponent<Money>();
            if (shopperMoney == null) return false;
            return shopperMoney.GetBalance() >= TransactionTotal();
        }

        public bool HasInventorySpace()
        {
            if (!isBuyingMode) return true;
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
                    if (isBuyingMode)
                    {
                        BuyItem(shopperInventory, shopperMoney, item, price);
                    }
                    else
                    {
                        SellItem(shopperInventory, shopperMoney, item, price);
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
            var availabilites = GetAvailabilies();
            int availability = availabilites[item];
            if (transaction[item] + quantitiy > availability)
            {
                transaction[item] = availability;
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
#endregion

        public bool HandleRayCast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().SetActiveShop(this);
            }
            return true;
        }
#region Buying / Selling
        private int CountItemsInInventory(InventoryItem item)
        {

            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null) return 0;

            int count = 0;
            for (int i = 0; i < shopperInventory.GetSize(); i++)
            {
                if (shopperInventory.GetItemInSlot(i) == item)
                {
                    count += shopperInventory.GetNumberInSlot(i);
                }
            }
            return count;
        }

        private Dictionary<InventoryItem, int> GetAvailabilies()
        {
            Dictionary<InventoryItem , int> availabilities = new Dictionary<InventoryItem, int>();

            foreach (var config in GetAvailableConfigs())
            {
                if (isBuyingMode)
                {
                    if (!availabilities.ContainsKey(config.item))
                    {
                        int sold = 0;
                        stockSold.TryGetValue(config.item, out sold);
                        availabilities[config.item] = -sold;
                    }

                    availabilities[config.item] += config.initialStock;
                }
                else
                {
                    availabilities[config.item] = CountItemsInInventory(config.item);
                }
            }
            return availabilities;
        }

        private Dictionary<InventoryItem, float> GetPrices()
        {
            Dictionary<InventoryItem, float> prices = new Dictionary<InventoryItem, float>();
            foreach (var config in GetAvailableConfigs())
            {
                if (isBuyingMode)
                {
                    if (!prices.ContainsKey(config.item))
                    {
                        prices[config.item] = config.item.GetPrice();
                    }

                    prices[config.item] *= (1 - config.buyingDiscountPercentage / 100);
                }
                else
                {
                    prices[config.item] = config.item.GetPrice() * (sellingPercentage / 100);
                }
            }

            return prices;
        }

        private IEnumerable<StockItemConfig> GetAvailableConfigs()
        {
            int shopperLevel = GetShopperLevel();
            foreach (var config in stockConfig)
            {
                if (config.levelToUnlock > shopperLevel) continue;
                yield return config;
            }
        }

        private void SellItem(Inventory shopperInventory, Money shopperMoney, InventoryItem item, float price)
        {
            int slot = FindFirstItemSlot(shopperInventory, item);
            if (slot == -1) return;

            AddToTransaction(item, -1);
            shopperInventory.RemoveFromSlot(slot, 1);
            if (!stockSold.ContainsKey(item))
            {
                stockSold[item] = 0;
            }
            stockSold[item]--;
            shopperMoney.UpdateBalance(price);

        }

        private void BuyItem(Inventory shopperInventory, Money shopperMoney, InventoryItem item, float price)
        {
            if (shopperMoney.GetBalance() < price) return;
            bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
            if (success)
            {
                AddToTransaction(item, -1);
                if (!stockSold.ContainsKey(item))
                {
                    stockSold[item] = 0;
                }
                stockSold[item]++;
                shopperMoney.UpdateBalance(-price);
            }
        }

        private int FindFirstItemSlot(Inventory shopperInventory, InventoryItem item)
        {
            for (int i = 0; i < shopperInventory.GetSize(); i++)
            {
                if (shopperInventory.GetItemInSlot(i) == item)
                {
                    return i;
                }
            }
            return -1;
        }
#endregion
#region Saving
        public object CaptureState()
        {
            Dictionary<string, int> saveObject = new Dictionary<string, int>();
            foreach (var pair in stockSold)
            {
                saveObject[pair.Key.GetItemID()] = pair.Value;
            }

            return saveObject;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, int> saveObject = (Dictionary<string, int>) state;
            stockSold.Clear();
            foreach(var pair in saveObject)
            {
                stockSold[InventoryItem.GetFromID(pair.Key)] = pair.Value; 
            }
        }
#endregion
    
    }
}
