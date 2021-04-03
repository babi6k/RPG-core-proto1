using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;

public class CharacterSelector : MonoBehaviour, ISaveable
{
    [SerializeField] List<Transform> models = new List<Transform>();
    int currentCharacter = 0;

    void Awake()
    {
        ApplyCurrentCharacter(); //default to character 0 until set or restoreState
    }

    public void SetCurrentCharacter(int character)
    {
        currentCharacter = character;
        ApplyCurrentCharacter();
    }

    void ApplyCurrentCharacter()
    {
        Debug.Log($"ApplyCurrentCharacter() = {currentCharacter}");
        foreach (Transform model in models)
        {
            model.gameObject.SetActive(false);
        }
        models[currentCharacter].gameObject.SetActive(true);
    }

    public object CaptureState()
    {
        Debug.Log("i am now saving the CurrentCharacter");
        Debug.Log($"CaptureState() = {currentCharacter}");
        return currentCharacter;
    }

    public void RestoreState(object state)
    {
        Debug.Log("i am now loading the CurrentCharacter");
        currentCharacter = (int)state;
        Debug.Log($"RestoreState() = {currentCharacter}");
        ApplyCurrentCharacter();
     }
}
