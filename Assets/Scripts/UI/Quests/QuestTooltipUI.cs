using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab; 

        public void Setup(Quest quest)
        {
            foreach (Transform item in objectiveContainer)
            {
                Destroy(item.gameObject);
            }
            title.text = quest.GetTitle();
            foreach (string objective in quest.GetObjectives())
            {
                GameObject objectiveInstance = Instantiate(objectivePrefab,objectiveContainer);
                var objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective;
            }
        }
    }
}
