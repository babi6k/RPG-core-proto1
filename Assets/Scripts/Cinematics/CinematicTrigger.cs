using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using GameDevTV.Saving;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour,ISaveable
    {
        bool alreadyTriggerd = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && !alreadyTriggerd)
            {
                GetComponent<PlayableDirector>().Play();
                alreadyTriggerd = true;
            }
        }

        public object CaptureState()
        {
            return alreadyTriggerd;
        }

        public void RestoreState(object state)
        {
            alreadyTriggerd = (bool)state;
        }
    }
}
