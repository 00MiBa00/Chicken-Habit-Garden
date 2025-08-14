using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Types;

namespace Views.Shop
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Image _bg;
        [SerializeField] private List<Sprite> _bgSprites;
        [SerializeField] private Button _btn;
        [SerializeField] private GameObject _cost;
        [SerializeField] private GameObject _select;
        [SerializeField] private GameObject _buy;

        public event Action<ShopItemView> OnPressBtnAction;

        private void OnEnable()
        {
            _btn.onClick.AddListener(OnPressBtn);
        }

        private void OnDisable()
        {
            _btn.onClick.RemoveAllListeners();
        }

        public void SetState(ShopItemType type)
        {
            switch (type)
            {
                case ShopItemType.Selected:
                    SetSelectedState();
                    break;
                case ShopItemType.CanBuy:
                    SetCanBuyState();
                    break;
                case ShopItemType.CanSelect:
                    SetCanSelectState();
                    break;
                case ShopItemType.NoMoney:
                    SetNoMoneyState();
                    break;
            }
        }

        private void SetSelectedState()
        {
            _btn.interactable = true;
            _cost.SetActive(false);
            _select.SetActive(false);
            _buy.SetActive(false);

            _bg.sprite = _bgSprites[0];
        }

        private void SetCanBuyState()
        {
            _btn.interactable = true;
            _cost.SetActive(true);
            _select.SetActive(false);
            _buy.SetActive(true);

            _bg.sprite = _bgSprites[1];
        }

        private void SetCanSelectState()
        {
            _btn.interactable = true;
            _cost.SetActive(false);
            _select.SetActive(true);
            _buy.SetActive(false);

            _bg.sprite = _bgSprites[1];
        }

        private void SetNoMoneyState()
        {
            _btn.interactable = true;
            _cost.SetActive(true);
            _select.SetActive(false);
            _buy.SetActive(false);

            _bg.sprite = _bgSprites[1];
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