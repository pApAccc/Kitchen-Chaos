using Common.SavingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class MusicManager : MonoBehaviour, ISaveable
    {
        public static MusicManager Instance { get; private set; }
        private AudioSource audioSource;
        private float volume;
        private void Awake()
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            volume = audioSource.volume;
        }

        public float GetVolume()
        {
            return volume;
        }
        public void ChangeVolume()
        {
            volume += .1f;
            if (volume > 1.02) { volume = 0; }
            audioSource.volume = volume;
        }

        public object CaptureState()
        {
            return volume;
        }

        public void RestoreState(object state)
        {
            volume = (float)state;
            audioSource.volume = volume;
        }
    }
}
