using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;
using System;

namespace RPG.SceneManagement
{

    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "Autosave";
        const string newSaveFile = "SaveSlot";
        [SerializeField] float fadeInTime = 0.2f;

        int saveSlotIndex = 0;

        IEnumerator LoadLastScene()
        {
            Debug.Log("LoadingLastScene");
            yield return GetComponent<SavingSystem>().LoadLastScene(newSaveFile + saveSlotIndex);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Delete();
            }

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

        public bool FileExists(int index)
        {
            return GetComponent<SavingSystem>().FileExists(newSaveFile + index);
        }
    }
}
