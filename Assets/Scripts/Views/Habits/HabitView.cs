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

        public void SetInfo(HabitData data)
        {
            _nameText.text = data.Name;
            _btn.interactable = !data.IsСompleted;
            _checkmark.SetActive(data.IsСompleted);
        }
    }
}