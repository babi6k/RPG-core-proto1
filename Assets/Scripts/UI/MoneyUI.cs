using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI balanceField;

        Money playerMoney = null;

        private void Start() 
        {
            playerMoney = GameObject.FindGameObjectWithTag("Player").GetComponent<Money>();
            if (playerMoney != null)
            {
                playerMoney.onChange += RefreshUI;
            }
            RefreshUI();
        }

        void RefreshUI()
        {
            balanceField.text = $"{playerMoney.GetBalance():N2}";
        }
    }
}
