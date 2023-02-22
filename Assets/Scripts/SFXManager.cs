using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Common.SavingSystem;
/// <summary>
/// 
/// </summary>
namespace ns
{
    public class SFXManager : MonoBehaviour, ISaveable
    {
        public static SFXManager Instance { get; private set; }
        [SerializeField] private AudioClipRefsSO audioClipRefsSO;
        private float volume = .7f;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            DeliveryManager.Instance.OnWaittingRecipeSuccessed += DeliveryManager_OnWaittingRecipeSuccessed;
            DeliveryManager.Instance.OnWaittingRecipeFailed += DeliveryManager_OnWaittingRecipeFailed;
            CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
            Player.Instance.OnPickSomething += PlayerOnPickSomething;
            BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
            TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
            PlayerSound.OnPlayerMoving += PlayerSound_OnPlayerMoving;
        }

        private void PlayerSound_OnPlayerMoving(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.footstep, Camera.main.transform.position);
        }

        private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.trash, Camera.main.transform.position);
        }

        private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.object_drop, Camera.main.transform.position);
        }

        private void PlayerOnPickSomething(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.object_pickup, Camera.main.transform.position);
        }

        private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.chop, Camera.main.transform.position);
        }

        private void DeliveryManager_OnWaittingRecipeFailed(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.delivery_fail, Camera.main.transform.position);
        }

        private void DeliveryManager_OnWaittingRecipeSuccessed(object sender, EventArgs e)
        {
            PlaySound(audioClipRefsSO.delivery_success, Camera.main.transform.position);
        }

        private void PlaySound(AudioClip clip, Vector3 position, float volumeMultipliter = 1)
        {
            AudioSource.PlayClipAtPoint(clip, position, volumeMultipliter * volume);
        }
        private void PlaySound(AudioClip[] clipArray, Vector3 position, float volumeMultipliter = 1)
        {
            PlaySound(clipArray[Random.Range(0, clipArray.Length)], position, volumeMultipliter * volume);
        }

        public void PlayCountDownSFX()
        {
            PlaySound(audioClipRefsSO.warning[1], Camera.main.transform.position);
        }
        public void PlayStoveWarning()
        {
            PlaySound(audioClipRefsSO.warning[0], Camera.main.transform.position);
        }

        public void ChangeVolume()
        {
            volume += .1f;
            if (volume > 1.02) { volume = 0; }
        }
        public float GetVolume()
        {
            return volume;
        }

        public object CaptureState()
        {
            return volume;
        }

        public void RestoreState(object state)
        {
            volume = (float)state;
        }
    }
}
