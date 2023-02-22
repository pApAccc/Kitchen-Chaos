using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class StoveWarningBarUI : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;
        private Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            stoveCounter.OnProgressBarChanged += StoveCounter_OnProgressBarChanged;
        }
        private void StoveCounter_OnProgressBarChanged(object sender, IHasProgress.OnProgressBarChangedEventArgs e)
        {
            bool show = (e.progressNormalized > .5f) && stoveCounter.IsFired();
            if (show)
            {
                animator.SetBool("IsFired", true);
            }
            else
            {
                animator.SetBool("IsFired", false);
            }
        }
    }
}
