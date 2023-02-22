using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class OptionsUI : MonoBehaviour
    {
        public static OptionsUI Instance { get; private set; }

        [SerializeField] private Button musicBtn;
        [SerializeField] private Button soundEffectBtn;
        [SerializeField] private Button closeBtn;
        [SerializeField] private Button moveUpBtn;
        [SerializeField] private Button moveDownBtn;
        [SerializeField] private Button moveLeftBtn;
        [SerializeField] private Button moveRightBtn;
        [SerializeField] private Button interactBtn;
        [SerializeField] private Button interactAlternateBtn;
        [SerializeField] private Button pauseBtn;

        [SerializeField] private TextMeshProUGUI musicText;
        [SerializeField] private TextMeshProUGUI soundEffectText;
        [SerializeField] private TextMeshProUGUI moveUpText;
        [SerializeField] private TextMeshProUGUI moveDownText;
        [SerializeField] private TextMeshProUGUI moveLeftText;
        [SerializeField] private TextMeshProUGUI moveRightText;
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private TextMeshProUGUI interactAlternateText;
        [SerializeField] private TextMeshProUGUI pauseText;

        [SerializeField] private GameObject waittingBindingUI;
        private void Awake()
        {
            Instance = this;

            musicBtn.onClick.AddListener(() =>
            {
                MusicManager.Instance.ChangeVolume();
                UpdateVisual();
            });
            soundEffectBtn.onClick.AddListener(() =>
            {
                SFXManager.Instance.ChangeVolume();
                UpdateVisual();
            });
            closeBtn.onClick.AddListener(() =>
            {
                Hide();
            });

            moveUpBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up); });
            moveDownBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down); });
            moveLeftBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left); });
            moveRightBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right); });
            interactBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
            interactAlternateBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate); });
            pauseBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });

        }
        private void Start()
        {
            UpdateVisual();

            GameInput.Instance.OnPause += GameInput_OnPause;
            Hide();
            HideWaittingBindingUI();
        }

        private void GameInput_OnPause(object sender, System.EventArgs e)
        {
            Hide();
        }

        private void UpdateVisual()
        {
            soundEffectText.text = "游戏音效:" + Mathf.RoundToInt(SFXManager.Instance.GetVolume() * 10).ToString();
            musicText.text = "游戏声音:" + Mathf.RoundToInt(MusicManager.Instance.GetVolume() * 10).ToString();

            moveUpText.text = GameInput.Instance.GetBindingKey(GameInput.Binding.Move_Up);
            moveDownText.text = GameInput.Instance.GetBindingKey(GameInput.Binding.Move_Down);
            moveLeftText.text = GameInput.Instance.GetBindingKey(GameInput.Binding.Move_Left);
            moveRightText.text = GameInput.Instance.GetBindingKey(GameInput.Binding.Move_Right);
            interactText.text = GameInput.Instance.GetBindingKey(GameInput.Binding.Interact);
            interactAlternateText.text = GameInput.Instance.GetBindingKey(GameInput.Binding.InteractAlternate);
            pauseText.text = GameInput.Instance.GetBindingKey(GameInput.Binding.Pause);
        }

        public void Show()
        {
            gameObject.SetActive(true);

        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void ShowWaittingBindingUI()
        {
            waittingBindingUI.SetActive(true);
        }

        private void HideWaittingBindingUI()
        {
            waittingBindingUI.SetActive(false);
        }

        private void RebindBinding(GameInput.Binding binding)
        {
            ShowWaittingBindingUI();
            GameInput.Instance.ReBindBinding(binding, () => { HideWaittingBindingUI(); UpdateVisual(); });
        }

    }
}
