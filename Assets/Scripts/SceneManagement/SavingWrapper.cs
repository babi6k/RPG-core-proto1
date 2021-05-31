using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;
using System;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{

    public class SavingWrapper : MonoBehaviour
    {
        const string newSaveFile = "SaveSlot";
        [SerializeField] float fadeInTime = 0.2f;
        [SerializeField] float fadeOutTime = 1f;

        int saveSlotIndex = 0;
        Fader fader;

        private void Awake() 
        {
            fader = FindObjectOfType<Fader>();
        }

        IEnumerator LoadLastScene()
        {
            Debug.Log("LoadingLastScene");
            Debug.Log("Save Slot index is : " + saveSlotIndex);
            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(newSaveFile + saveSlotIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        IEnumerator MainMenu()
        {
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(1);
            yield return fader.FadeIn(fadeInTime);
        }

        // Update is called once per frame
        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.L))
            // {
            //     Load();
            // }

            // if (Input.GetKeyDown(KeyCode.S))
            // {
            //     Save();
            // }

            // if (Input.GetKeyDown(KeyCode.D))
            // {
            //     Delete();
            // }

        }

        public void SetSlotIndex(int index)
        {
            saveSlotIndex = index;
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(newSaveFile + saveSlotIndex);
        }

        public void Load()
        {
            //Call to saving system to load
            GetComponent<SavingSystem>().Load(newSaveFile + saveSlotIndex);         
        }

        public void Save()
        {
            //Call to saving system to save
            GetComponent<SavingSystem>().Save(newSaveFile + saveSlotIndex);
        }

        public void NewSaveFile(int index)
        {
            GetComponent<SavingSystem>().Save(newSaveFile + index);
        }

        public void LoadFromMenu(int index)
        {
           GetComponent<SavingSystem>().Load(newSaveFile + index); 
        }

        public void LoadLastSave()
        {
            StartCoroutine(LoadLastScene());
        }

        public void LoadMainMenu()
        {
            StartCoroutine(MainMenu());
        }

        public bool FileExists(int index)
        {
            return GetComponent<SavingSystem>().FileExists(newSaveFile + index);
        }
    }
}
