using UnityEngine;

namespace RPG.Audio
{

    public class PlaySounds : MonoBehaviour
    {
        [SerializeField] AudioClip[] audioClips = null;
        AudioSource audioSource = null;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayClips()
        {
            AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
