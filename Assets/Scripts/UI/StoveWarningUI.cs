using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class StoveWarningUI : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;
        private float warningTimer;
        private float warningTimerMax = .2f;
        private void Awake()
        {
            warningTimer = warningTimerMax;
        }
        private void Start()
        {
            stoveCounter.OnProgressBarChanged += StoveCounter_OnProgressBarChanged;
            Hide();
        }

        private void StoveCounter_OnProgressBarChanged(object sender, IHasProgress.OnProgressBarChangedEventArgs e)
        {
            bool show = (e.progressNormalized > .5f) && stoveCounter.IsFired();
            if (show)
            {
                Show();

                warningTimer -= Time.deltaTime;
                if (warningTimer <= 0)
                {
                    SFXManager.Instance.PlayStoveWarning();
                    warningTimer = warningTimerMax;
                }
            }
            else
            {
                Hide();
            }
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
