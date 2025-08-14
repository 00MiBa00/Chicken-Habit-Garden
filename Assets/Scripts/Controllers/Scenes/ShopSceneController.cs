using System.Collections.Generic;
using DG.Tweening;
using Models.Scenes;
using Types;
using UnityEngine;
using UnityEngine.UI;
using Values;
using Views.Shop;

namespace Controllers.Scenes
{
    public class ShopSceneController : AbstractSceneController
    {
        [SerializeField] private List<ShopItemView> _shopItemViews;
        [SerializeField] private GameObject _notPanel;
        [SerializeField] private Text _textGameObject;

        private ShopSceneModel _model;
        private Sequence _seq;
        
        protected override void OnSceneEnable()
        {
            UpdateStates();
        }

        protected override void OnSceneStart()
        {
            
        }

        protected override void OnSceneDisable()
        {
           
        }

        protected override void Initialize()
        {
            _model = new ShopSceneModel();
        }

        protected override void Subscribe()
        {
            _shopItemViews.ForEach(item => item.OnPressBtnAction += OnPressShopItem);
        }

        protected override void Unsubscribe()
        {
            _shopItemViews.ForEach(item => item.OnPressBtnAction -= OnPressShopItem);
        }

        private void UpdateStates()
        {
            List<ShopItemType> states = new(_model.GetStateItems());

            int selectedIndex = _model.SelectedIndex;

            for (int i = 0; i < _shopItemViews.Count; i++)
            {
                if (selectedIndex == i)
                {
                    _shopItemViews[i].SetState(ShopItemType.Selected);
                }
                else
                {
                    _shopItemViews[i].SetState(states[i]);
                }
            }
        }

        private void OnPressShopItem(ShopItemView view)
        {
            int index = _shopItemViews.IndexOf(view);

            ShopItemType state;

            state = index == _model.SelectedIndex ? ShopItemType.Selected : _model.GetItemState(index);
            
            switch (state)
            {
                case ShopItemType.Selected:
                    _textGameObject.text = "Duplicate pick — already selected";
                    PlayAnimNotification();
                    break;
                case ShopItemType.CanBuy:
                    Wallet.TryPurchase(150);
                    _model.UpdateItemState(index);
                    UpdateStates();
                    break;
                case ShopItemType.CanSelect:
                    _model.SelectedIndex = index;
                    UpdateStates();
                    break;
                case ShopItemType.NoMoney:
                    _textGameObject.text = "Not enough Eggs";
                    PlayAnimNotification();
                    break;
            }
        }
        
        private void PlayAnimNotification()
        {
            _seq?.Kill();
            
            _notPanel.transform.localScale = Vector3.zero;
            _textGameObject.transform.localScale = Vector3.zero;

            _seq = DOTween.Sequence()
                .Append(_notPanel.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack))
                .Append(_textGameObject.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutBack))
                .AppendInterval(2)
                .Append(_textGameObject.transform.DOScale(Vector3.zero, 1).SetEase(Ease.InBack))
                .Append(_notPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack));
        }
    }
}