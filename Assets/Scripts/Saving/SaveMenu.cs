using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.SceneManagement;
using TMPro;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    [SerializeField] GameObject characterPanel;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] Transform player;
    [SerializeField] TextMeshProUGUI [] savefilesNames;

    int slotIndex = 1;
    const string savefileName = "SaveSlot";
    SavingWrapper savingWrapper;

    private void Awake() 
    {
        savingWrapper = FindObjectOfType<SavingWrapper>();
    }

    private void Start() 
    {
        for (int i = 0; i < 3; i++)
        {
            if (savingWrapper.FileExists(i+1))
            {
                savefilesNames[i].text = "Continue";
            }
            else
            {
                savefilesNames[i].text = savefileName + (i+ 1);
            }
        }
    }

    public void ChooseCharacter(int index)
    {
        if (savingWrapper.FileExists(index))
        {
            savingWrapper.SetSlotIndex(index);
            savingWrapper.LoadLastSave();
            CloseUI();
            return;
        }

        slotIndex = index;
        gameObject.SetActive(false);
        characterPanel.SetActive(true);
        backButton.SetActive(true);
        startButton.SetActive(true);
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
        characterPanel.SetActive(false);
        backButton.SetActive(false);
        startButton.SetActive(false);
        quitButton.SetActive(false);
    }

    public void OpenSaveMenu()
    {
        gameObject.SetActive(true);
        characterPanel.SetActive(false);
        backButton.SetActive(false);
        startButton.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public int GetSlotIndex() {return slotIndex;}
}
