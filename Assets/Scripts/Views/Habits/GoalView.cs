using Views.General;

namespace Views.Habits
{
    public class GoalView : TextUpdater
    {
        public void UpdateGoal(int current, int goal)
        {
            string text = $"Today completed: {current}/{goal}";
            
            base.UpdateText(text);
        }
    }
}