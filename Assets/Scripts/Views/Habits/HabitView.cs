using System;
using Datas.Habits;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Habits
{
    public class HabitView : MonoBehaviour
    {
        [SerializeField] private Button _btn;
        [SerializeField] private GameObject _checkmark;
        [SerializeField] private Text _nameText;

        public event Action<HabitView> OnPressBtnAction;

        private void OnEnable()
        {
            _btn.onClick.AddListener(OnPressBtn);
        }

        private void OnDisable()
        {
            _btn.onClick.RemoveAllListeners();
        }

        public void SetInfo(HabitData data)
        {
            _nameText.text = data.Name;
            _btn.interactable = !data.IsСompleted;
            _checkmark.SetActive(data.IsСompleted);
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