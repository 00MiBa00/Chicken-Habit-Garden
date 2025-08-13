using UnityEngine;
using UnityEngine.UI;

namespace Views.General
{
    public class TextUpdater : MonoBehaviour
    {
        [SerializeField] private Text _text;

        protected void UpdateText(string text)
        {
            _text.text = text;
        }
    }
}