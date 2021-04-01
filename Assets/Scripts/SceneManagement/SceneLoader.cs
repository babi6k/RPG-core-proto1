using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using RPG.Control;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] CharacterSelector character;
    [SerializeField] SaveMenu saveMenu;
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float fadeWaitTime = 0.5f;
    [SerializeField] int sceneToLoad = 2;

    SavingSystem savingSystem;
    SavingWrapper savingWrapper;
    Fader fader;
    // Start is called before the first frame update
    void Start()
    {
        savingSystem = GetComponent<SavingSystem>();
        savingWrapper = GetComponent<SavingWrapper>();
        fader = GetComponent<Fader>();
        character = character.GetComponent<CharacterSelector>();
        saveMenu = saveMenu.GetComponent<SaveMenu>();
    }

    void StartNewGame()
    {

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

        savingWrapper.Save();

        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        newPlayerController.enabled = false;
        //Remove control player
        Vector3 startPos = newPlayerController.GetStartPos();
        savingWrapper.Load();
        newPlayerController.transform.position = startPos;
        
    }
}
