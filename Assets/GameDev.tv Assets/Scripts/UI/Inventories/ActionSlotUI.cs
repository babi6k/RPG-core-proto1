using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
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
        [SerializeField] GameObject coolDownEffect;

        // CACHE
        ActionStore store;
        bool isInCooldown = false;
        float timeSinceLastCoolDown = Mathf.Infinity;
        float coolDownTime = 0;

        // LIFECYCLE METHODS
        private void Awake()
        {
            store = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionStore>();
            store.storeUpdated += UpdateIcon;
            store.OnCoolDownApplied += UpdateCoolDown;
        }

        private void Update()
        {
            if (timeSinceLastCoolDown < coolDownTime)
            {
                coolDownEffect.GetComponent<Image>().fillAmount = timeSinceLastCoolDown / coolDownTime;
                isInCooldown = true;
            }
            else
            {
                coolDownEffect.GetComponent<Image>().fillAmount = 0;
                isInCooldown = false;
            }
            timeSinceLastCoolDown += Time.deltaTime;
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

        void UpdateCoolDown(int index, float cooldownTime)
        {
            if (index == this.index)
            {
                coolDownEffect.GetComponent<Image>().fillAmount = 1;
                if (timeSinceLastCoolDown > cooldownTime)
                {
                    timeSinceLastCoolDown = 0;
                    this.coolDownTime = cooldownTime;
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var player = GameObject.FindWithTag("Player");
            store.Use(index, player);
        }
    }
}
