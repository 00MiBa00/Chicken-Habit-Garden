using System.Collections.Generic;
using Types;
using UnityEngine;
using Values;

namespace Models.Scenes
{
    public class ShopSceneModel
    {
        private const string StateKey = "ShopSceneModel.State";
        private const string SelectedIndexKey = "ShopSceneModel.SelectedIndex";

        public int SelectedIndex
        {
            get => PlayerPrefs.GetInt(SelectedIndexKey, 0);
            set => PlayerPrefs.SetInt(SelectedIndexKey, value);
        }

        public List<ShopItemType> GetStateItems()
        {
            List<ShopItemType> states = new List<ShopItemType>();

            for (int i = 0; i < 6; i++)
            {
                ShopItemType state = GetItemState(i);
                
                states.Add(state);
            }

            return states;
        }

        public ShopItemType GetItemState(int index)
        {
            ShopItemType state = (ShopItemType)PlayerPrefs.GetInt(StateKey + index, index > 0 ? (int)ShopItemType.CanBuy : (int)ShopItemType.CanSelect);

            if (state == ShopItemType.CanBuy && Wallet.Money < 150)
            {
                state = ShopItemType.NoMoney;
            }

            return state;
        }

        public void UpdateItemState(int index)
        {
            PlayerPrefs.SetInt(StateKey+index, (int)(ShopItemType.CanSelect));
        }
    }
}