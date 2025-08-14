using Types;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Scenes
{
    public class MenuSceneController : AbstractSceneController
    {
        [SerializeField] private Button _habitsBtn;
        [SerializeField] private Button _rewardsBtn;
        [SerializeField] private Button _shopBtn;
        
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
        }

        protected override void Unsubscribe()
        {
            _habitsBtn.onClick.RemoveAllListeners();
            _rewardsBtn.onClick.RemoveAllListeners();
            _shopBtn.onClick.RemoveAllListeners();
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
    }
}