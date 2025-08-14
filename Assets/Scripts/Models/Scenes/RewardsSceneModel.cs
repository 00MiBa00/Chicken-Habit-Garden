using System.Collections.Generic;
using Types;
using UnityEngine;

namespace Models.Scenes
{
    public class RewardsSceneModel
    {
        private const string RewardsKey = "RewardsSceneModel.Rewards";

        private int _streak;
        private int _selectedItem;

        public RewardsSceneModel(int streak)
        {
            _streak = streak;

            if (_streak == 0)
            {
                ResetStates();
            }
        }

        public List<RewardItemType> GetRewardStates()
        {
            List<RewardItemType> states = new(LoadStates());

            int[] thresholds = { 1, 3, 5, 10, 15, 20, 25, 30 };
            
            if (states.Count < thresholds.Length)
                states.AddRange(new RewardItemType[thresholds.Length - states.Count]);
            else if (states.Count > thresholds.Length)
                states.RemoveRange(thresholds.Length, states.Count - thresholds.Length);
            
            for (int i = 0; i < thresholds.Length; i++)
            {
                if (states[i] == RewardItemType.Completed)
                    continue;

                states[i] = (_streak >= thresholds[i])
                    ? RewardItemType.Active
                    : RewardItemType.Disable;
            }

            return states;
        }

        public void SetSelectedItem(int index)
        {
            _selectedItem = index;
        }

        public void SetRewardCompleted()
        {
            PlayerPrefs.SetInt(RewardsKey+_selectedItem, 1);
        }

        private void ResetStates()
        {
            for (int i = 0; i < 8; i++)
            {
                PlayerPrefs.DeleteKey(RewardsKey+i);
            }
        }

        private List<RewardItemType> LoadStates()
        {
            List<RewardItemType> states = new();
            
            for (int i = 0; i < 8; i++)
            {
                RewardItemType state = PlayerPrefs.GetInt(RewardsKey + i, 0) == 0 ? RewardItemType.Active : RewardItemType.Completed;
                
                states.Add(state);
            }

            return states;
        }
    }
}