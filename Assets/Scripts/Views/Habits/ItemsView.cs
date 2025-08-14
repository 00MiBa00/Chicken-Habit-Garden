using System.Collections.Generic;
using UnityEngine;

namespace Views.Habits
{
    public class ItemsView : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _items;

        public void SetItems(int index)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                bool active = i <= index;
                
                _items[i].SetActive(active);
            }
        }
    }
}