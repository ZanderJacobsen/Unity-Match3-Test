using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] AudioClip click;
        [SerializeField] AudioClip deselect;

        [SerializeField] AudioClip match;
        [SerializeField] AudioClip dud;

        [SerializeField] AudioClip woosh;
        [SerializeField] AudioClip pop;

        AudioSource audioSource;

        void OnValidate()
        {
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
        }

        public void PlayClick() => audioSource.PlayOneShot(click);
        public void PlayDeselect() => audioSource.PlayOneShot(deselect);

        public void PlayMatch() => audioSource.PlayOneShot(match);
        public void PlayDud() => audioSource.PlayOneShot(dud);

        public void PlayWoosh() => PlayRandomPitch(woosh);
        public void PlayPop() => PlayRandomPitch(pop);

        private void PlayRandomPitch(AudioClip audioClip)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(audioClip);
            audioSource.pitch = 1.0f;
        }
    }
}
