using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using RPG.Control;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] CharacterSelector character;
    [SerializeField] SaveMenu saveMenu;
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float fadeWaitTime = 0.5f;
    [SerializeField] int sceneToLoad = 2;

    SavingWrapper savingWrapper;
    Fader fader;
    // Start is called before the first frame update
    void Awake()
    {
        savingWrapper = FindObjectOfType<SavingWrapper>();
        fader = FindObjectOfType<Fader>();
        character = character.GetComponent<CharacterSelector>();
        saveMenu = saveMenu.GetComponent<SaveMenu>();
    }

    public void StartNewGame()
    {
        StartCoroutine(LoadNewGame());
    }

    IEnumerator LoadNewGame()
    {
        if (sceneToLoad < 0)
        {
            Debug.LogError("Scene to load not set.");
            yield break;
        }

        DontDestroyOnLoad(transform.root.gameObject);
        PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.enabled = false;
        //Remove control player

        yield return fader.FadeOut(fadeOutTime);

        savingWrapper.NewSaveFile(saveMenu.GetSlotIndex());

        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        newPlayerController.enabled = false;
        //Change Player Character
        Vector3 startPos = newPlayerController.GetStartPos();
        savingWrapper.LoadFromMenu(saveMenu.GetSlotIndex());
        newPlayerController.GetComponent<NavMeshAgent>().Warp(startPos);
        newPlayerController.GetComponent<CharacterSelector>().ChangeCharacterModel(character.GetCurrentIndex());
        yield return new WaitForSeconds(fadeWaitTime);
        fader.FadeIn(fadeInTime);
        newPlayerController.enabled = true;
        savingWrapper.NewSaveFile(saveMenu.GetSlotIndex());
        Destroy(gameObject); 
    }
}
