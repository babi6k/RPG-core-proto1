using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;

public class CharacterSelector : MonoBehaviour, ISaveable
{
    [SerializeField] List<GameObject> models = new List<GameObject>();
    int currentCharacter = 0;
    GameObject currentModel;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        ApplyCurrentCharacter(); //default to character 0 until set or restoreState
    }

    public void SetCurrentCharacter(int character)
    {
        currentCharacter = character;
        ApplyCurrentCharacter();
    }

    void ApplyCurrentCharacter()
    {
       if (currentModel) Destroy(currentModel);
       currentModel = Instantiate(models[currentCharacter],transform);
       StartCoroutine(ResetAnimator());
    }

    IEnumerator ResetAnimator()
    {
       animator.enabled = false;
       currentModel.SetActive(false);
       yield return new WaitForSeconds(0.3f);
       animator.enabled = true;  
       currentModel.SetActive(true);
       animator.Rebind();
    }

    public object CaptureState()
    {
        return currentCharacter;
    }

    public void RestoreState(object state)
    {
        currentCharacter = (int)state;
        ApplyCurrentCharacter();
     }
}
