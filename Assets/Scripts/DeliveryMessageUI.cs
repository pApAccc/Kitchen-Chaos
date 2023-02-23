using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class DeliveryMessageUI : MonoBehaviour
    {
        [SerializeField] private GameObject successfulUI;
        [SerializeField] private GameObject failedUI;

        private Animator animator;
        private float autoCloseUITimer;
        private float autoCloseUITimerMax = 2f;
        private GameObject showingUI;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            autoCloseUITimer = autoCloseUITimerMax;

        }
        private void Start()
        {
            DeliveryManager.Instance.OnWaittingRecipeSuccessed += DeliveryManager_OnWaittingRecipeSuccessed;
            DeliveryManager.Instance.OnWaittingRecipeFailed += DeliveryManager_OnWaittingRecipeFailed;

            successfulUI.gameObject.SetActive(false);
            failedUI.gameObject.SetActive(false);
            showingUI = null;
        }
        private void Update()
        {
            if (showingUI != null)
            {
                autoCloseUITimer -= Time.deltaTime;

                if (autoCloseUITimer <= 0)
                {
                    showingUI.SetActive(false);
                    showingUI = null;
                }
            }
        }

        private void DeliveryManager_OnWaittingRecipeFailed(object sender, System.EventArgs e)
        {
            SetUIState(failedUI);
        }

        private void DeliveryManager_OnWaittingRecipeSuccessed(object sender, System.EventArgs e)
        {
            SetUIState(successfulUI);
        }

        private void SetUIState(GameObject gameObjectUI)
        {
            gameObjectUI.gameObject.SetActive(true);
            animator.SetTrigger("Show");
            showingUI = gameObjectUI;

            autoCloseUITimer = autoCloseUITimerMax;
        }

    }
}
