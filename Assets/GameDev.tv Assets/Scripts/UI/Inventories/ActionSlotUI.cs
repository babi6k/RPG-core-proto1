using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
using RPG.Inventories;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// The UI slot for the player action bar.
    /// </summary>
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem> , IPointerClickHandler
    {
        // CONFIG DATA
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] int index = 0;
        [SerializeField] GameObject coolDownEffect = null;

        // CACHE
        ActionStore store;
        CoolDownManager cooldownManager;

        // LIFECYCLE METHODS
        private void Awake()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            store = player.GetComponent<ActionStore>();
            cooldownManager = player.GetComponent<CoolDownManager>();
            store.storeUpdated += UpdateIcon;
        }

        private void Update()
        { 
            if (GetItem() != null) 
            {
                coolDownEffect.GetComponent<Image>().fillAmount = cooldownManager.GetFractionRemaining(GetItem().GetItemID());
            }
        }

        // PUBLIC

        public void AddItems(InventoryItem item, int number)
        {
            store.AddAction(item, index, number);
        }

        public InventoryItem GetItem()
        {
            return store.GetAction(index);
        }

        public int GetNumber()
        {
            return store.GetNumber(index);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return store.MaxAcceptable(item, index);
        }

        public void RemoveItems(int number)
        {
            store.RemoveItems(index, number);
        }

        // PRIVATE

        void UpdateIcon()
        {
            icon.SetItem(GetItem(), GetNumber());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var player = GameObject.FindWithTag("Player");
            store.Use(index, player);
        }
    }
}
