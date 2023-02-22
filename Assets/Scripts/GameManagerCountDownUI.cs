using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class GameManagerCountDownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countDowntext;
        [SerializeField] private Animator animator;
        private int previousNum;

        private void Start()
        {
            GameManager.Instance.OnStateChanged += GameMnagaer_OnStateChanged;
            Hide();
        }

        private void GameMnagaer_OnStateChanged(object sender, System.EventArgs e)
        {
            if (GameManager.Instance.IsCountDownToStart())
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
        private void Update()
        {
            int countDownNum = Mathf.CeilToInt(GameManager.Instance.GetCountDownToStartTimer());
            countDowntext.text = countDownNum.ToString();
            if (previousNum != countDownNum)
            {
                animator.SetTrigger("NumberPopup");
                SFXManager.Instance.PlayCountDownSFX();
                previousNum = countDownNum;
            }

        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
