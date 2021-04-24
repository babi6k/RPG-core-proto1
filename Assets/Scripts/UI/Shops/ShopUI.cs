using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI shopName;
        [SerializeField] Transform listRoot;
        [SerializeField] RowUI rowPrefab;
        [SerializeField] TextMeshProUGUI totalField;
        [SerializeField] TextMeshProUGUI bankField;
        [SerializeField] Button confirmButton;
        [SerializeField] Button buyingModeButton;
        [SerializeField] Button sellingModeButton;
        
        Color oldTotalTxtColor;
        Color inActiveButtonColor;
        Color activeButtonColor;

        Shopper shopper = null;
        Shop currentShop = null;
        // Start is called before the first frame update
        void Start()
        {
            oldTotalTxtColor = totalField.color;
            inActiveButtonColor = sellingModeButton.image.color;
            activeButtonColor = buyingModeButton.image.color;
            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
            if (shopper == null) return;

            shopper.activeShopChange += ShopChanged;
            confirmButton.onClick.AddListener(ConfirmTransaction);
            buyingModeButton.onClick.AddListener(SwitchToBuying);
            sellingModeButton.onClick.AddListener(SwitchToSelling);
            ShopChanged();
        }

        private void ShopChanged()
        {
            if (currentShop != null)
            {
                currentShop.onChange -= RefreshUI;
            }
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);
            foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.SetShop(currentShop);
            }
            if (currentShop == null) return;
            shopName.text = currentShop.GetShopName();

            currentShop.onChange += RefreshUI;
            RefreshUI();
        }

        private void RefreshUI()
        {
            foreach(Transform child in listRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (ShopItem item in currentShop.GetFilteredItems())
            {
                RowUI row = Instantiate<RowUI>(rowPrefab,listRoot);
                row.Setup(currentShop, item);
            }

            totalField.text = $"Total: {currentShop.TransactionTotal():N2}";
            bankField.text = $"Bank: {currentShop.BankTotal():N2}";
            totalField.color = currentShop.HasSuffiecientFund() ? oldTotalTxtColor : Color.red;
            confirmButton.interactable = currentShop.CanTransact();
            TextMeshProUGUI buyModeText = buyingModeButton.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI sellModeText = sellingModeButton.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI confirmText = confirmButton.GetComponentInChildren<TextMeshProUGUI>();

            if (currentShop.IsBuyingMode())
            {
                buyModeText.color = Color.white;
                buyingModeButton.image.color = activeButtonColor;
                sellModeText.color = Color.yellow;
                sellingModeButton.image.color = inActiveButtonColor;
                confirmText.text = "Buy";
            }
            else
            {
                buyModeText.color = Color.yellow;
                buyingModeButton.image.color = inActiveButtonColor;
                sellModeText.color = Color.white;
                sellingModeButton.image.color = activeButtonColor;
                confirmText.text = "Sell";
            }

            foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.RefreshUI();
            }
        }

        public void Close()
        {
            shopper.SetActiveShop(null);
        }

        public void ConfirmTransaction()
        {
            currentShop.ConfirmTransaction();
        }

        public void SwitchToBuying()
        {
            currentShop.SelectMode(true);
        }

        public void SwitchToSelling()
        {
            currentShop.SelectMode(false);
        }
    }
}
