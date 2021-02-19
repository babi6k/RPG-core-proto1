﻿using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] Quest[] tempQuests;
        [SerializeField] QuestItemUI questPrefab;
        // Start is called before the first frame update
        void Start()
        {
           foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }
            foreach (Quest quest in tempQuests)
            {
               QuestItemUI uiInstance = Instantiate<QuestItemUI>(questPrefab,transform);
               uiInstance.Setup(quest);
            }
        }
    }
}
