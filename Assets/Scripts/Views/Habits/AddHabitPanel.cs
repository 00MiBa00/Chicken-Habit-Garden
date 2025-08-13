using System;
using UnityEngine;
using UnityEngine.UI;
using Views.General;

namespace Views.Habits
{
    public class AddHabitPanel : PanelView
    {
        [SerializeField] private Button _saveBtn;
        [SerializeField] private InputField _inputField;

        private string _savedName;
        public event Action<string> OnPressSaveBtnAction;

        private void Awake()
        {
            _saveBtn.onClick.AddListener(OnPressSaveBtn);
            _inputField.onEndEdit.AddListener(OnInputFieldChanged);
        }

        private void OnDestroy()
        {
            _saveBtn.onClick.RemoveAllListeners();
            _inputField.onEndEdit.RemoveAllListeners();
        }

        private void OnInputFieldChanged(string input)
        {
            bool saveBtnActive = !string.IsNullOrWhiteSpace(input);
            
            _saveBtn.interactable = saveBtnActive;

            if (saveBtnActive)
            {
                _savedName = input;
            }
        }

        private void OnPressSaveBtn()
        {
            OnPressSaveBtnAction?.Invoke(_savedName);
        }
    }
}