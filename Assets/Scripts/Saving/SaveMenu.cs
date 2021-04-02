using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    [SerializeField] GameObject characterPanel;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject startButton;
    [SerializeField] Transform player;

    int slotIndex = 1;
    const string savefileName = "saveSlot";
    CharacterSelector character;

    private void Start() 
    {
        character = player.GetComponent<CharacterSelector>();
    }

    public void ChooseCharacter(int index)
    {
        slotIndex = index;
        gameObject.SetActive(false);
        characterPanel.SetActive(true);
        backButton.SetActive(true);
        startButton.SetActive(true);
        character.ActiveCharacter(true);
    }

    public void OpenSaveMenu()
    {
        gameObject.SetActive(true);
        characterPanel.SetActive(false);
        backButton.SetActive(false);
        startButton.SetActive(false);
        character.ActiveCharacter(false);
    }

    public int GetSlotIndex() {return slotIndex;}
}
