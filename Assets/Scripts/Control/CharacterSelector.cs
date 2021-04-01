using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] GameObject [] characterList;
        [SerializeField] GameObject defaultCharacter;
        [SerializeField] bool isChoosingCharacter;

        int currentIndex;

        public void ChangeCharacterModel(int index)
        {
            currentIndex = index;
            defaultCharacter.SetActive(false);
            Debug.Log("I am changing Model");
            characterList[index].SetActive(true);
            defaultCharacter = characterList[index];
        }

        public void ActiveCharacter(bool active)
        {
            defaultCharacter.SetActive(active);
        }

        public int GetCurrentIndex() {return currentIndex; }
    }
