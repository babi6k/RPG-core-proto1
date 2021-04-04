using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.SceneManagement;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    [SerializeField] GameObject characterPanel;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject startButton;
    [SerializeField] Transform player;

    int slotIndex = 1;
    const string savefileName = "SaveSlot";
    SavingWrapper savingWrapper;

    private void Awake() 
    {
        savingWrapper = FindObjectOfType<SavingWrapper>();
    }

    public void ChooseCharacter(int index)
    {
        if (savingWrapper.FileExists(index))
        {
            savingWrapper.SetSlotIndex(index);
        }

        slotIndex = index;
        gameObject.SetActive(false);
        characterPanel.SetActive(true);
        backButton.SetActive(true);
        startButton.SetActive(true);
    }

    public void OpenSaveMenu()
    {
        gameObject.SetActive(true);
        characterPanel.SetActive(false);
        backButton.SetActive(false);
        startButton.SetActive(false);
    }

    public int GetSlotIndex() {return slotIndex;}
}
