using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using UnityEngine;

namespace RPG.UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] GameObject soundImageMute;
        [SerializeField] GameObject musicImageMute;
        [SerializeField] GameObject soundImage;
        [SerializeField] GameObject musicImage;

        bool soundIsActive = true;
        bool musicIsActive = true;

        SavingWrapper savingWrapper;

        private void Awake() 
        {
            savingWrapper = FindObjectOfType<SavingWrapper>(); 
        }

        public void MuteSound()
        {
            soundIsActive = !soundIsActive;
            if (soundIsActive)
            {
                soundImage.SetActive(true);
                soundImageMute.SetActive(false);
            }
            else
            {
                soundImage.SetActive(false);
                soundImageMute.SetActive(true);
            }
        }

        public void MuteMusic()
        {
            musicIsActive = !musicIsActive;
            if (musicIsActive)
            {
                musicImage.SetActive(true);
                musicImageMute.SetActive(false);
            }
            else
            {
                musicImage.SetActive(false);
                musicImageMute.SetActive(true);
            }
        }

        public void LoadMainMenu()
        {
            savingWrapper.LoadMainMenu();
            gameObject.SetActive(false);
        }

        public void SaveGame()
        {
            savingWrapper.Save();
        }

        public void DeleteGame()
        {
            savingWrapper.Delete();
        }

        public void LoadGame()
        {
            savingWrapper.LoadLastSave();
        }

    }
}
