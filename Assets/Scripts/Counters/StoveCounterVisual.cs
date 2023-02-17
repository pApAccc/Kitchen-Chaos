using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class StoveCounterVisual : MonoBehaviour
    {
        [SerializeField] Transform stoveOnVisual;
        [SerializeField] ParticleSystem sizzlingParticle;
        private StoveCounter stoveCounter;
        private void Awake()
        {
            stoveCounter = GetComponent<StoveCounter>();
        }
        private void Start()
        {
            stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        }

        private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
        {
            bool active = e.state == StoveCounter.State.Fring || e.state == StoveCounter.State.Fired;

            stoveOnVisual.gameObject.SetActive(active);
            sizzlingParticle.gameObject.SetActive(active);
        }

    }
}
