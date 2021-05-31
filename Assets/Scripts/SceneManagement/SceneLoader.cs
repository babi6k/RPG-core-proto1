using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using RPG.Control;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{

    public class SceneLoader : MonoBehaviour
    {
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
            saveMenu = saveMenu.GetComponent<SaveMenu>();
        }

        public void StartNewGame()
        {
            StartCoroutine(LoadNewGame());
        }

        IEnumerator LoadNewGame()
        {
            DontDestroyOnLoad(transform.root.gameObject);
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;
            //Remove control player
            saveMenu.CloseUI();
            yield return fader.FadeOut(fadeOutTime);

            savingWrapper.SetSlotIndex(saveMenu.GetSlotIndex());
            savingWrapper.NewSaveFile(saveMenu.GetSlotIndex());

            //Load new Scene
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            savingWrapper.LoadFromMenu(saveMenu.GetSlotIndex());
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;

            //Change Player Character
            Vector3 startPos = newPlayerController.GetStartPos();
            newPlayerController.GetComponent<NavMeshAgent>().Warp(startPos);
            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);
            newPlayerController.enabled = true;
            savingWrapper.SetSlotIndex(saveMenu.GetSlotIndex());
            savingWrapper.NewSaveFile(saveMenu.GetSlotIndex());
            Destroy(gameObject);
        }
    }
}
