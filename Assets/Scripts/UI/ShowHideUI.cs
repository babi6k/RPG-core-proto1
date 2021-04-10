using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTV.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] GameObject uiContainer = null;

        // Start is called before the first frame update
        void Start()
        {
            uiContainer.SetActive(false);
        }

        // Update is called once per frame

        public void Toggle()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
        }
    }
}