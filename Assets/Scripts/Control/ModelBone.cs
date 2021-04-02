using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

public class ModelBone : MonoBehaviour
{
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;

    Fighter player;

    private void Awake() 
    {
        player = GameObject.FindWithTag("Player").GetComponent<Fighter>();

    }
    private void OnEnable() 
    {
        player.SetHandTransfroms(leftHand,rightHand);
        player.GetComponent<Animator>().Rebind();
    }
}