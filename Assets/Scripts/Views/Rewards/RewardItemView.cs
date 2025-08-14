using System;
using System.Collections.Generic;
using Types;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Rewards
{
    public class RewardItemView : MonoBehaviour
    {
        [SerializeField] private Button _btn;
        [SerializeField] private Image _bg;
        [SerializeField] private Text _description;

        [Space(5)] 
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _disableColor;
        [SerializeField] private Color _completedColor;
        [SerializeField] private Color _pressColor;
        [SerializeField] private List<Color> _descriptionColors;

        public event Action<RewardItemView> OnPressBtnAction;

        private void OnEnable()
        {
            _btn.onClick.AddListener(OnPressBtn);
        }

        private void OnDisable()
        {
            _btn.onClick.RemoveAllListeners();
        }

        public void SetState(RewardItemType type)
        {
            switch (type)
            {
                case RewardItemType.Active:
                    SetActiveState();
                    break;
                case RewardItemType.Completed:
                    SetCompletedState();
                    break;
                case RewardItemType.Disable:
                    SetDisableState();
                    break;
                case RewardItemType.Press:
                    SetPressState();
                    break;
            }
        }

        private void SetActiveState()
        {
            _bg.color = _activeColor;
            _description.color = _descriptionColors[0];
            _btn.interactable = true;
        }

        private void SetCompletedState()
        {
            _bg.color = _completedColor;
            _description.color = _descriptionColors[0];
            _btn.interactable = false;
        }

        private void SetDisableState()
        {
            _bg.color = _disableColor;
            _description.color = _descriptionColors[1];
            _btn.interactable = false;
        }

        private void SetPressState()
        {
            _bg.color = _pressColor;
            _description.color = _descriptionColors[1];
            _btn.interactable = false;
        }

        private void OnPressBtn()
        {
            Notification();
        }

        private void Notification()
        {
            OnPressBtnAction?.Invoke(this);
        }
    }
}