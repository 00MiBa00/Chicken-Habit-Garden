using Views.General;

namespace Views.Habits
{
    public class StreakView : TextUpdater
    {
        public void UpdateStreak(int value)
        {
            string text = $"Streak: {value}";
            
            base.UpdateText(text);
        }
    }
}