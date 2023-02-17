using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private GameObject gameobjectHasProgress;
        private IHasProgress hasProgress;

        private void Start()
        {
            hasProgress = gameobjectHasProgress.GetComponent<IHasProgress>();
            if (hasProgress == null)
            {
                Debug.LogError("gameobjectHasProgress 没有实现接口 IHasProgress");
            }

            hasProgress.OnProgressBarChanged += HasProgress_OnProgressBarChanged;
            image.fillAmount = 0;
            SetActive(false);
        }

        private void HasProgress_OnProgressBarChanged(object sender, IHasProgress.OnProgressBarChangedEventArgs e)
        {
            image.fillAmount = e.progressNormalized;

            if (Mathf.Approximately(image.fillAmount, 1) || image.fillAmount == 0)
            {
                SetActive(false);
            }
            else
            {
                SetActive(true);
            }
        }

        private void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }



    }
}
