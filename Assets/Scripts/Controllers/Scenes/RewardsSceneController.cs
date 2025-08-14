using System.Collections.Generic;
using DG.Tweening;
using Models.Scenes;
using Types;
using UnityEngine;
using UnityEngine.UI;
using Values;
using Views.Rewards;

namespace Controllers.Scenes
{
    public class RewardsSceneController : AbstractSceneController
    {
        [SerializeField] private List<RewardItemView> _rewardItemViews;
        [SerializeField] private Button _collectBtn;
        [SerializeField] private GameObject _rewardObject;
        [SerializeField] private GameObject _rewardPanel;

        private HabitSceneModel _habitModel;
        private RewardsSceneModel _model;
        private Sequence _seq;
        
        protected override void OnSceneEnable()
        {
            SetRewardsStates();
        }

        protected override void OnSceneStart()
        {
            
        }

        protected override void OnSceneDisable()
        {
            _seq?.Kill();
        }

        protected override void Initialize()
        {
            _habitModel = new HabitSceneModel();
            _habitModel.Init();
            _model = new RewardsSceneModel(_habitModel.Streak);
        }

        protected override void Subscribe()
        {
            _rewardItemViews.ForEach(item => item.OnPressBtnAction += OnPressRewardItem);
            _collectBtn.onClick.AddListener(OnPressCollectBtn);
        }

        protected override void Unsubscribe()
        {
            _rewardItemViews.ForEach(item => item.OnPressBtnAction -= OnPressRewardItem);
            _collectBtn.onClick.RemoveAllListeners();
        }

        private void SetRewardsStates()
        {
            List<RewardItemType> states = new List<RewardItemType>(_model.GetRewardStates());
            
            for (int i = 0; i < _rewardItemViews.Count; i++)
            {
                _rewardItemViews[i].SetState(states[i]);
            }
        }

        private void OnPressRewardItem(RewardItemView view)
        {
            int index = _rewardItemViews.IndexOf(view);

            _collectBtn.interactable = true;
            _model.SetSelectedItem(index);
            
            _rewardItemViews[index].SetState(RewardItemType.Press);
        }

        private void OnPressCollectBtn()
        {
            _model.SetRewardCompleted();
            SetRewardsStates();
            
            Wallet.AddMoney(1);
            
            PlayAnimReward();
        }

        private void PlayAnimReward()
        {
            _seq?.Kill();
            
            _rewardPanel.transform.localScale = Vector3.zero;
            _rewardObject.transform.localScale = Vector3.zero;

            _seq = DOTween.Sequence()
                .Append(_rewardPanel.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack))
                .Append(_rewardObject.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutBack))
                .AppendInterval(2)
                .Append(_rewardObject.transform.DOScale(Vector3.zero, 1).SetEase(Ease.InBack))
                .Append(_rewardPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack));
        }
    }
}