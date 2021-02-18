using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] Dialogue currentDialogue = null;
        [SerializeField] string conversantName;

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRayCast(PlayerController callingController)
        {
            if (currentDialogue == null)
            {
                return false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<PlayerConversant>().StartDialogue(this,currentDialogue);
            }
            return true;
        }

        public string GetConversantName()
        {
            return conversantName;
        }
    }
}
