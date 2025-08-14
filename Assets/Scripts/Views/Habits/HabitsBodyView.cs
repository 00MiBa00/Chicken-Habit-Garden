using System;
using System.Collections.Generic;
using Datas.Habits;
using UnityEngine;

namespace Views.Habits
{
    public class HabitsBodyView : MonoBehaviour
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private RectTransform _container;

        private List<GameObject> _activeItems;
        public event Action<int> OnCompletedHabitAction;

        public void SetInfo(HabitsData data)
        {
            ClearActiveItems();

            _activeItems ??= new List<GameObject>();

            foreach (var habit in data.Habits)
            {
                GameObject go = Instantiate(_itemPrefab, _container);
                HabitView view = go.GetComponent<HabitView>();
                view.OnPressBtnAction += OnPressHabitToggle;
                
                _activeItems.Add(go);
            
                view.SetInfo(habit);
            }
        }

        private void ClearActiveItems()
        {
            if (_activeItems is { Count: > 0 })
            {
                foreach (var item in _activeItems)
                {
                    HabitView view = item.GetComponent<HabitView>();
                    view.OnPressBtnAction -= OnPressHabitToggle;
                    Destroy(item);
                }
                
                _activeItems.Clear();
            }
        }

        private void OnPressHabitToggle(HabitView view)
        {
            view.OnPressBtnAction -= OnPressHabitToggle;

            int index = _activeItems.IndexOf(view.gameObject);
            
            Debug.Log($"Index {index}");
            
            OnCompletedHabitAction?.Invoke(index);
        }
    }
}