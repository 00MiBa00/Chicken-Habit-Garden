using Models.Scenes;
using UnityEngine;
using UnityEngine.UI;
using Views.General;
using Views.Habits;

namespace Controllers.Scenes
{
    public class HabitsSceneController : AbstractSceneController
    {
        [Space(5)] 
        [SerializeField] private Button _addBtn;
        [SerializeField] private Button _backBtn;
        [SerializeField] private Button _resetStreakBtn;

        [Space(5)] 
        [SerializeField] private StreakView _streakView;
        [SerializeField] private GoalView _goalView;
        [SerializeField] private HabitsBodyView _habitsBodyView;

        [Space(5)] 
        [SerializeField] private PanelView _mainPanel;
        [SerializeField] private AddHabitPanel _addHabitPanel;
        
        private HabitSceneModel _model;
        
        protected override void OnSceneEnable()
        {
            UpdateStreak();
            UpdateGoal();
            SetHabits();
        }

        protected override void OnSceneStart()
        {
            
        }

        protected override void OnSceneDisable()
        {
            
        }

        protected override void Initialize()
        {
            _model = new HabitSceneModel();
            _model.Init();
        }

        protected override void Subscribe()
        {
            _addBtn.onClick.AddListener(OpenAddHabitPanel);
        }

        protected override void Unsubscribe()
        {
            _addBtn.onClick.RemoveAllListeners();
        }

        private void SetHabits()
        {
            _habitsBodyView.SetInfo(_model.Today);
        }

        private void UpdateStreak()
        {
            _streakView.UpdateStreak(_model.Streak);
        }

        private void UpdateGoal()
        {
            _goalView.UpdateGoal(_model.TodayCompletedCount, _model.TodayTotalCount);
        }

        private void OpenAddHabitPanel()
        {
            _addHabitPanel.PressBtnAction += OnReceiveAnswerAddHabitPanel;
            _addHabitPanel.OnPressSaveBtnAction += OnAddedNewHabit;
            
            ClosePanel(_mainPanel);
            OpenPanel(_addHabitPanel);
        }

        private void OnAddedNewHabit(string value)
        {
            _addHabitPanel.PressBtnAction -= OnReceiveAnswerAddHabitPanel;
            _addHabitPanel.OnPressSaveBtnAction -= OnAddedNewHabit;

            _model.AddHabit(value);
            
            UpdateGoal();
            SetHabits();
            
            ClosePanel(_addHabitPanel);
            OpenPanel(_mainPanel);
        }

        private void OnReceiveAnswerAddHabitPanel(int answer)
        {
            _addHabitPanel.PressBtnAction -= OnReceiveAnswerAddHabitPanel;
            _addHabitPanel.OnPressSaveBtnAction -= OnAddedNewHabit;
            
            ClosePanel(_addHabitPanel);
            OpenPanel(_mainPanel);
        }

        private void OpenPanel(PanelView view)
        {
            view.Open();
        }

        private void ClosePanel(PanelView view)
        {
            view.Close();
        }
    }
}