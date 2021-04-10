using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using UnityEngine;

namespace RPG.UI
{
    public class DeadUI : MonoBehaviour
    {
        
        public void Reload()
        {
            FindObjectOfType<SavingWrapper>().LoadLastSave();
        }
    }
}
