using Types;
using UnityEngine;
using UnityEngine.UI;
using Views.General;

namespace Controllers.Scenes
{
    public class MenuSceneController : AbstractSceneController
    {
        [SerializeField] private Button _habitsBtn;
        [SerializeField] private Button _rewardsBtn;
        [SerializeField] private Button _shopBtn;
        [SerializeField] private Button _privacyBtn;
        [SerializeField] private PanelView _privacyPanel;
        
        protected override void OnSceneEnable()
        {
            
        }

        protected override void OnSceneStart()
        {
            
        }

        protected override void OnSceneDisable()
        {
            
        }

        protected override void Initialize()
        {
            
        }

        protected override void Subscribe()
        {
            _habitsBtn.onClick.AddListener(OnPressHabitsBtn);
            _rewardsBtn.onClick.AddListener(OnPressRewardsBtn);
            _shopBtn.onClick.AddListener(OnPressShopBtn);
            _privacyBtn.onClick.AddListener(OpenPrivacyPanel);
        }

        protected override void Unsubscribe()
        {
            _habitsBtn.onClick.RemoveAllListeners();
            _rewardsBtn.onClick.RemoveAllListeners();
            _shopBtn.onClick.RemoveAllListeners();
            _privacyBtn.onClick.RemoveAllListeners();
        }

        private void OnPressHabitsBtn()
        {
            base.LoadScene(SceneType.HabitsScene);
        }

        private void OnPressRewardsBtn()
        {
            base.LoadScene(SceneType.RewardsScene);
        }

        private void OnPressShopBtn()
        {
            base.LoadScene(SceneType.ShopScene);
        }

        private void OpenPrivacyPanel()
        {
            _privacyPanel.PressBtnAction += OnReceiveAnswerPrivacyPanel;
            
            _privacyPanel.Open();
        }

        private void OnReceiveAnswerPrivacyPanel(int answer)
        {
            _privacyPanel.PressBtnAction -= OnReceiveAnswerPrivacyPanel;
            
            _privacyPanel.Close();
        }
    }
}